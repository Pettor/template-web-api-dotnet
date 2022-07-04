using WebApiTemplate.Application.Common.Interfaces;

namespace WebApiTemplate.Application.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request);
}