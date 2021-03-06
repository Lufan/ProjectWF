﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using System.Linq;
using UnitTests.TestHelpers;

using MongoDB.Bson;

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
        private RecordStore<Contact, TUser> setupRecordStore<TUser>(MockDataTable<Contact> data)
        {
            Mock<IDatabase> mockDb = new Mock<IDatabase>();
            mockDb.Setup(m => m.GetCollection<Contact>(It.IsAny<string>()))
                .Returns(data);

            Mock<IDbContext> mock = new Mock<IDbContext>();
            mock.Setup(m => m.GetDatabase())
                .Returns(mockDb.Object);

            return new RecordStore<Contact, TUser>(mock.Object, "ignored argument");
        }

        [TestMethod]
        public void CanCreateContact()
        {
            //Arrange
            string firstName = "First", firstShurname = "FirstShurname";
            var firstId = new ObjectId(1, 1, 1, 1);
            var doc = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var dataTable = new MockDataTable<Contact>();
            var recordStore = setupRecordStore<IAppUser>(dataTable);
            var recordManager = new ContactsRecordManager(recordStore);
            
            //Act
            recordManager.CreateAsync(doc, new AppUser()).Wait();
            
            //Assert
            Assert.AreEqual(1, dataTable.GetCollection().Count(), "Must be only one object.");
            Assert.AreEqual(doc, dataTable.FindOneById(firstId.ToString()).Result, "Stored and retrived objects must be equal.");
        }

        [TestMethod]
        public void CanDeleteContact()
        {
            //Arrange
            string firstName = "First", firstShurname = "FirstShurname";
            var firstId = new ObjectId(1, 1, 1, 1);
            var doc = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var dataTable = new MockDataTable<Contact>();
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
            string firstName = "First", firstShurname = "FirstShurname";
            var firstId = new ObjectId(1, 1, 1, 1);
            var doc1 = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc2 = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc3 = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var dataTable = new MockDataTable<Contact>();

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
            string firstName = "First", firstShurname = "FirstShurname";
            var firstId = new ObjectId(1, 1, 1, 1);
            var secondId = new ObjectId(2, 2, 2, 2);
            string updateName = "New Name";
            var doc1 = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc2 = new Contact { _Id = secondId };
            var dataTable = new MockDataTable<Contact>();
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
                FirstOrDefault(d => d.Id.Equals(firstId.ToString())).
                Name, "Name must be updated.");
        }

        [TestMethod]
        public void CanUpdateFieldsDocument()
        {
            //Arrange
            string firstName = "First", firstShurname = "first description";
            string secondName = "Second", secondShurname = "second description";
            string thirdName = "Third", thirdShurname = "third description";
            var firstId = new ObjectId(1, 1, 1, 1);
            var secondId = new ObjectId(2, 2, 2, 2);
            var thirdId = new ObjectId(3, 3, 3, 3);
            string updateName = "New Name";
            var doc1 = new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname };
            var doc2 = new Contact { _Id = secondId, Name = secondName, Shurname = secondShurname };
            var doc3 = new Contact { _Id = thirdId, Name = thirdName, Shurname = thirdShurname };
            var dataTable = new MockDataTable<Contact>();
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
