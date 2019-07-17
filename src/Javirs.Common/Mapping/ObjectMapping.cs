using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mapping
{
    public class ObjectMapping : BaseMapping, IMapping
    {
        public ObjectMapping(object obj)
        {

        }

        protected override Dictionary<string, object> CreateValueDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
