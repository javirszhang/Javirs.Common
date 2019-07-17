using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.eCharts
{
    public class Series
    {
        public string name { get; set; }
        public string type { get; set; }
        public object[] data { get; set; }
        public Mark markPoint { get; set; }
        public Mark markLine { get; set; }
        public string radius { get; set; }
        public string[] center { get; set; }
    }
}
