using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public class PostDataAttribute : Attribute
    {
        public string Name { get; set; }
        public bool UrlEncode { get; set; }
        public string Charset { get; set; }
    }
}
