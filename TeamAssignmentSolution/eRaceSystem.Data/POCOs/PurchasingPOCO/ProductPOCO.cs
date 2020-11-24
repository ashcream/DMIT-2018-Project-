using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace eRaceSystem.Data.POCOs.PurchasingPOCO
{
    public class ProductPOCO
    {
        public int ProductID { get; set; }
        public string ItemName { get; set; }

        public int ReOrderLevel { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public int UnitSize { get; set; }
        public decimal UnitCost { get; set; }
        public string Size { get; set; }

    }
}
