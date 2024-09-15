using Application.Interfaces.IExtensionServices;
using Application.Models.Settings;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Text;

namespace Application.Extensions
{
    public class EmailService : IEmailService
    {
        private readonly MailSection _emailConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmailService(IOptions<MailSection> emailsection, IHttpClientFactory httpClientFactory)
            => (_emailConfig, _httpClientFactory) = (emailsection.Value, httpClientFactory);

        public async Task SendEmail(User user, Order order)
        {
            var data = new
            {
                service_id = _emailConfig.ServiceId,
                template_id = _emailConfig.TemplateId,
                user_id = _emailConfig.UserId,
                accessToken = _emailConfig.AccessToken,
                template_params = new
                {
                    Fullname = user.FullName,
                    Phone = user.Phone,
                    Title = _emailConfig.Title,
                    Address = user.Address,
                    UserName = user.Username,
                    TotalPrice = order.TotalPrice,
                    PaymentStatus = order.PaymentStatus.ToString(),
                    To = user.Email,
                    Sign = _emailConfig.Sign
                }
            };

            var httpClient = _httpClientFactory.CreateClient(_emailConfig.ServiceName);

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, _emailConfig.MediaType);

            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsync(_emailConfig.BaseUrlSend, content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Oops... {e.Message}");
            }
        }
    }
}
