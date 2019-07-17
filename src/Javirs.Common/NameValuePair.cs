using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common
{
    public class NameValuePair
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class NameValueCollection : ICollection<NameValuePair>, IEnumerable<NameValuePair>, ICollection, IEnumerable
    {
        private int capable = 4;
        private int _length = 0;
        private NameValuePair[] nvps;
        public NameValueCollection()
        {
            nvps = new NameValuePair[capable];
        }
        public void Add(NameValuePair item)
        {
            if (_length >= capable)
            {
                NameValuePair[] temp = new NameValuePair[_length + capable];
                Array.Copy(nvps, 0, temp, 0, nvps.Length);
                nvps = temp;
            }

            nvps[this._length++] = item;
        }

        public void Clear()
        {
            _length = 0;
            nvps = new NameValuePair[capable];
        }

        public bool Contains(NameValuePair item)
        {
            foreach (NameValuePair nvp in nvps)
            {
                if (nvp.Name == item.Name && nvp.Value == item.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(NameValuePair[] array, int arrayIndex)
        {
            if (array != null)
            {
                Array.Copy(nvps, 0, array, arrayIndex, array.Length);
            }
        }

        public int Count
        {
            get { return _length; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(NameValuePair item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<NameValuePair> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(nvps, index, array, 0, array.Length);
        }

        public bool IsSynchronized
        {
            get;
            private set;
        }
        private static object lockObj = new object();
        public object SyncRoot
        {
            get { return lockObj; }
        }
    }
}
