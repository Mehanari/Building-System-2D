using System;
using System.IO;
using BuildingSystem2D.Core.JsonConverters;
using Newtonsoft.Json;
using UnityEngine;

namespace BuildingSystem2D.Core
{
    public static class ConstructionIO
    {
        private static readonly string DefaultPath = Path.Combine(
            Application.persistentDataPath,
            "construction.json"
        );
        
        public static void Save(Construction construction)
        {
            Save(construction, DefaultPath);
        }
        
        public static void Save(Construction construction, string path)
        {
            try
            {
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                string jsonString = JsonConvert.SerializeObject(construction, new Vector2Converter());
                
                File.WriteAllText(path, jsonString);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving inventory: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Checks if a save file exists for a default path. 
        /// </summary>
        /// <returns></returns>
        public static bool Exists()
        {
            return File.Exists(DefaultPath);
        }
        
        public static Construction Load()
        {
            return Load(DefaultPath);
        }
        
        public static Construction Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return new Construction();
                }
                
                string jsonString = File.ReadAllText(path);
                
                var construction = JsonConvert.DeserializeObject<Construction>(jsonString, new Vector2Converter());
                
                return construction;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading inventory: {ex.Message}");
                throw;
            }
        }
    }
}