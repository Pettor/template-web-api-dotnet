using Backend.Application.Common.Interfaces;

namespace Backend.Application.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request);
}
