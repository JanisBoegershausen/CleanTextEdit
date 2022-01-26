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
        public bool autosave { get; set; }
        public bool alwaysOnTop { get; set; }
        public float opacity { get; set; }
        public bool playTypingSound { get; set; }

        public Settings ()
        {
            startupPath = "";
            opacity = 0.5f;
            autosave = false;
            alwaysOnTop = false;
            playTypingSound = true;
        }

        /// <summary>
        /// Save the current settings to the given path. 
        /// </summary>
        public static void SaveCurrent(string path)
        {
            Console.WriteLine("Saving Settings");
            if (!System.IO.File.Exists(path))
                System.IO.File.Create(path).Close();
            
            string jsonString = JsonSerializer.Serialize<Settings>(current);
            System.IO.File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Try to load settings from a given file path. 
        /// </summary>
        /// <returns>Rather or not the loading succeeded. </returns>
        public static bool TryLoadFromFile(string path)
        {
            // Check if the path is valid
            if (String.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return false;

            // Deserialize the content of the file. (Should check if the content of the file is valid)
            string jsonString = System.IO.File.ReadAllText(path);
            Settings loadedSettings = JsonSerializer.Deserialize<Settings>(jsonString);

            // Apply the settings
            current = loadedSettings;
            return true;
        }
    }
}
