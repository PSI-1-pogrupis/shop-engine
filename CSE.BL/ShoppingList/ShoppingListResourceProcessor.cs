using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ShoppingList
{
    public static class ShoppingListResourceProcessor
    {
        public static void SaveList(ShoppingListManager shoppingList)
        {
            BinaryFileManager.WriteToBinaryFile<ShoppingListManager>("data.bin", shoppingList);
        }

        public static ShoppingListManager LoadList()
        {
            return BinaryFileManager.ReadFromBinaryFile<ShoppingListManager>("data.bin");
        }
    }
}
