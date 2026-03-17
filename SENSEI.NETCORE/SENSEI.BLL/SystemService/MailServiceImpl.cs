using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using SENSEI.BLL.SystemService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SENSEI.BLL.SystemService
{
    public class MailServiceImpl: IMailService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _sender;

        #region SendGrid
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;
        #endregion

        public MailServiceImpl(IConfiguration configuration)
        {
            var tenantId = configuration["SenseiMailService:TenantId"];
            var clientId = configuration["SenseiMailService:ClientId"];
            var clientSecret = configuration["SenseiMailService:ClientSecret"];
            _sender = configuration["SenseiMailService:Sender"];

            var credential = new ClientSecretCredential(
                tenantId,
                clientId,
                clientSecret
            );

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            _graphClient = new GraphServiceClient(credential, scopes);

            _apiKey = configuration["SendGrid:ApiKey"];
            _senderEmail = configuration["SendGrid:SenderEmail"];
            _senderName = configuration["SendGrid:SenderName"];
        }

        #region Google Mail

        public async Task<bool> SendGoogleMail(string receiverMail, string mailSubject, string mailBody)
        {
            bool status = false;
            string senderEmail = "thirangamicrosoft@gmail.com";
            string senderPassword = "svxg mlqh bavo wtfh";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail, "SJC Class"),
                Subject = mailSubject,
                Body = mailBody,
                IsBodyHtml = true
            };

            mail.To.Add("malshan.rathnayake@iits.biz");
            mail.To.Add(receiverMail);

            try
            {
                smtpClient.Send(mail);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }

            return status;
        }

        public async Task<bool> SendGoogleMailForMultiple(List<string> receiverMail, string mailSubject, string mailBody)
        {
            bool status = false;
            string senderEmail = "thirangamicrosoft@gmail.com";
            string senderPassword = "svxg mlqh bavo wtfh";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress(senderEmail, "SJC Class"),
                Subject = mailSubject,
                Body = mailBody,
                IsBodyHtml = true
            };

            mail.To.Add("malshan.rathnayake@iits.biz");
            foreach (var receiver in receiverMail)
                mail.To.Add(receiver);

            try
            {
                smtpClient.Send(mail);
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }

            return status;
        }

        #endregion

        #region Graph Mail

        public async Task<bool> SendGraphMail(string receiver, string subject, string body)
        {
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = body
                },
                ToRecipients = new List<Recipient>()
                {
                    new Recipient
                    {
                        EmailAddress = new Microsoft.Graph.Models.EmailAddress
                        {
                            Address = receiver
                        }
                    }
                }
            };

            try
            {
                await _graphClient
                .Users[_sender]
                .SendMail
                .PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = true
                });

            }catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SendOutlookMail(string receiverMail, string subject, string body)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var smtpClient = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        "no-reply@senseijapanesecenter.com",
                        "noreply@123"
                    )
                };

                var mail = new MailMessage
                {
                    From = new MailAddress("no-reply@senseijapanesecenter.com", "Sensei Japanese Center"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(receiverMail);

                await smtpClient.SendMailAsync(mail);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region SendGrid Mail

        public async Task<bool> SendEmailSendGrid(string toEmail, string subject, string htmlContent)
        {
            try
            {
                var client = new SendGridClient(_apiKey);

                var from = new SendGrid.Helpers.Mail.EmailAddress(_senderEmail, _senderName);
                var to = new SendGrid.Helpers.Mail.EmailAddress(toEmail);

                var msg = MailHelper.CreateSingleEmail(
                    from,
                    to,
                    subject,
                    plainTextContent: "",
                    htmlContent: htmlContent
                );

                var response = await client.SendEmailAsync(msg);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        #endregion
    }
}
