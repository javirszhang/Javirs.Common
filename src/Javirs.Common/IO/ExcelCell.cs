using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.IO
{
    /// <summary>
    /// excel表格单元格
    /// </summary>
    public class ExcelCell
    {
        public object Value { get; set; }
        public int FontSize { get; set; }
        public int FontFamily { get; set; }
        public int FontWeight { get; set; }
        public short Color { get; set; }
    }
}
