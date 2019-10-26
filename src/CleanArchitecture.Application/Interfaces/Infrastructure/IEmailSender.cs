namespace CleanArchitecture.Application.Interfaces.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        Task SendAsync(string subject, string plainTextContent, ICollection<string> toEmailAddresses);
    }
}
