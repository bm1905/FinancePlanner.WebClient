using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FinancePlanner.WebClient.Extensions;
using FinancePlanner.WebClient.Models;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.WebClient.Services
{
    public class PayService : IPayService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public PayService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<PayCheckResponse> GetPayCheck(PayCheckRequest model)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:ApiGatewayClient:ClientName").Value);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _config.GetSection("Clients:ApiGatewayClient:CalculatePay").Value);
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            request.Content = content;
            HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.ReadContentAs<PayCheckResponse>();
        }
    }
}
