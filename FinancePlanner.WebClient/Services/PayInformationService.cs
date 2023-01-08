using FinancePlanner.Shared.Models.FinanceServices;
using FinancePlanner.WebClient.Extensions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinancePlanner.WebClient.Services;

public class PayInformationService : IPayInformationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public PayInformationService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<PayInformationResponse> GetPay(string userId)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:ApiAggregatorClient:ClientName").Value);
        string requestUrl = $"{_config.GetSection("Clients:ApiAggregatorClient:GetPayInformation").Value}/{userId}";
        List<PayInformationResponse> responses = await httpClient.GetList<PayInformationResponse>(requestUrl);
        return responses.First();
    }

    public async Task<PayInformationResponse?> SavePay(PayInformationRequest request)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:ApiAggregatorClient:ClientName").Value);
        string requestUrl = $"{_config.GetSection("Clients:ApiAggregatorClient:SavePayInformation").Value}";
        PayInformationResponse? response = await httpClient.Post<PayInformationRequest, PayInformationResponse>(request, requestUrl);
        return response ?? default;
    }

    public async Task<PayInformationResponse?> UpdatePay(PayInformationRequest request, string userId, int payId, int incomeId)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:ApiAggregatorClient:ClientName").Value);
        string requestUrl = $"{_config.GetSection("Clients:ApiAggregatorClient:UpdatePayInformation").Value}/{userId}/{payId}/{incomeId}";
        PayInformationResponse? response = await httpClient.Post<PayInformationRequest, PayInformationResponse>(request, requestUrl);
        return response ?? default;
    }

    public async Task<bool> DeletePay(string userId, int payId, int incomeId)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:ApiAggregatorClient:ClientName").Value);
        string requestUrl = $"{_config.GetSection("Clients:ApiAggregatorClient:DeletePayInformation").Value}/{userId}/{payId}/{incomeId}";
        bool response = await httpClient.Delete(requestUrl);
        return response;
    }
}