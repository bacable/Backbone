using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace ProximityND.Backbone.IO
{
    /// <summary>
    /// Loads a single JSON file into an object of the specified type, or a folder matching the given
    /// extensions into a list of objects of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of object the file will load into.</typeparam>
    public static class Loader<T>
    {
        /// <summary>
        /// Loads all files matching the extensions list in the given folder into a list of objects of the given type.
        /// </summary>
        /// <param name="folderPath">The path to the folder.</param>
        /// <param name="fileExtensions">List of file extensions that are valid for loading in this folder.</param>
        /// <param name="searchOption">Just the initial folder (TopDirectoryOnly), or including subfolders (AllDirectories)</param>
        /// <returns></returns>
        public static async Task<List<T>> LoadFolder(string folderPath, List<string> fileExtensions, SearchOption searchOption)
        {
            List<T> results = new List<T>();

            var fileList = Directory.EnumerateFiles(folderPath, "*.*", searchOption)
                                    .Where(s => fileExtensions.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));

            foreach(var file in fileList)
            {
                var obj = await Load(file);
                results.Add(obj);
            }

            return results;
        }

        public static async Task<T> Load(string filename)
        {
            T result = default(T);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            using (FileStream openStream = File.OpenRead(filename))
            {
                result = await JsonSerializer.DeserializeAsync<T>(openStream);
            }

            return result;
        }
    }
}
