using System.Threading.Tasks;
using FinancePlanner.WebClient.Models;

namespace FinancePlanner.WebClient.Services
{
    public interface IPayService
    {
        Task<PayCheckResponse> GetPayCheck(PayCheckRequest request);
    }
}
