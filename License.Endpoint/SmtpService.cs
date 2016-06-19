using System;
using System.Net;
using System.Net.Mail;


namespace License.Endpoint
{
    public class SmtpService
    {
        public static void SendEmail(string toAddress, string mailSubject, string mailBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);

                mail.From = new MailAddress("marincas.andreea.maria@gmail.com");
                mail.To.Add(toAddress);
                mail.Subject = mailSubject;
                mail.Body = mailBody;

                NetworkCredential login = new NetworkCredential("marincas.andreea.maria@gmail.com", "mam~1992");
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
