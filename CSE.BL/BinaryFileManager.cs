using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSE.BL
{
    public class BinaryFileManager
    {
        /* Writes the given object instance to a binary file.
         * Parameter "append" - if value = false, the file will be overwritten if it already exists. If true the contents will be appended to the file.*/

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /* Reads an object instance from a binary file and returns it.*/
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
