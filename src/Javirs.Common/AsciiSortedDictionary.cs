using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    public class AsciiSortedDictionary<TValue> : SortedDictionary<string, TValue>
    {
        public AsciiSortedDictionary() : base(new StringKeyAsciiComparer())
        {

        }
        protected class StringKeyAsciiComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y)
                {
                    return 0;
                }
                int len = x.Length < y.Length ? x.Length : y.Length;
                for (int i = 0; i < len; i++)
                {
                    char xChar = x[i];
                    char yChar = y[i];
                    if (xChar == yChar)
                    {
                        continue;
                    }
                    else if (xChar > yChar)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                return x.Length > len ? -1 : 1;
            }
        }
    }
}
