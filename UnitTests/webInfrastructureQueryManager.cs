using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;

using MongoDB.Bson;

using DomainLayer.DataAccess;
using DomainLayer.Contact;
using DomainLayer.DataAccess.Query;
using web.Infrastructure;
using UnitTests.TestHelpers;
using System;
using System.Collections.Generic;


namespace UnitTests
{
    [TestClass]
    public class webInfrastructureQueryManager
    {
        private QueryStore<Contact> setupQueryStore(MockDataTable<Contact> data)
        {
            Mock<IDatabase> mockDb = new Mock<IDatabase>();
            mockDb.Setup(m => m.GetCollection<Contact>(It.IsAny<string>()))
                .Returns(data);

            Mock<IDbContext> mock = new Mock<IDbContext>();
            mock.Setup(m => m.GetDatabase())
                .Returns(mockDb.Object);

            return new QueryStore<Contact>(mock.Object, "ignored argument");
        }

        [TestMethod]
        public void CanFindByIdContact()
        {
            //Arrange
            var dataTable = new MockDataTable<Contact>();
            string firstName = "First", firstShurname = "FirstShur";
            string secondName = "Second", secondShurname = "SecondShur";
            var firstId = new ObjectId(1, 1, 1, 1);
            var secondId = new ObjectId(2, 2, 2, 2);
            var thirdId = new ObjectId(3, 3, 3, 3);

            dataTable.Insert(
                new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname }
                )
                .Wait();
            dataTable.Insert(
                new Contact { _Id = secondId, Name = secondName, Shurname = secondShurname }
                )
                .Wait();

            var queryStore = setupQueryStore(dataTable);
            var queryManager = new ContactsQueryManager(queryStore);

            //Act
            var result1 = queryManager.FindByIdAsync(firstId.ToString()).Result;
            var result2 = queryManager.FindByIdAsync(secondId.ToString()).Result;
            var result3 = queryManager.FindByIdAsync(thirdId.ToString()).Result;

            //Assert
            Assert.AreEqual<string>(result1.Name, firstName, "Names must be equal.");
            Assert.AreEqual<string>(result1.Id, firstId.ToString(), "Ids must be equal.");
            Assert.AreEqual<string>(result1.Shurname, firstShurname, "Shurnames must be equal");
            Assert.AreEqual<string>(result2.Name, secondName, "Names must be equal.");
            Assert.AreEqual<string>(result2.Id, secondId.ToString(), "Ids must be equal.");
            Assert.AreEqual<string>(result2.Shurname, secondShurname, "Shurnames must be equal");
            Assert.AreEqual(result3, null, "Must be null.");
        }

        [TestMethod]
        public void CanFindByNameContact()
        {
            //Arrange
            var dataTable = new MockDataTable<Contact>();
            string firstName = "First", firstShurname = "FirstShur";
            string secondName = "Second", secondShurname = "SecondShur";
            string missingName = "Third";
            var firstId = new ObjectId(1, 1, 1, 1);
            var secondId = new ObjectId(2, 2, 2, 2);
            var thirdId = new ObjectId(3, 3, 3, 3);

            dataTable.Insert(
                new Contact { _Id = firstId, Name = firstName, Shurname = firstShurname }
                )
                .Wait();
            dataTable.Insert(
                new Contact { _Id = secondId, Name = secondName, Shurname = secondShurname }
                )
                .Wait();
            dataTable.Insert(
                new Contact { _Id = thirdId, Name = secondName, Shurname = firstName }
                )
                .Wait();

            var queryStore = setupQueryStore(dataTable);
            var queryManager = new ContactsQueryManager(queryStore);
            //Act
            var result1_1 = queryManager.FindByNameAsync(m => m.Name, firstName).Result;
            var result1_2 = queryManager.FindByNameAsync(m => m.Shurname, firstShurname).Result;
            var result2_1 = queryManager.FindByNameAsync(m => m.Name, secondName).Result;
            var result2_2 = queryManager.FindByNameAsync(m => m.Shurname, secondShurname).Result;
            var result3_1 = queryManager.FindByNameAsync(m => m.Name, missingName).Result;
            var result4_1 = queryManager.FindByNameAsync(m => m.Name + " " + m.Shurname, firstName).Result;

            //Assert
            Assert.AreEqual<int>(result1_1.Count(), 1,"Must find exactly one objects.");
            Assert.AreEqual<string>(result1_1.ElementAt(0).Name, firstName, "Names must be equal.");
            Assert.AreEqual<int>(result1_2.Count(), 1, "Must find exactly one objects.");
            Assert.AreEqual<string>(result1_2.ElementAt(0).Name, firstName, "Names must be equal.");
            Assert.AreEqual<int>(result2_1.Count(), 2, "Must find exactly two objects.");
            Assert.AreEqual<int>(result2_2.Count(), 1, "Must find exactly one objects.");
            Assert.AreEqual<string>(result2_2.ElementAt(0).Name, secondName, "Names must be equal.");
            Assert.AreEqual<int>(result3_1.Count(), 0, "Must find exactly zero objects.");
            Assert.AreEqual<int>(result4_1.Count(), 2, "Must find exactly two objects.");
        }

        [TestMethod]
        public void CanTakeNContact()
        {
            //Arrange
            var dataTable = new MockDataTable<Contact>();
            var firstId = new ObjectId(1, 1, 1, 1);
            var secondId = new ObjectId(2, 2, 2, 2);
            var thirdId = new ObjectId(3, 3, 3, 3);

            dataTable.Insert(
                new Contact { _Id = firstId }
                )
                .Wait();
            dataTable.Insert(
                new Contact { _Id = secondId }
                )
                .Wait();
            dataTable.Insert(
                new Contact { _Id = thirdId }
                )
                .Wait();

            var queryStore = setupQueryStore(dataTable);
            var queryManager = new ContactsQueryManager(queryStore);
            //Act
            var result1_1 = queryManager.TakeNAsync(1, 0).Result;
            var result1_2 = queryManager.TakeNAsync(1, 1).Result;
            var result2_1 = queryManager.TakeNAsync(2, 0).Result;
            var result2_2 = queryManager.TakeNAsync(2, 1).Result;
            var result3_1 = queryManager.TakeNAsync(3, 0).Result;
            var result3_2 = queryManager.TakeNAsync(3, 1).Result;
            var result4_1 = queryManager.TakeNAsync(3, 3).Result;

            //Assert
            Assert.AreEqual<int>(result1_1.Count(), 1, "Must find exactly one objects.");
            Assert.AreEqual<int>(result1_2.Count(), 1, "Must find exactly one objects.");
            Assert.AreEqual<int>(result2_1.Count(), 2, "Must find exactly two objects.");
            Assert.AreEqual<int>(result2_2.Count(), 2, "Must find exactly two objects.");
            Assert.AreEqual<int>(result3_1.Count(), 3, "Must find exactly three objects.");
            Assert.AreEqual<int>(result3_2.Count(), 2, "Must find exactly two objects.");
            Assert.AreEqual<int>(result4_1.Count(), 0, "Must find exactly zero objects.");
        }
    }
}
