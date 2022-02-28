using BlazorP1.Server.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Services
{
    public class EmailSender : IEmailSender
    {
        public string SendGridKey { get; set; }
        public DataContext _context { get; }

        public EmailSender(IOptions<EmailOptions> eo, DataContext context)
        {
            SendGridKey = eo.Value.SendGridKey;
           _context = context;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridKey, email, subject, message);
        }

        private async Task<bool> Execute(string sendGridKey, string email, string subject, string message)
        {
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("-", "Monkey Fights");
            var user = _context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user == null) return false;
            var to = new EmailAddress(email, user.Username);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, "");
            var response = await client.SendEmailAsync(msg);
            return true;

        }
    }
}
