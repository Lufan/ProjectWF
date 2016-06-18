﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Moq;

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

        private QueryStore<IContact> setupQueryStore(MockDataTable<IContact> data)
        {
            Mock<IDatabase> mockDb = new Mock<IDatabase>();
            mockDb.Setup(m => m.GetCollection<IContact>(It.IsAny<string>()))
                .Returns(data);

            Mock<IDbContext> mock = new Mock<IDbContext>();
            mock.Setup(m => m.GetDatabase())
                .Returns(mockDb.Object);

            return new QueryStore<IContact>(mock.Object, "ignored argument");
        }

        [TestMethod]
        public void CanFindByIdContact()
        {
            //Arrange
            var dataTable = new MockDataTable<IContact>();
            string firstId = "1", firstName = "First", firstShurname = "FirstShur";
            string secondId = "2", secondName = "Second", secondShurname = "SecondShur";
            string thirdId = "3";

            dataTable.Insert(
                new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname }
                )
                .Wait();
            dataTable.Insert(
                new FakeContact { Id = secondId, Name = secondName, Shurname = secondShurname }
                )
                .Wait();

            var queryStore = setupQueryStore(dataTable);
            var queryManager = new ContactsQueryManager(queryStore);

            //Act
            var result1 = queryManager.FindByIdAsync(firstId).Result;
            var result2 = queryManager.FindByIdAsync(secondId).Result;
            var result3 = queryManager.FindByIdAsync(thirdId).Result;

            //Assert
            Assert.AreEqual<string>(result1.Name, firstName);
            Assert.AreEqual<string>(result1.Id, firstId);
            Assert.AreEqual<string>(result1.Shurname, firstShurname);
            Assert.AreEqual<string>(result2.Name, secondName);
            Assert.AreEqual<string>(result2.Id, secondId);
            Assert.AreEqual<string>(result2.Shurname, secondShurname);
            Assert.AreEqual(result3, null);
        }

        [TestMethod]
        public void CanFindByNameContact()
        {
            //Arrange
            var dataTable = new MockDataTable<IContact>();
            string firstId = "1", firstName = "First", firstShurname = "FirstShur";
            string secondId = "2", secondName = "Second", secondShurname = "SecondShur";
            string thirdId = "3";
            string missingName = "Third";

            dataTable.Insert(
                new FakeContact { Id = firstId, Name = firstName, Shurname = firstShurname }
                )
                .Wait();
            dataTable.Insert(
                new FakeContact { Id = secondId, Name = secondName, Shurname = secondShurname }
                )
                .Wait();
            dataTable.Insert(
                new FakeContact { Id = thirdId, Name = secondName, Shurname = firstName }
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
            Assert.AreEqual<int>(result1_1.Count(), 1);
            Assert.AreEqual<string>(result1_1.ElementAt(0).Name, firstName);
            Assert.AreEqual<int>(result1_2.Count(), 1);
            Assert.AreEqual<string>(result1_2.ElementAt(0).Name, firstName);
            Assert.AreEqual<int>(result2_1.Count(), 2);
            Assert.AreEqual<int>(result2_2.Count(), 1);
            Assert.AreEqual<string>(result2_2.ElementAt(0).Name, secondName);
            Assert.AreEqual<int>(result3_1.Count(), 0);
            Assert.AreEqual<int>(result4_1.Count(), 2);
        }

        [TestMethod]
        public void CanTakeNContact()
        {
            //Arrange
            var dataTable = new MockDataTable<IContact>();
            string firstId = "1";
            string secondId = "2";
            string thirdId = "3";

            dataTable.Insert(
                new FakeContact { Id = firstId }
                )
                .Wait();
            dataTable.Insert(
                new FakeContact { Id = secondId }
                )
                .Wait();
            dataTable.Insert(
                new FakeContact { Id = thirdId }
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
            Assert.AreEqual<int>(result1_1.Count(), 1);
            Assert.AreEqual<int>(result1_2.Count(), 1);
            Assert.AreEqual<int>(result2_1.Count(), 2);
            Assert.AreEqual<int>(result2_2.Count(), 2);
            Assert.AreEqual<int>(result3_1.Count(), 3);
            Assert.AreEqual<int>(result3_2.Count(), 2);
            Assert.AreEqual<int>(result4_1.Count(), 0);
        }
    }
}