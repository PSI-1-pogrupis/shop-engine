using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public static class ScannedListLibrary
    {
        private static Dictionary<DateTime, ScannedListManager> allLists = new Dictionary<DateTime, ScannedListManager>();
        public static decimal TotalSum { get; set; }
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

        public static List<ScannedListManager> GetLists(DateTime index)
        {
            List<ScannedListManager> list = new List<ScannedListManager>();
            DateTime date = index.Date;
            TotalSum = 0m;

            foreach(var item in AllLists)
            {
                if (item.Key.Date == date) 
                {
                    list.Add(item.Value);
                    TotalSum += item.Value.TotalSum;
                }                    
            }
            return list;

        }

        public static bool AddList(ScannedListManager list)
        {
            bool ok = false;

            if (list.GetCount() != 0 && list != null)
            {
                allLists.Add(DateTime.Now, list);
                ok = true;
            }

            return ok;
        }

        public static bool AddList(ScannedListManager list, DateTime date)
        {
            bool ok = false;

            if (list.GetCount() != 0 && list != null)
            {
                allLists.Add(date, list);
                ok = true;
            }

            return ok;
        }
    }
}
