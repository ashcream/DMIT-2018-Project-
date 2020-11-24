
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region
using eRaceSystem.Data.POCOs.SalePOCO;
#endregion


namespace eRaceSystem.Data.DTOs.SalesDTO
{
    public class SalesItemDTO
    {
        public int OrderID { get; set; }
        
        public int InvoiceID { get; set; }
       
        public DateTime InvoiceData { get; set; }
        public int EmployeeID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemsPOCO> SaleItemsList { get; set; }

    }
}
