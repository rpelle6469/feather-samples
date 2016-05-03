﻿using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace PrecompiledViewsCrawler.Utilities
{
    public class JsonLogger : IJsonLogger
    {
        public void SaveToFile(object data, string fileName)
        {
            string path = this.MapLogFilePath(fileName);
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }

            string oldJson = File.ReadAllText(path).Trim();
            IList<object> result = JsonConvert.DeserializeObject<List<object>>(oldJson);
            if (result == null)
            {
                result = new List<object>();
            }

            result.Add(data);

            string newJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            using (StreamWriter file = File.CreateText(path))
            {
                file.Write(newJson);
            }
        }

        private string MapLogFilePath(string fileName)
        {
            return HostingEnvironment.MapPath(LogDirectory + fileName);
        }

        private const string LogDirectory = "~/App_Data/Sitefinity/Logs/";
    }
}
