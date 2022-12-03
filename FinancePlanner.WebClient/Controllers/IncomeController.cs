using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FinancePlanner.Shared.Models.FinanceServices;
using FinancePlanner.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FinancePlanner.WebClient.Controllers;

public class IncomeController : Controller
{
    private readonly IIncomeInformationService _incomeInformation;

    public IncomeController(IIncomeInformationService incomeInformation)
    {
        _incomeInformation = incomeInformation;
    }

    public IActionResult Index()
    {
        string? responses = TempData["IncomeInformation"]?.ToString();
        return responses == null ? View() : View(JsonConvert.DeserializeObject<List<IncomeInformationResponse>>(responses));
    }

    public async Task<IActionResult> GetIncomeInformation()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        List<IncomeInformationResponse>? responses = await _incomeInformation.GetIncomeInformation(userId);
        TempData["IncomeInformation"] = JsonConvert.SerializeObject(responses);
        return RedirectToAction(nameof(Index));
    }
}