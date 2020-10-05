using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ShoppingList
{
    public static class ShoppingListResourceProcessor
    {
        private static string filePath = "shoppingList.bin";

        public static void SaveList(ShoppingListManager shoppingList)
        {
            BinaryFileManager.WriteToBinaryFile<ShoppingListManager>(filePath, shoppingList);
        }

        public static ShoppingListManager LoadList()
        {
            return BinaryFileManager.ReadFromBinaryFile<ShoppingListManager>(filePath);
        }
    }
}
