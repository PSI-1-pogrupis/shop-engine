using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    //temporary class to provide data for shopping history 
    public static class TempData
    {
        public static ScannedListManager scannedListManager1 = new ScannedListManager();
        public static ScannedListManager scannedListManager2 = new ScannedListManager();

        public static bool Loaded { get; set; }

        public static void LoadData()
        {
            scannedListManager1.AddItem(new ScannedItem("Miltai", 2m, ShopTypes.IKI));
            scannedListManager1.AddItem(new ScannedItem("Sviestas", 1.8m, ShopTypes.IKI));
            scannedListManager1.AddItem(new ScannedItem("Kava", 5.99m, ShopTypes.IKI));
            ScannedListLibrary.AddList(scannedListManager1, DateTime.Now.AddDays(-1));

            ScannedItem scannedItem = new ScannedItem("Pienas", 1.15m, ShopTypes.IKI);
            scannedItem.Discount = -0.5m;
            scannedListManager2.AddItem(scannedItem);
            scannedListManager2.AddItem(new ScannedItem("Obuoliai", 0.34m, ShopTypes.IKI));
            ScannedListLibrary.AddList(scannedListManager2, DateTime.Now.AddDays(-1));
        }
    }
}
