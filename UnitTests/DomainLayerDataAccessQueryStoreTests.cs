using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;

using DomainLayer.DataAccess;
using DomainLayer.DataAccess.Query;
using UnitTests.TestHelpers;

namespace UnitTests
{
    /// <summary>
    /// Tests for DataAccess.QueryStore
    /// </summary>
    [TestClass]
    public class DomainLayerDataAccessQueryStoreTests
    {
        internal class TestPOCO : IDocument
        {
            public string Id { get; set; }
            public MongoDB.Bson.ObjectId _Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private QueryStore<TestPOCO> setupQueryStore(MockDataTable<TestPOCO> data)
        {
            Mock<IDatabase> mockDb = new Mock<IDatabase>();
            mockDb.Setup(m => m.GetCollection<TestPOCO>(It.IsAny<string>()))
                .Returns(data);

            Mock<IDbContext> mock = new Mock<IDbContext>();
            mock.Setup(m => m.GetDatabase())
                .Returns(mockDb.Object);

            return new QueryStore<TestPOCO>(mock.Object, "ignored argument");
        }

        [TestMethod]
        public void CanFindByIdDocument()
        {
            //Arrange
            var dataTable = new MockDataTable<TestPOCO>();
            string firstId = "1", firstName = "First", firstDescription = "first description";
            string secondId = "2", secondName = "Second", secondDescription = "second description";
            string thirdId = "3";

            dataTable.Insert(
                new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription }
                )
                .Wait();
            dataTable.Insert(
                new TestPOCO { Id = secondId, Name = secondName, Description = secondDescription }
                )
                .Wait();

            var queryStore = setupQueryStore(dataTable);
            //Act
            var result1 = queryStore.FindByIdAsync(firstId).Result;
            var result2 = queryStore.FindByIdAsync(secondId).Result;
            var result3 = queryStore.FindByIdAsync(thirdId).Result;
            //Assert
            Assert.AreEqual<string>(result1.Name, firstName);
            Assert.AreEqual<string>(result1.Id, firstId);
            Assert.AreEqual<string>(result1.Description, firstDescription);
            Assert.AreEqual<string>(result2.Name, secondName);
            Assert.AreEqual<string>(result2.Id, secondId);
            Assert.AreEqual<string>(result2.Description, secondDescription);

            Assert.AreEqual(result3, null);
        }

        [TestMethod]
        public void CanFindByOtherPropertiesDocument()
        {
            //Arrange
            var dataTable = new MockDataTable<TestPOCO>();
            string firstId = "1", firstName = "First", firstDescription = "first description";
            string secondId = "2", secondName = "Second", secondDescription = "second description";
            string thirdId = "3", thirdName = "Second", thirdDescription = "third description";
            string missingName = "Third";

            dataTable.Insert(
                new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription }
                )
                .Wait();
            dataTable.Insert(
                new TestPOCO { Id = secondId, Name = secondName, Description = secondDescription }
                )
                .Wait();
            dataTable.Insert(
                new TestPOCO { Id = thirdId, Name = thirdName, Description = thirdDescription }
                )
                .Wait();

            var queryStore = setupQueryStore(dataTable);
            //Act
            var result1_1 = queryStore.FindByFieldAsync(m => m.Name, firstName).Result;
            var result1_2 = queryStore.FindByFieldAsync(m => m.Description, firstDescription).Result;
            var result2_1 = queryStore.FindByFieldAsync(m => m.Name, secondName).Result;
            var result2_2 = queryStore.FindByFieldAsync(m => m.Id, secondId).Result;
            var result3_1 = queryStore.FindByFieldAsync(m => m.Name, missingName).Result;

            //Assert
            Assert.AreEqual<int>(result1_1.Count(), 1);
            Assert.AreEqual<string>(result1_1.ElementAt(0).Name, firstName);
            Assert.AreEqual<int>(result1_2.Count(), 1);
            Assert.AreEqual<string>(result1_2.ElementAt(0).Name, firstName);
            Assert.AreEqual<int>(result2_1.Count(), 2);
            Assert.AreEqual<int>(result2_2.Count(), 1);
            Assert.AreEqual<string>(result2_2.ElementAt(0).Name, secondName);
            Assert.AreEqual<int>(result3_1.Count(), 0);
        }

        [TestMethod]
        public void GetAsQuerableReturnQuerable()
        {
            //Arrange
            var dataTable = new MockDataTable<TestPOCO>();
            string firstId = "1", firstName = "First", firstDescription = "first description";
            string secondId = "2", secondName = "Second", secondDescription = "second description";

            dataTable.Insert(
                new TestPOCO { Id = firstId, Name = firstName, Description = firstDescription }
                )
                .Wait();
            dataTable.Insert(
                new TestPOCO { Id = secondId, Name = secondName, Description = secondDescription }
                )
                .Wait();
            var queryStore = setupQueryStore(dataTable);
            //Act
            var result = queryStore.GetAsQueryable;
            //Assert
            Assert.IsInstanceOfType(result, typeof(IQueryable<TestPOCO>));
            Assert.AreEqual(2, result.Count());
        }
    }
}
