﻿using Backend.Application.Common.Mailing;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Backend.Infrastructure.Mailing;

public class SmtpMailService(IOptions<MailSettings> settings, ILogger<SmtpMailService> logger)
    : IMailService
{
    private readonly MailSettings _settings = settings.Value;

    public async Task SendAsync(MailRequest request)
    {
        try
        {
            var email = new MimeMessage();

            // From
            email.From.Add(
                new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From)
            );

            // To
            foreach (var address in request.To)
                email.To.Add(MailboxAddress.Parse(address));

            // Reply To
            if (!string.IsNullOrEmpty(request.ReplyTo))
                email.ReplyTo.Add(new MailboxAddress(request.ReplyToName, request.ReplyTo));

            // Bcc
            if (request.Bcc != null)
            {
                foreach (
                    var address in request.Bcc.Where(bccValue =>
                        !string.IsNullOrWhiteSpace(bccValue)
                    )
                )
                    email.Bcc.Add(MailboxAddress.Parse(address.Trim()));
            }

            // Cc
            if (request.Cc != null)
            {
                foreach (
                    var address in request.Cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue))
                )
                    email.Cc.Add(MailboxAddress.Parse(address.Trim()));
            }

            // Headers
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                    email.Headers.Add(header.Key, header.Value);
            }

            // Content
            var builder = new BodyBuilder();
            email.Sender = new MailboxAddress(
                request.DisplayName ?? _settings.DisplayName,
                request.From ?? _settings.From
            );
            email.Subject = request.Subject;
            builder.HtmlBody = request.Body;

            // Create the file attachments for this e-mail message
            if (request.AttachmentData != null)
            {
                foreach (var attachmentInfo in request.AttachmentData)
                    builder.Attachments.Add(attachmentInfo.Key, attachmentInfo.Value);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }
}
