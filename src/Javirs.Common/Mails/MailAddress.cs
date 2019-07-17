using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mails
{
    public class MailAddress
    {
        public string UserName { get; set; }
        public string Domain { get; set; }

        public override string ToString()
        {
            return string.Concat(UserName, "@", Domain);
        }
    }
}
