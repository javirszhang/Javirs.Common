using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Javirs.Common.Mails
{
    public class MailBody
    {
        public string Content { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
        public MailPriority Priority { get; set; }
        public Encoding Encoding { get; set; }
    }
}
