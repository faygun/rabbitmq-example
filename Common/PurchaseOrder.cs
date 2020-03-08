using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class PurchaseOrder
    {
        public decimal AmountToPay { get; set; }
        public string PoNumber { get; set; }
        public string CompanyName { get; set; }
        public string PaymentDayTerms { get; set; }
    }
}
