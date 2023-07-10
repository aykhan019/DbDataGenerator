using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Entities.Models;

namespace DataGenerator.Helpers
{
    public class FileHelper<T> where T : class
    {
        public static List<T> GetDataFromJson(string path)
        {
            // Read the JSON file content
            string jsonContent = File.ReadAllText(path);

            // Deserialize JSON into a list of User objects
            List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonContent);

            return data;
        }

        public static List<string> GetDateFromTxtFile(string path)
        {
            // Read all lines from the text file
            var lines = File.ReadAllLines(path);

            return lines.ToList();
        }

        public static void WriteToFile(string filePath, string[] lines)
        {
            using (var stream = new StreamWriter(filePath, false))
            {
                foreach (var line in lines)
                {
                    stream.WriteLine(line);
                }
            }
        }
    }
}
