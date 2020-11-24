using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.RacingPOCO
{
    
    public class RaceDetailPOCO
    {
        public int RaceDetailID { get; set; }
        
        public string Name { get; set; }
        public decimal RaceFee { get; set; }
        public decimal RentalFee { get; set; }
        public int? Placement { get; set; }
        public bool Refunded { get; set; }
        public string RefundReason { get; set; }
        public string CarClass { get; set; }
        public int? CarserialNumber { get; set; }
        public string Comment { get; set; }
        
    }
}
