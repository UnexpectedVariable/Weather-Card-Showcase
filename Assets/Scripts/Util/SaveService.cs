using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Util
{
    internal class SaveService
    {
        public static async Task SaveAsync(string json, string file = "Save")
        {
            string path = $"{Data.DEFAULT_SAVE_URI}{file}.json";

            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteAsync(json);
            }
        }

        public static async Task<string> LoadAsync(string file = "Save")
        {
            string path = $"{Data.DEFAULT_SAVE_URI}{file}.json";

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch(FileNotFoundException)
            {
                return null;
            }
        }
    }
}
