using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public interface IPostData
    {
        string Name { get; set; }
        object Value { get; set; }
    }
}
