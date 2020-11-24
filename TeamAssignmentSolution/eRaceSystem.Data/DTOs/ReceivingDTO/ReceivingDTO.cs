using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
using eRaceSystem.Data.POCOs.ReceivingPOCO;
#endregion

namespace eRaceSystem.Data.DTOs.ReceivingDTO
{
    public class ReceivingDTO
    {
        
        public int OrderID { get; set; }
        public int EmployeeID { get; set; }
        public string Reason { get; set; }
        public List<ReceivingList> ReceivingLists { get; set; }
        public List<UnorderedItemsList> UnorderedItemLists { get; set; }
    }
}


