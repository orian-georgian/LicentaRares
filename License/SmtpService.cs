using System;
using System.Net;
using System.Net.Mail;


namespace License
{
    public class SmtpService
    {
        public static void SendEmail(string toAddress, string mailSubject, string mailBody)
        {
            try
            {
                var fromAddress = "marincas.andreea.maria@gmail.com";
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);

                mail.From = new MailAddress(fromAddress);

                if (toAddress == "")
                {
                    mail.To.Add(fromAddress);
                }
                else
                {
                    mail.To.Add(toAddress);
                }

                mail.Subject = mailSubject;
                mail.Body = mailBody;

                NetworkCredential login = new NetworkCredential(fromAddress, "mam~1992");
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = login;
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
