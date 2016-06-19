using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using System.Linq;
using UnitTests.TestHelpers;

using Moq;

using DomainLayer.Contact;
using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Record;
using DomainLayer.Identity;
using web.Infrastructure;


namespace UnitTests
{
    [TestClass]
    public class webInfrastructureRecordManager
    {
        internal class FakeContact : IContact
        {
            private IDictionary<string, string> _emails;
            public IDictionary<string, string> Emails
            {
                get
                {
                    if (_emails == null)
                    {
                        _emails = new Dictionary<string, string>();
                    }
                    return _emails;
                }
                set
                {
                    _emails = value != null ? new Dictionary<string, string>(value) : null;
                }
            }

            public string Id { get; set; }

            public string Name { get; set; }

            public string OrganizationId { get; set; }

            public string Patronymic { get; set; }

            private IDictionary<string, string> _phones;
            public IDictionary<string, string> Phones
            {
                get
                {
                    if (_phones == null)
                    {
                        _phones = new Dictionary<string, string>();
                    }
                    return _phones;
                }
                set
                {
                    _phones = value != null ? new Dictionary<string, string>(value) : null;
                }
            }

            private IPosition _position;
            public IPosition Position
            {
                get
                {
                    if (_position == null)
                    {
                        _position = new EnPosition();
                    }
                    return _position;
                }
                set
                {
                    _position = value != null ? new EnPosition(value) : null;
                }
            }

            public string Remarks { get; set; }

            public string Shurname { get; set; }
        }

        private RecordStore<IContact, TUser> setupRecordStore<TUser>(MockDataTable<IContact> data)
        {
            Mock<IDatabase> mockDb = new Mock<IDatabase>();
            mockDb.Setup(m => m.GetCollection<IContact>(It.IsAny<string>()))
                .Returns(data);

            Mock<IDbContext> mock = new Mock<IDbContext>();
            mock.Setup(m => m.GetDatabase())
                .Returns(mockDb.Object);

            return new RecordStore<IContact, TUser>(mock.Object, "ignored argument");
        }

        [TestMethod]
        public void CanCreateContact()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstShurname = "FirstShurname";
            var doc = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var dataTable = new MockDataTable<IContact>();
            var recordStore = setupRecordStore<IAppUser>(dataTable);
            var recordManager = new ContactsRecordManager(recordStore);
            
            //Act
            recordManager.CreateAsync(doc, new AppUser()).Wait();
            
            //Assert
            Assert.AreEqual(1, dataTable.GetCollection().Count(), "Must be only one object.");
            Assert.AreEqual(doc, dataTable.FindOneById(firstId).Result, "Stored and retrived objects must be equal.");
        }

        [TestMethod]
        public void CanDeleteContact()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstShurname = "FirstShurname";
            var doc = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var dataTable = new MockDataTable<IContact>();
            var recordStore = setupRecordStore<IAppUser>(dataTable);
            var recordManager = new ContactsRecordManager(recordStore);
            recordManager.CreateAsync(doc, new AppUser()).Wait();

            //Act
            var result1 = recordManager.DeleteAsync(doc, new AppUser());
            result1.Wait();
            var result2 = recordManager.DeleteAsync(doc, new AppUser());
            result2.Wait();

            //Assert
            Assert.AreEqual(1, result1.Result, "Must delete exactly one object.");
            Assert.AreEqual(0, result2.Result, "Must delete exactly zero object.");
            Assert.AreEqual(0, dataTable.GetCollection().Count(), "Collection must be empty.");
        }

        [TestMethod]
        public void CanDeleteSeveralContacts()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstShurname = "FirstShurname";
            var doc1 = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc2 = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc3 = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var dataTable = new MockDataTable<IContact>();

            dataTable.Insert(doc1).Wait();
            dataTable.Insert(doc2).Wait();
            dataTable.Insert(doc3).Wait();

            var recordStore = setupRecordStore<IAppUser>(dataTable);
            var recordManager = new ContactsRecordManager(recordStore);

            //Act
            var result1 = recordManager.DeleteAsync(doc1, new AppUser());
            result1.Wait();
            var result2 = recordManager.DeleteAsync(doc2, new AppUser());
            result2.Wait();

            //Assert
            Assert.AreEqual(3, result1.Result, "Must delete exactly three objects.");
            Assert.AreEqual(0, result2.Result, "Must delete exactly zero object.");
            Assert.AreEqual(0, dataTable.GetCollection().Count(), "Collection must be empty.");
        }

        [TestMethod]
        public void CanUpdateDocument()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstShurname = "FirstShurname";
            string secondId = "2";
            string updateName = "New Name";
            var doc1 = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc2 = new FakeContact { Id = secondId };
            var dataTable = new MockDataTable<IContact>();
            dataTable.Insert(doc1).Wait();
            var recordStore = setupRecordStore<IAppUser>(dataTable);

            //Act
            doc1.Name = updateName;
            var result1 = recordStore.UpdateAsync(doc1, new AppUser());
            result1.Wait();
            var result2 = recordStore.UpdateAsync(doc2, new AppUser());
            result2.Wait();

            //Assert
            Assert.AreEqual(1, result1.Result, "Must udate exactly one objects.");
            Assert.AreEqual(0, result2.Result, "Must update exactly zero object.");
            Assert.AreEqual(updateName,
                dataTable.
                GetCollection().
                FirstOrDefault(d => d.Id.Equals(firstId)).
                Name, "Name must be updated.");
        }

        [TestMethod]
        public void CanUpdateFieldsDocument()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstShurname = "first description";
            string secondId = "2", secondName = "Second", secondShurname = "second description";
            string thirdId = "3", thirdName = "Third", thirdShurname = "third description";
            string updateName = "New Name";
            var doc1 = new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc2 = new FakeContact { Id = secondId, Name = secondName, Shurname = secondShurname };
            var doc3 = new FakeContact { Id = thirdId, Name = thirdName, Shurname = thirdShurname };
            var dataTable = new MockDataTable<IContact>();
            dataTable.Insert(doc1).Wait();
            dataTable.Insert(doc2).Wait();
            dataTable.Insert(doc3).Wait();
            var recordStore = setupRecordStore<IAppUser>(dataTable);
            var recordManger = new ContactsRecordManager(recordStore);

            //Act
            var result = recordStore.UpdateAsync(d => true, f => f.Name, updateName, new AppUser());
            result.Wait();

            //Assert
            Assert.AreEqual(3, result.Result, "Must udate exactly three objects.");
            Assert.IsTrue(dataTable.GetCollection().Any(d => d.Name.Equals(updateName)), "Name must be updated.");
        }
    }
}
