using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using DomainLayer.Configuration;


namespace UnitTests
{
    [TestClass]
    public class DomainLayerConfigurationTests
    {
        private string fileName = @"..\..\testdata\testconfig.json";
        private void CreateConfigFile(string fileName, AppSettings settings)
        {
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(settings));
        }
        private void DeleteConfigFile(string fileName)
        {
            File.Delete(fileName);
        }

        [TestMethod]
        public void CanReadConfigFile()
        {
            //Arrange
            var appSettings = new Dictionary<string, string> { {"key1", "value1"}, { "key2", "value2" } };
            var connectionStrings = new Dictionary<string, string> { { "string1", "connection value1" }, { "string2", "connection value2" } };
            var settings = new AppSettings { appSettings = appSettings, connectionStrings = connectionStrings};
            this.CreateConfigFile(fileName, settings);
            //Act
            ConfigurationManager mngr = new ConfigurationManager(fileName);
            //Assert
            Assert.AreEqual<string>(mngr.AppSettings["key1"], appSettings["key1"]);
            Assert.AreEqual<string>(mngr.AppSettings["key2"], appSettings["key2"]);
            Assert.AreEqual<string>(mngr.ConnectionStrings["string1"], connectionStrings["string1"]);
            Assert.AreEqual<string>(mngr.ConnectionStrings["string2"], connectionStrings["string2"]);
            this.DeleteConfigFile(fileName);
        }

        [TestMethod]
        public void CanWriteConfigFile()
        {
            //Arrange
            var appSettings = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };
            var connectionStrings = new Dictionary<string, string> { { "string1", "connection value1" }, { "string2", "connection value2" } };
            var settings = new AppSettings { appSettings = appSettings, connectionStrings = connectionStrings };
            this.CreateConfigFile(fileName, settings);
            //Act
            ConfigurationManager mngr = new ConfigurationManager(fileName);
            mngr.AppSettings["key1"] = "new value1";
            mngr.AppSettings["key2"] = "new value2";
            mngr.ConnectionStrings.Add("new string", "new value3");
            mngr.SaveAsync().Wait();
            ConfigurationManager mngr2 = new ConfigurationManager(fileName);
            //Assert
            Assert.AreEqual<string>(mngr2.AppSettings["key1"], "new value1");
            Assert.AreEqual<string>(mngr2.AppSettings["key2"], "new value2");
            Assert.AreEqual<string>(mngr.ConnectionStrings["string1"], connectionStrings["string1"]);
            Assert.AreEqual<string>(mngr.ConnectionStrings["string2"], connectionStrings["string2"]);
            Assert.AreEqual<string>(mngr.ConnectionStrings["new string"], "new value3");
            this.DeleteConfigFile(fileName);
        }
    }
}
