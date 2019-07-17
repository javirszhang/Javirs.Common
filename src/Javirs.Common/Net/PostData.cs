using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Net
{
    public class PostData:IPostData
    {
        public string Name
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
    }
}
