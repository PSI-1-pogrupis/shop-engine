using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    //temporary class to provide data for shopping history 
    public class TempData
    {
        ScannedListManager scannedListManager1 = new ScannedListManager();
        ScannedListManager scannedListManager2 = new ScannedListManager();

        public TempData()
        {
            scannedListManager1.AddItem(new ScannedItem("Miltai", 2m));
            scannedListManager1.AddItem(new ScannedItem("Sviestas", 1.8m));
            scannedListManager1.AddItem(new ScannedItem("Kava", 5.99m));
            ScannedListLibrary.AddList(scannedListManager1, DateTime.Now.AddDays(-1));

            ScannedItem scannedItem = new ScannedItem("Pienas", 1.15m);
            scannedItem.Discount = -0.5m;
            scannedListManager2.AddItem(scannedItem);
            scannedListManager2.AddItem(new ScannedItem("Obuoliai", 0.34m));
            ScannedListLibrary.AddList(scannedListManager2, DateTime.Now.AddDays(-2));
        }
    }
}
