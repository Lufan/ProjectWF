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
            //Assert.AreEqual(0, dataTable.GetCollection().Count(), "Collection must be empty.");
        }
    }
}
