using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.SalePOCO
{
    public class SaleItemsPOCO
    {
        public int ProductID { get; set; }
        /*item name*/
        public string Product { get; set; }
        public int Quantity { get; set; }
        /*ItemPrice*/
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }  
  
     
    }
}
