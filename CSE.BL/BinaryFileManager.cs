using System;
using System.Collections.Generic;
using System.IO;

namespace CSE.BL
{
    public class BinaryFileManager
    {
        /* Writes the given object instance to a binary file.*/
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite)
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /* Reads an object instance from a binary file and returns it.
         * If file does not exist, return empty list.*/
        public static List<T> ReadFromBinaryFile<T>(string filePath)
        {
            try
            {
                if (new FileInfo(filePath).Length != 0)
                {
                    using (Stream stream = File.Open(filePath, FileMode.Open))
                    {
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        return (List<T>)binaryFormatter.Deserialize(stream);
                    }
                }
                else
                {
                    return new List<T>();
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}
