using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.eCharts
{
    public class Mark
    {
        public MarkType[] data { get; set; }
    }

    public class MarkType
    {
        public string name { get; set; }
        public string type { get; set; }
    }
}
