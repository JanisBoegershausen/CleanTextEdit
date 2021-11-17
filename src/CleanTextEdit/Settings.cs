using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CleanTextEdit
{
    class Settings
    {
        public static Settings current;

        public string startupPath { get; set; }
        public float opacity { get; set; }

        public Settings ()
        {
            startupPath = "";
            opacity = 0.5f;
        }

        public static void SaveCurrent(string path)
        {
            Console.WriteLine("Saving Settings");
            if (!System.IO.File.Exists(path))
                System.IO.File.Create(path).Close();
            
            string jsonString = JsonSerializer.Serialize<Settings>(current);
            System.IO.File.WriteAllText(path, jsonString);
        }

        public static bool TryLoadFromFile(string path)
        {
            if (String.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return false;

            string jsonString = System.IO.File.ReadAllText(path);
            Settings loadedSettings = JsonSerializer.Deserialize<Settings>(jsonString);

            current = loadedSettings;
            return true;
        }
    }
}
