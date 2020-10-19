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
            return BinaryFileManager.ReadObjectFromBinaryFile<ShoppingListManager>(filePath);
        }
    }
}
