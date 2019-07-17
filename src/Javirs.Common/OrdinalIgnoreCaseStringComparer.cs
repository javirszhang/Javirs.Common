using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    public class OrdinalIgnoreCaseStringComparer : StringComparer
    {
        public override int Compare(string x, string y)
        {
            if (x == null)
            {
                return -1;
            }
            return x.CompareTo(y);
        }

        public override bool Equals(string x, string y)
        {
            return x.Equals(y, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
