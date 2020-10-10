using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ShoppingList
{
    public static class ShoppingListResourceProcessor
    {
        private static string filePath = "shoppingList.bin";

        public static void SaveLists(List<ShoppingListManager> shoppingList)
        {
            BinaryFileManager.WriteToBinaryFile(filePath, shoppingList);
        }

        public static List<ShoppingListManager> LoadLists()
        {
            return BinaryFileManager.ReadFromBinaryFile<List<ShoppingListManager>>(filePath);
        }
    }
}
