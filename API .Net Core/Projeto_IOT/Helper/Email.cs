using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Projeto_IOT.Models;
using Projeto_IOT.Repository.IRepository;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Projeto_IOT.Helper
{
    public class Email : IEmail
    {
        private readonly IConfiguration config;
        private ISendGridClient _client;

        private readonly IUsuarioRepository _repo;

        private static Usuario userCreator;

        public Email(IConfiguration config, ISendGridClient client, IUsuarioRepository repo)
        {
            this.config = config;
            _client = client;
            _repo = repo;
        }

        private string GetDate(DateTime? date)
        {
            CultureInfo ci = new CultureInfo("pt-BR");
            return date?.ToString("dd-MMM-yyyy");
        }

        public async Task<Response> SendEmail(EmailEntity mail)
        {
             var apiKey = config.GetSection("SendGridApiKey").Value;
            this._client = new SendGridClient(apiKey);

            var email = new SendGridMessage()
            {
                From = new EmailAddress(config["MAILFROM"], $"IOT Mackenzie"),
                Subject = mail.Subject,
                HtmlContent = mail.Body
            };

            if (mail.MailTo != null)
            {

                email.AddTos(mail.MailTo.Distinct().ToList());
            }
            if (mail.MailCc != null)
            {
                email.AddCcs(mail.MailCc.Distinct().ToList());

            }
            if (mail.MailCco != null)
            {

                email.AddBccs(mail.MailCco.Distinct().ToList());
            }

            _client.UrlPath = "https://api.sendgrid.com/v3/mail/send";
            var response = await _client.SendEmailAsync(email).ConfigureAwait(false);
            return response;

        }

        
        public EmailEntity Notify(List<Usuario> userList) 
        {
            List<EmailEntity> emailList = new List<EmailEntity>();
            EmailEntity email = new EmailEntity();
            List<EmailAddress> emailToList = new List<EmailAddress>();

            if (userList != null)
            {
                foreach (var emailTo in userList)
                {
                    emailToList.Add(new EmailAddress(emailTo.Email));

                }                
                var color = "color:#005bcb";
                
                    email.Body = $@"<div>
                                                Olá <b>Alerta!!</b>,
                                                <br>
                                                <br>
                                                Foi detectada uma quantidade consideravel de gás ou fumaça em sua residencia.
                                                <br>
                                                <br>
                                                <br>
                                                Recomendamos que o usuario verifique o possível perigo 
                                            </div>";
               
                email.Subject = "Gas ou Fumaça Detectados!!!";
                email.MailTo = emailToList;
                emailList.Add(email);
            }

            return email;
        }
        public EmailEntity NotifyUser(string emailUser) 
        {
            List<EmailEntity> emailList = new List<EmailEntity>();
            EmailEntity email = new EmailEntity();
            List<EmailAddress> emailToList = new List<EmailAddress>();

            if (emailUser != null)
            {
          
                emailToList.Add(new EmailAddress(emailUser));

                              
                var color = "color:#005bcb";
                
                    email.Body = $@"<div>
                                                Olá <b>Alerta!!</b>,
                                                <br>
                                                <br>
                                                Foi detectada uma quantidade consideravel de gás ou fumaça em sua residencia.
                                                <br>
                                                <br>
                                                <br>
                                                Recomendamos que o usuario verifique o possível perigo 
                                            </div>";
               
                email.Subject = "Gas ou Fumaça Detectados!!!";
                email.MailTo = emailToList;
                emailList.Add(email);
            }

            return email;
        }
         public EmailEntity CriacaoCadastroAUsuario(Usuario user, string link)
        {
            EmailEntity email = new EmailEntity();            
            List<EmailAddress> emailAddress = new List<EmailAddress>();
            emailAddress.Add(new EmailAddress(user.Email));

            var color = "color:#005bcb";

            if (true)
            {
                email.Body = $@"<div>
                                                Olá, Você realizou seu cadastro no sistema de Agentes Públicos.
                                                <br>                                              
                                                <br>
                                                <br>                                                                                             
                                            </div>";
            }
            else
            {
                email.Body = $@"<div>
                                                Olá, Você solicitou seu cadastro no sistema de Agentes Públicos.
                                                <br>
                                                <br>
                                                No momento sua solicitação foi enviada aos administradores para análise e aguarda aprovação.<br>
                                                Em breve você receberá um novo e-mail informando a situação de seu cadastro.
                                                <br>
                                                <br>
                                                <br>                                                
                                            </div>";
            }

            email.MailTo = emailAddress;
            email.Subject = "Interação com Agentes Públicos - Solicitação de Cadastro";

            return email;
        }
    }
}