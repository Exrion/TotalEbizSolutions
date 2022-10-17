using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TeBS_CC_PluginDev.Services
{
    public class EmailService
    {
        private static readonly string _apiKey = "";
        private static readonly SendGridClient client = new SendGridClient(_apiKey);
        private static readonly EmailAddress _fromMailAddress = new EmailAddress("titus.lim@totalebizsolutions.com", "TeBS_CC");

        public async void SendTemplateMail(
                string toEmail, 
                string toName,
                string templateID,
                EmailModel templateData
            )
        {
            var msg = MailHelper.CreateSingleTemplateEmail(
                _fromMailAddress,
                new EmailAddress(toEmail, toName),
                templateID,
                templateData
                );
            var result = await client.SendEmailAsync(msg);

            /*var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = "Sending with Twilio SendGrid is Fun",
                PlainTextContent = "and easy to do anywhere, especially with C#"
            };
            msg.AddTo(new EmailAddress(toEmail, toName));
            var response = await client.SendEmailAsync(msg);*/
        }

        public async Task SendMail(List<string> lstToAddress, string subject, string templateId, dynamic templateData)
        {
            var client = new SendGridClient(_apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(_fromMailAddress);
            msg.SetSubject(subject);
            lstToAddress.ForEach(x => {
                msg.AddTo(new EmailAddress(x));
            });
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);

            var response = await client.SendEmailAsync(msg);
        }
    }
}
