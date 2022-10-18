using System.Collections.Generic;

namespace FinancePlanner.WebClient.Models
{
    public class PayCheckResponse
    {
        public List<Response> Responses { get; set; }
        public decimal TotalNetPay { get; set; }
        public decimal TotalGrossPay { get; set; }

        public PayCheckResponse()
        {
            Responses = new List<Response>();
        }
    }

    public class Response
    {
        public decimal GrossPay { get; set; }
        public decimal NetPay { get; set; }
        public decimal PreTaxDeductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal PostTaxDeductions { get; set; }
    }
}
