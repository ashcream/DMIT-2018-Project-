using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.SalePOCO
{
    public class RefundPOCO
    {
        public string Category { get; set; }
        public int ProductID { get; set; }
        public string Product { get; set; }
        public int Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal RestockChg { get; set; }
        public decimal? Amount { get; set; }
        public string RefundReason { get; set; }
    }
}
