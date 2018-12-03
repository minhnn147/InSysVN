using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Utils
{
    public class SendEmail
    {
        public class EmailConfig
        {
            public EmailConfig()
            {
                SMTP_Host = "smtp.gmail.com";
                Port = 587;
                isHtml = true;
            }
            public string FromMail;
            public string FromMailPass;
            public string ToMail;
            public string Subject;
            public string Content;
            public bool isHtml = true;
            public string SMTP_Host = "smtp.gmail.com";
            public int Port = 587;
            public string Attachments;
        }
        public static bool SendGmailMail(EmailConfig config)
        {
            try
            {
                //Code
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(config.FromMail);
                SmtpClient smtp = new SmtpClient();
                smtp.Port = config.Port;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(mail.From.Address, config.FromMailPass);
                smtp.Host = config.SMTP_Host;

                //recipient
                mail.To.Add(new MailAddress(config.ToMail));

                if (!string.IsNullOrEmpty(config.Attachments))
                {
                    foreach (var e in config.Attachments.Split(new string[] { "[|]" }, StringSplitOptions.None))
                    {
                        try
                        {
                            Attachment attachment = new Attachment(e, MediaTypeNames.Application.Octet);
                            ContentDisposition disposition = attachment.ContentDisposition;
                            disposition.CreationDate = File.GetCreationTime(e);
                            disposition.ModificationDate = File.GetLastWriteTime(e);
                            disposition.ReadDate = File.GetLastAccessTime(e);
                            disposition.FileName = Path.GetFileName(e);
                            disposition.Size = new FileInfo(e).Length;
                            disposition.DispositionType = DispositionTypeNames.Attachment;
                            mail.Attachments.Add(attachment);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }

                mail.IsBodyHtml = config.isHtml;
                mail.Subject = config.Subject;
                mail.Body = config.Content;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }
        }
    }
}
