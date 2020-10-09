using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public class ScannedListLibrary
    {
        private List<ScannedListManager> allLists;

        public ScannedListLibrary()
        {
            allLists = new List<ScannedListManager>();
        }

        public ScannedListManager GetList(int index)
        {
            if (!CheckIndex(index))
                return null;

            return allLists[index];
        }

        public bool AddList(ScannedListManager list)
        {
            bool ok = false;

            if (list != null)
            {
                allLists.Add(list);
                ok = true;
            }

            return ok;
        }

        private bool CheckIndex(int index)
        {
            bool ok = false;

            if ((index >= 0) && index < allLists.Count)
            {
                ok = true;
            }

            return ok;
        }
    }
}
