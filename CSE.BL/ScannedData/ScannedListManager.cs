using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public class ScannedListManager
    {
        public List<ScannedItem> ScannedItems { get; set; }
        

        public ScannedListManager()
        {
            ScannedItems = new List<ScannedItem>();
        }

        public ScannedItem GetItem(int index)
        {
            if (!CheckIndex(index))
                return null;

            return ScannedItems[index];
        }

        public bool AddItem(ScannedItem item)
        {
            bool ok = false;

            if (item != null)
            {
                ScannedItems.Add(item);
                ok = true;
            }

            return ok;
        }

        private bool CheckIndex(int index)
        {
            bool ok = false;

            if ((index >= 0) && index < ScannedItems.Count)
            {
                ok = true;
            }

            return ok;
        }
    }
}
