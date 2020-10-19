namespace CSE.BL.ShoppingList
{
    public static class ShoppingListResourceProcessor
    {
        private static string filePath = "shoppingList.bin";

        public static void SaveLists(List<ShoppingListManager> shoppingList)
        {
            BinaryFileManager.WriteToBinaryFile(filePath, shoppingList);
        }

        public static ShoppingListManager LoadList()
        {
            return BinaryFileManager.ReadObjectFromBinaryFile<ShoppingListManager>(filePath);
        public static List<ShoppingListManager> LoadLists()
        {
            try
            {
                return BinaryFileManager.ReadFromBinaryFile<List<ShoppingListManager>>(filePath);
            }
            catch(Exception)
            {
                throw;
            }
            
        }
    }
}
