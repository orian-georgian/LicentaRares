using System;
using System.Linq;
using License.Model;

namespace License
{
    public class AuthentificationLogic
    {
        public static User CreateUser(Member member)
        {
            var user = new User();

            user.Email = member.Email;
            user.Password = CreatePassword();
            user.Name = member.FullName;
            user.AuthRole = "Member";
            user.AuthToken = string.Empty;

            return user;
        }

        public static void SendNotificationEmail(User user)
        {
            var toAddress = user.Email;
            var mailSubject = "User password for localhost site";
            var mailBody = "Your account information is: \n Email: "
                + user.Email + " \nPassword: "
                + user.Password
                + " \n Please log in and change your password!";

            SmtpService.SendEmail(toAddress, mailSubject, mailBody);
        }

        private static string CreatePassword()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

    }
}
