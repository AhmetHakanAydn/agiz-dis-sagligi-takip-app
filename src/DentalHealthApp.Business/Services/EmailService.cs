using DentalHealthApp.Core.Interfaces;
using System.Net.Mail;
using System.Net;

namespace DentalHealthApp.Business.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;

    public EmailService()
    {
        // In a real application, these would come from configuration
        _smtpServer = "smtp.gmail.com";
        _smtpPort = 587;
        _smtpUsername = "your-email@gmail.com";
        _smtpPassword = "your-app-password";
        _fromEmail = "your-email@gmail.com";
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string firstName)
    {
        var subject = "Ağız ve Diş Sağlığı Takip Uygulamasına Hoş Geldiniz!";
        var body = $@"
            <html>
            <body>
                <h2>Merhaba {firstName}!</h2>
                <p>Ağız ve Diş Sağlığı Takip Uygulamasına hoş geldiniz!</p>
                <p>Sağlıklı ağız ve diş sağlığı alışkanlıklarınızı takip etmek için gerekli tüm araçlara sahipsiniz.</p>
                <p>Uygulamamız ile:</p>
                <ul>
                    <li>Günlük sağlık hedeflerinizi belirleyebilir</li>
                    <li>Aktivitelerinizi takip edebilir</li>
                    <li>Notlarınızı kaydedebilir</li>
                    <li>Sağlık önerilerimizi takip edebilirsiniz</li>
                </ul>
                <p>Sağlıklı günler dileriz!</p>
                <p><strong>Ağız ve Diş Sağlığı Takip Ekibi</strong></p>
            </body>
            </html>";

        await SendEmailAsync(toEmail, subject, body);
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        var subject = "Parola Sıfırlama Talebi";
        var body = $@"
            <html>
            <body>
                <h2>Parola Sıfırlama</h2>
                <p>Parolanızı sıfırlamak için bir talepte bulundunuz.</p>
                <p>Yeni parolanızı belirlemek için aşağıdaki formu kullanın:</p>
                <p>Bu işlemi siz yapmadıysanız, bu e-postayı dikkate almayın.</p>
                <p><strong>Ağız ve Diş Sağlığı Takip Ekibi</strong></p>
            </body>
            </html>";

        await SendEmailAsync(toEmail, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "Ağız ve Diş Sağlığı Takip"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
        catch (Exception ex)
        {
            // Log the exception - in a real application, use proper logging
            Console.WriteLine($"Email sending failed: {ex.Message}");
            throw;
        }
    }
}