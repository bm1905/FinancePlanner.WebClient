using System.Collections.Generic;
using System.Threading.Tasks;
using FinancePlanner.Shared.Models.FinanceServices;

namespace FinancePlanner.WebClient.Services;

public interface IIncomeInformationService
{
    Task<List<IncomeInformationResponse>?> GetIncomeInformation(string userId);
}