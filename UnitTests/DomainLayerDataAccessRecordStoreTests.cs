using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        private RecordStore<TestPOCO, TUser> setupQueryStore<TUser>(MockDataTable<TestPOCO> data)
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
        public void TestMethod1()
        {
            //
            // TODO: добавьте здесь логику теста
            //
        }
    }
}
