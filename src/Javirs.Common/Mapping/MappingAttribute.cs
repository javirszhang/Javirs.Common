using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mapping
{
    public class MappingAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Ignore { get; set; }
    }
}
