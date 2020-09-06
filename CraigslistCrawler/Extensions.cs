using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CraigslistCrawler {
    public static class Extensions {

        public static void WriteToFile<T>(this IEnumerable<T> objects, string fileName) {
            using (var textWriter = File.CreateText($"{fileName}.json")) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(textWriter, objects);
            }            
        }
    }
}
