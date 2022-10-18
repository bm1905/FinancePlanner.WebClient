using System.Collections.Generic;
using Shared.Models.Enums;
using Shared.Models.WageServices;

namespace FinancePlanner.WebClient.Models
{
    public class PayCheckRequest
    {
        public List<Company> Companies { get; set; }

        public PayCheckRequest()
        {
            Companies = new List<Company>();
        }
    }

    public class Company
    {
        public PreTaxWagesRequest PreTaxWagesRequest { get; set; }
        public string W4Type { get; set; }
        public TaxFilingStatus TaxFilingStatus { get; set; }
        public int PayPeriodNumber { get; set; }
        public string State { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public PostTaxDeduction PostTaxDeductionRequest { get; set; }
    }
}
