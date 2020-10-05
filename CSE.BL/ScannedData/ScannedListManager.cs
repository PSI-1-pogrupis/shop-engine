using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public class ScannedListManager
    {
        private List<ScannedItem> scannedItems;

        public ScannedListManager()
        {
            scannedItems = new List<ScannedItem>();
        }

        public ScannedItem GetItem(int index)
        {
            if (!CheckIndex(index))
                return null;

            return scannedItems[index];
        }

        public bool AddItem(ScannedItem item)
        {
            bool ok = false;

            if (item != null)
            {
                scannedItems.Add(item);
                ok = true;
            }

            return ok;
        }

        private bool CheckIndex(int index)
        {
            bool ok = false;

            if ((index >= 0) && index < scannedItems.Count)
            {
                ok = true;
            }

            return ok;
        }
    }
}
