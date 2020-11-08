using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "login@mail.ru")); //тут e-mail отправителя
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync("login@mail.ru", "password");//аутентификация аккаунта e-mail'а отправителя, здесь нужно указать свой e-mail и пароль
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
