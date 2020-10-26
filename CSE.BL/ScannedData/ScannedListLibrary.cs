using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public static class ScannedListLibrary
    {
        private static Dictionary<DateTime, ScannedListManager> allLists = new Dictionary<DateTime, ScannedListManager>();
        public static Dictionary<DateTime, ScannedListManager> AllLists 
        {
            get
            {
                return allLists;
            }
            set
            {
                allLists = value;
            }
        }

        public static ScannedListManager GetList(DateTime index)
        {
            if(!AllLists.ContainsKey(index))
                AllLists[index] = new ScannedListManager();
            return AllLists[index]; 

        }

        public static bool AddList(ScannedListManager list)
        {
            bool ok = false;

            if (list.GetCount() != 0 && list != null)
            {
                DateTime dateTime = DateTime.Now.Date;
                if (allLists.ContainsKey(dateTime))//temporararily, there can only be one list per day
                    allLists.Remove(dateTime);

                allLists.Add(DateTime.Now.Date, list);
                ok = true;
            }

            return ok;
        }

        public static bool AddList(ScannedListManager list, DateTime date)
        {
            bool ok = false;

            if (list.GetCount() != 0 && list != null)
            {
                if (allLists.ContainsKey(date.Date))//temporararily, there can only be one list per day
                    allLists.Remove(date.Date);

                allLists.Add(date.Date, list);
                ok = true;
            }

            return ok;
        }
    }
}
