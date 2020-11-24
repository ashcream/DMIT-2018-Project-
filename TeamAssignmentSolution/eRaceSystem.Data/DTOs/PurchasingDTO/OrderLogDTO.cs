using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eRaceSystem.Data.POCOs.PurchasingPOCO;

namespace eRaceSystem.Data.DTOs.PurchasingDTO
{
    public class OrderLogDTO
    {
        public int OrderID { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int EmployeeID { get; set; }
        public decimal TaxGST { get; set; }
        public decimal SubTotal { get; set; }
        public int VendorID { get; set; }
        public bool Closed { get; set; }
        public string Comment { get; set; }
        public List<OrderItemPOCO> ItemList { get; set; }

    }
}
