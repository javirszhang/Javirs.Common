using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public class PostFileData:PostData,IPostFileData
    {
        public string FileName
        {
            get;
            set;
        }
    }
}
