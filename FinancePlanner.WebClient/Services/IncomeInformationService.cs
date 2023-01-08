using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FinancePlanner.Shared.Models.FinanceServices;
using Microsoft.Extensions.Configuration;
using FinancePlanner.WebClient.Extensions;

namespace FinancePlanner.WebClient.Services;

public class IncomeInformationService : IIncomeInformationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public IncomeInformationService(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    public async Task<List<IncomeInformationResponse>?> GetIncomeInformation(string userId)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(_config.GetSection("Clients:ApiAggregatorClient:ClientName").Value);
        string requestUrl = $"{_config.GetSection("Clients:ApiAggregatorClient:GetIncomeInformation").Value}/{userId}";
        List<IncomeInformationResponse> responses = await httpClient.GetList<IncomeInformationResponse>(requestUrl);
        return responses;
    }
}