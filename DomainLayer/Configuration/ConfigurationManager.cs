using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace DomainLayer.Configuration
{
    public class ConfigurationManager
    {
        private readonly string _fileName;
        private AppSettings _settings;
        
        public ConfigurationManager(string fileName)
        {
            _fileName = fileName;
        }

        public async Task SaveAsync()
        {
            using (var writer = new StreamWriter(_fileName))
            {
                await writer.WriteLineAsync((new JavaScriptSerializer()).Serialize(_settings));
            }
        }

        public async Task ReloadAsync()
        {
            AppSettings t = new AppSettings();
            if (File.Exists(_fileName))
            {
                using (var reader = File.OpenText(_fileName))
                {
                    var fileText = await reader.ReadToEndAsync();
                    t = (new JavaScriptSerializer()).Deserialize<AppSettings>(fileText);
                }
                
            }
            else
            {
                throw new FileNotFoundException(_fileName);
            }
            this._settings = t;
        }
        
        public Dictionary<string, string> AppSettings
        {
            get
            {
                if (this._settings == null) { this.ReloadAsync().Wait(); }
                return _settings.appSettings;
            }
        }
        
        public Dictionary<string, string> ConnectionStrings
        {
            get
            {
                if (this._settings == null) { this.ReloadAsync().Wait(); }
                return _settings.connectionStrings;
            }
        }
    }
}
