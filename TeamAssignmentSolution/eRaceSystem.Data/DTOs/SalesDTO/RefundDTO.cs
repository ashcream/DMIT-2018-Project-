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
    public class RefundDTO
    {   

        public DateTime InvoiceDate { get; set; }
        public int EmployeeID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
        public int OriginalInvoiceID { get; set; }




        public List<RefundPOCO> RefundList { get; set; }
    }
}
