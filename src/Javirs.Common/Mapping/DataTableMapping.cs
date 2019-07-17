using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mapping
{
    public class DataTableMapping : BaseMapping, IMapping
    {
        public DataTableMapping(DataTable table)
        { }


        protected override Dictionary<string, object> CreateValueDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
