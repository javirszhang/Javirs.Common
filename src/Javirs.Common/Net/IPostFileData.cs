using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public interface IPostFileData:IPostData
    {
        string FileName { get; set; }
    }
}
