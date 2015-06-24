using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLMaker2.Properties;

namespace SQLMaker2
{
    static class Zones
    {
        private static string _item;
        public static string Item
        {
            get
            {
                return _item;
            }
            set
            {
                if (_item == value) return;
                if (!Items.Contains(value)) throw new Exception("値が不正です");
                _item = value;
            }
        }

        private static string[] _items;
        public static string[] Items
        {
            get
            {
                if (_items == null) _items = CreateItems();
                return _items;
            }
        }

        private static string[] CreateItems()
        {
            int minZone = 1;
            int maxZone = Settings.Default.ZoneMax;
            int length = maxZone - minZone + 1;
            string[] r = new string[length+1];
            r[0] = "";
            for (int i = 0; i<length; i++)
            {
                int zone = minZone + i;
                r[i+1] = zone.ToString();
            }
            return r;
        }
    }
}
