using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mails
{
    public class MailEventArgs : EventArgs
    {
        public Exception InnerException { get; set; }

        public MailAddress To { get; set; }
        public MailBody Body { get; set; }

    }
}
