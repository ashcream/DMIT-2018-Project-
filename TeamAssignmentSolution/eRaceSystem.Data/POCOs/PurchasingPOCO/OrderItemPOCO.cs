using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.PurchasingPOCO
{
    public class OrderItemPOCO
    {
        public int? OrderDetailID { get; set; }
        public string Product { get; set; }

        public int OrderQty { get; set; }
        public int UnitSize { get; set; }
        public decimal UnitCost { get; set; }
    }
}
