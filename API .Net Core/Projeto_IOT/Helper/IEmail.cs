using System.Collections.Generic;
using System.Threading.Tasks;
using Projeto_IOT.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Projeto_IOT.Helper
{
    public interface IEmail
    {
        Task<Response> SendEmail(EmailEntity mail);
        EmailEntity Notify(List<Usuario> userList);
        EmailEntity NotifyUser(string emailUser); 
        EmailEntity CriacaoCadastroAUsuario(Usuario user, string link);
    }

    public class EmailEntity
    {
        public EmailEntity()
        {
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAddress> MailTo { get; set; }
        public List<EmailAddress> MailCc { get; set; }
        public List<EmailAddress> MailCco { get; set; }
    }
}
