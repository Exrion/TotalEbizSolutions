// using SendGrid's C# Library
// https://github.com/sendgrid/sendgrid-csharp
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using TeBS_CC_ConsoleTest;

namespace Example
{
    internal class Example
    {
        private static void Main()
        {
            Execute2().Wait();
        }

        static async Task Execute()
        {
            var apiKey = "";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("titus.lim@totalebizsolutions.com", "TeBS_CC");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        static async Task Execute2()
        {
            string _apiKey = "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";
            SendGridClient client = new SendGridClient(_apiKey);

            EmailModel templateData = new EmailModel()
            {
                subject = "New Booking - Awaiting Approval",
                prehead = "A customer has sent a booking request.",
                body = "Follow the link to approve or deny the booking request.",
                url = "https://org7f3624ff.crm5.dynamics.com/main.aspx?appid=9e6787fd-be42-ed11-bba3-0022485adde6&pagetype=entityrecord&etn=cr90d_rentalrecord&id=c5a16ffd-f14f-41d4-a627-2cbac62768a1"
            };

            var msg = MailHelper.CreateSingleTemplateEmail(
                new EmailAddress("titus.lim@totalebizsolutions.com", "TeBS_CC"),
                new EmailAddress("titus.lim@totalebizsolutions.com", "Titus Lim"),
                "d-112daae2df47449384fbbe02e3aa8bf4",
                templateData
                );
            var result = await client.SendEmailAsync(msg);
        }
    }
}