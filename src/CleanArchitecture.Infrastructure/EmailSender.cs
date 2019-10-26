namespace CleanArchitecture.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CleanArchitecture.Application.Interfaces.Infrastructure;
    //using System;
    //using System.Linq;
    //using SendGrid;
    //using SendGrid.Helpers.Mail;

    public class EmailSender : IEmailSender
    {
        public async Task SendAsync(string subject, string plainTextContent, ICollection<string> toEmailAddresses)
        {
            //string apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            //var client = new SendGridClient(apiKey);

            //var from = new EmailAddress("test@example.com", "Example User");

            //var recipients = toEmailAddresses
            //    .Select(emailAddress => new EmailAddress(emailAddress, string.Empty))
            //    .ToList();

            //SendGridMessage sendGridMessage = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, subject, plainTextContent, string.Empty);
            //Response response = await client.SendEmailAsync(sendGridMessage);
        }
    }
}