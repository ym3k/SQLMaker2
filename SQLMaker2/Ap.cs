using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLMaker2
{
    class Ap
    {
        /// <summary>
        /// APの名前（漢字表記、ApName）と対応するデータベース上の表記（ローマ字、ApCode）
        /// </summary>
        private string _apName;
        private string _apCode;

        public string ApName
        {
            get
            {
                return _apName;
            }
            set
            {
                _apName = value;
            }
        }

        public string ApCode
        {
            get
            {
                return _apCode;
            }
            set
            {
                _apCode = value;
            }
        }

        public override string ToString()
        {
            return ApName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            string a = ToString();
            string b = obj.ToString();
            bool r = (a == b);
            return r;
        }

        static public bool operator ==(Ap a, Ap b)
        {
            if ((object)a == null) return ((object)b == null);
            return a.Equals(b);
        }

        static public bool operator !=(Ap a, Ap b)
        {
            if ((object)a == null) return !((object)b == null);
            return !a.Equals(b);
        }
    }
}
