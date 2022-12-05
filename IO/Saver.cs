using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backbone.IO
{
    public static class Saver<T>
    {
        public static async Task Save(T obj, string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            // TODO: Check if file exists and throw up a warning window first
            // before overwriting
            using (FileStream createStream = File.Create(filename))
            {
                await JsonSerializer.SerializeAsync(createStream, obj, options);
            }
        }
    }
}
