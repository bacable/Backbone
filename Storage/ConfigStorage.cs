using System;
using System.Xml.Serialization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;

namespace Backbone.Storage
{
    public class ConfigStorage
    {

        public static void Save<T>(T saveObject, string fileName)
        {

            XmlSerializer serializer;

            try
            {
                var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);

                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(writer, saveObject);
                    }
                }

                Console.WriteLine(fileName + " saved.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static T Load<T>(string filename)
        {
            T returnObj = default(T);

            try
            {
                var isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);

                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        returnObj = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return returnObj;
        }
    }
}
