using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Moq;

using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Record;
using UnitTests.TestHelpers;

namespace UnitTests
{
    /// <summary>
    /// Tests for DataAccess.RecordStore
    /// </summary>
    [TestClass]
    public class DomainLayerDataAccessRecordStoreTests
    {
        internal class TestPOCO : IDocument
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        internal class TestUser {  }

        private RecordStore<TestPOCO, TUser> setupRecordStore<TUser>(MockDataTable<TestPOCO> data)
        {
            Mock<IDatabase> mockDb = new Mock<IDatabase>();
            mockDb.Setup(m => m.GetCollection<TestPOCO>(It.IsAny<string>()))
                .Returns(data);

            Mock<IDbContext> mock = new Mock<IDbContext>();
            mock.Setup(m => m.GetDatabase())
                .Returns(mockDb.Object);

            return new RecordStore<TestPOCO, TUser>(mock.Object, "ignored argument");
        }

        [TestMethod]
        public void CanCreateDocument()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstDescription = "first description";
            var doc = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var dataTable = new MockDataTable<TestPOCO>();
            var recordStore = setupRecordStore<TestUser>(dataTable);

            //Act
            recordStore.CreateAsync(doc, new TestUser()).Wait();
            //Assert
            Assert.AreEqual(1, dataTable.GetCollection().Count(), "Must be only one object.");
            Assert.AreEqual(doc, dataTable.FindOneById(firstId).Result, "Stored and retrived objects must be equal.");
        }

        [TestMethod]
        public void CanDeleteDocument()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstDescription = "first description";
            var doc = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var dataTable = new MockDataTable<TestPOCO>();
            dataTable.Insert(doc).Wait();
            var recordStore = setupRecordStore<TestUser>(dataTable);

            //Act
            var result1 = recordStore.DeleteAsync(doc, new TestUser());
            result1.Wait();
            var result2 = recordStore.DeleteAsync(doc, new TestUser());
            result2.Wait();

            //Assert
            Assert.AreEqual(1, result1.Result, "Must delete exactly one object.");
            Assert.AreEqual(0, result2.Result, "Must delete exactly zero object.");
            Assert.AreEqual(0, dataTable.GetCollection().Count(), "Collection must be empty.");
        }

        [TestMethod]
        public void CanDeleteSeveralDocument()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstDescription = "first description";
            var doc1 = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var doc2 = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var doc3 = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var dataTable = new MockDataTable<TestPOCO>();
            dataTable.Insert(doc1).Wait();
            dataTable.Insert(doc2).Wait();
            dataTable.Insert(doc3).Wait();
            var recordStore = setupRecordStore<TestUser>(dataTable);

            //Act
            var result1 = recordStore.DeleteAsync(doc1, new TestUser());
            result1.Wait();
            var result2 = recordStore.DeleteAsync(doc2, new TestUser());
            result2.Wait();

            //Assert
            Assert.AreEqual(3, result1.Result, "Must delete exactly tree objects.");
            Assert.AreEqual(0, result2.Result, "Must delete exactly zero object.");
            Assert.AreEqual(0, dataTable.GetCollection().Count(), "Collection must be empty.");
        }

        [TestMethod]
        public void CanUpdateDocument()
        {
            //Arrange
            string firstId = "1", firstName = "First", firstDescription = "first description";
            string secondId = "2";
            string updateName = "New Name";
            var doc = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var doc2 = new TestPOCO { Id = secondId };
            var dataTable = new MockDataTable<TestPOCO>();
            dataTable.Insert(doc).Wait();
            var recordStore = setupRecordStore<TestUser>(dataTable);

            //Act
            doc.Name = updateName;
            var result1 = recordStore.UpdateAsync(doc, new TestUser());
            result1.Wait();
            var result2 = recordStore.UpdateAsync(doc2, new TestUser());
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
            string firstId = "1", firstName = "First", firstDescription = "first description";
            string secondId = "2", secondName = "Second", secondDescription = "second description";
            string thirdId = "3", thirdName = "Third", thirdDescription = "third description";
            string updateName = "New Name";
            var doc1 = new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription };
            var doc2 = new TestPOCO { Id = secondId, Name = secondName, Description = secondDescription };
            var doc3 = new TestPOCO { Id = thirdId, Name = thirdName, Description = thirdDescription };
            var dataTable = new MockDataTable<TestPOCO>();
            dataTable.Insert(doc1).Wait();
            dataTable.Insert(doc2).Wait();
            dataTable.Insert(doc3).Wait();
            var recordStore = setupRecordStore<TestUser>(dataTable);

            //Act
            var result = recordStore.UpdateAsync(d => true, f => f.Name, updateName, new TestUser());
            result.Wait();

            //Assert
            Assert.AreEqual(3, result.Result, "Must udate exactly three objects.");
            Assert.IsTrue(dataTable.GetCollection().Any(d => d.Name.Equals(updateName)), "Name must be updated.");
        }
    }
}
