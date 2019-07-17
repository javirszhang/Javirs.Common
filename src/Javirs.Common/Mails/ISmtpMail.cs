using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Javirs.Common.Mails
{
    public interface ISmtpMail
    {
        bool SendTo(MailBody body, params MailAddress[] addrCollection);
    }
}
