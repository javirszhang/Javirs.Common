using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.eCharts
{
    public class eChart
    {
        public Title title { get; set; }
        public Tooltip tooltip { get; set; }
        public Legend legend { get; set; }
        public bool calculable { get; set; }
        public Axis xAxis { get; set; }
        public Axis yAxis { get; set; }
        public Series[] series { get; set; }
        public Toolbox toolbox { get; set; }
    }
}
