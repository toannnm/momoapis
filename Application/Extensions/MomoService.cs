using Application.Interfaces.IExtensionServices;
using Application.Models.MomoModels;
using Application.Models.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Application.Extensions
{
    public class MomoService : IMomoService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MomoSection _momoSection;
        public MomoService(IHttpClientFactory httpClientFactory, IOptions<MomoSection> momosection)
        => (_httpClientFactory, _momoSection) = (httpClientFactory, momosection.Value);

        public async Task<(bool, MomoResponse?)> IntializePayment(string endpoint, MomoModel request)
        {
            var client = _httpClientFactory.CreateClient(_momoSection.ServiceName);
            var requestData = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8,
                _momoSection.MediaType);

            var createPaymentLinkRes = await client.PostAsync(endpoint, requestContent);

            if (createPaymentLinkRes.IsSuccessStatusCode)
            {
                var responseContent = await createPaymentLinkRes.Content.ReadAsStringAsync();
                var responseData = JsonConvert
                    .DeserializeObject<MomoResponse>(responseContent);
                if (responseData is not null && responseData.resultCode == "0")
                {
                    return (true, responseData);
                }
                else
                {
                    return (false, responseData);
                }
            }
            else
            {
                return (false, null);
            }
        }
        public async Task<(bool, QueryTransactionResponse?)> QueryTransaction(string endpoint, QueryTransactionModel request)
        {
            var client = _httpClientFactory.CreateClient(_momoSection.ServiceName);
            var requestData = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8,
                _momoSection.MediaType);

            var createPaymentLinkRes = await client.PostAsync(endpoint, requestContent);

            if (createPaymentLinkRes.IsSuccessStatusCode)
            {
                var responseContent = await createPaymentLinkRes.Content.ReadAsStringAsync();
                var responseData = JsonConvert
                    .DeserializeObject<QueryTransactionResponse>(responseContent);
                if (responseData is not null)
                {
                    return (true, responseData);
                }
                else
                {
                    return (false, responseData);
                }
            }
            else
            {
                return (false, null);
            }
        }

        public async Task<(bool, RefundResponse?)> Refund(string endpoint, RefundModel request)
        {
            var client = _httpClientFactory.CreateClient(_momoSection.ServiceName);
            var requestData = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8,
                _momoSection.MediaType);

            var createPaymentLinkRes = await client.PostAsync(endpoint, requestContent);

            if (createPaymentLinkRes.IsSuccessStatusCode)
            {
                var responseContent = await createPaymentLinkRes.Content.ReadAsStringAsync();
                var responseData = JsonConvert
                    .DeserializeObject<RefundResponse>(responseContent);
                if (responseData is not null)
                {
                    return (true, responseData);
                }
                else
                {
                    return (false, responseData);
                }
            }
            else
            {
                return (false, null);
            }
        }
    }
}

