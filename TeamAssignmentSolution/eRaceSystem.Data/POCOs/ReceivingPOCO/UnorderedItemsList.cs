using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.ReceivingPOCO
{
    public class UnorderedItemsList
    {
        
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string VendorProductID { get; set; }
        public int UnorderedItemQuantity { get; set; }
    }
}
