using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    static class ScannedListResourceProcessor
    {
        private static string filePath = "shoppingHistory.bin";

        public static void SaveList(ScannedListLibrary scannedLists)
        {
            BinaryFileManager.WriteToBinaryFile<ScannedListLibrary>(filePath, scannedLists);
        }

        public static ScannedListLibrary LoadList()
        {
            return BinaryFileManager.ReadFromBinaryFile<ScannedListLibrary>(filePath);
        }
    }
}
