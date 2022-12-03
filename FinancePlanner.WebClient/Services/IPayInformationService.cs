using FinancePlanner.Shared.Models.FinanceServices;
using System.Threading.Tasks;

namespace FinancePlanner.WebClient.Services;

public interface IPayInformationService
{
    Task<PayInformationResponse> GetPay(string userId);
    Task<PayInformationResponse?> SavePay(PayInformationRequest request);
    Task<PayInformationResponse?> UpdatePay(PayInformationRequest request, string userId, int payId, int incomeId);
    Task<bool> DeletePay(string userId, int payId, int incomeId);
}