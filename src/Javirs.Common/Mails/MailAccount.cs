using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mails
{
    public class MailAccount
    {
        public MailAccount(MailAddress addr, string password, string smtpServer, int port)
        {
            this.Address = addr;
            this.Password = password;
            this.SmtpServer = smtpServer;
            this.Port = port;
        }
        public string Password { get; set; }
        public MailAddress Address { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        
    }
}
