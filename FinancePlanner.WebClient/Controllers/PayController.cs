using FinancePlanner.Shared.Models.FinanceServices;
using FinancePlanner.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinancePlanner.WebClient.Controllers;

public class PayController : Controller
{
    private readonly IPayInformationService _payInformationService;

    public PayController(IPayInformationService payInformationService)
    {
        _payInformationService = payInformationService;
    }

    public async Task<IActionResult> PayIndex()
    {
        PayInformationResponse response = await _payInformationService.GetPay("test123");
        return View(response);
    }

    public async Task<IActionResult> EditPayInformation(PayInformationRequest request, int payId, int incomeId)
    {
        string userId = "test123";
        //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PayInformationResponse? response = await _payInformationService.UpdatePay(request, userId, payId, incomeId);
        if (response != null)
        {
            return RedirectToAction("GetIncomeInformation", "Income");
        }

        return View(request);
    }
}
