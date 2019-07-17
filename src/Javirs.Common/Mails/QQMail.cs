using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Javirs.Common.Mails
{
    public class QQMail:ISmtpMail
    {
        private MailAccount _account;

        public MailAccount Account
        {
            get { return _account; }
        }
        public QQMail(MailAccount account)
        {
            this._account = account;
        }

        public bool SendTo(MailBody body,params MailAddress[] addrCollection)
        {
            if (addrCollection == null || addrCollection.Length <= 0)
                return false;
            try
            {
                SmtpClient client = new SmtpClient(this._account.SmtpServer, this._account.Port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(this._account.Address.ToString(), this._account.Password);

                System.Net.Mail.MailMessage myEmail = new System.Net.Mail.MailMessage();
                myEmail.From = new System.Net.Mail.MailAddress(this._account.Address.ToString(), body.Subject, body.Encoding);
                foreach(MailAddress addr in addrCollection)
                    myEmail.To.Add(addr.ToString());
                myEmail.Subject = body.Subject;
                myEmail.IsBodyHtml = body.IsHtml;
                myEmail.Body = body.Content;
                myEmail.Priority = body.Priority;
                myEmail.BodyEncoding = body.Encoding;
                


                client.Send(myEmail);
            }
            catch (Exception ex)
            {
                OnErrorOccurs(ex);
                return false;
            }
            return true;
        }
        private void OnErrorOccurs(Exception ex)
        {
            MailEventArgs args = new MailEventArgs();
            args.InnerException = ex;
            if (ErrorOccurs != null)
                ErrorOccurs(this, args);
        }
        public event Action<object, EventArgs> ErrorOccurs;
    }
}
