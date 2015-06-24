using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLMaker2.Properties;

namespace SQLMaker2
{
    static class Aps
    {
        private static Ap _item;
        public static Ap Item
        {
            get
            {
                return _item;
            }
            set
            {
                if (_item == value) return;
                if (!Items.Contains(value))
                {
                    throw new Exception("値が不正です");
                }
                _item = value;
            }
        }

        private static Ap[] _items;
        public static Ap[] Items
        {
            get
            {
                if (_items == null) _items = CreateItems();
                return _items;
            }
        }


        private static Ap[] CreateItems()
        {
            string[] records = GetRecords();
            char[] separator = { ':' };

            Ap[] r = new Ap[records.Length+1];
            Ap apnull = new Ap();
            apnull.ApCode = "";
            apnull.ApName = "";
            r[0] = apnull;
            for (int i =0; i < records.Length; i++)
            {
                string[] field = records[i].Split(separator);
                if (field.Length != 2)
                {
                    throw new Exception("APの指定が間違っています");
                }

                Ap ap = new Ap();
                ap.ApName = field[0];
                ap.ApCode = field[1];

                r[i+1] = ap;
            }
            return r;
        }

        private static string[] GetRecords()
        {
            char[] separator = { '\n' };
            //string text = "東京:tokyo\n大阪:osaka";
            string text = Settings.Default.ApList;
            string[] records = text.Replace("\r\n", "\n").Split(separator);
            return records;
        }
    }
}
