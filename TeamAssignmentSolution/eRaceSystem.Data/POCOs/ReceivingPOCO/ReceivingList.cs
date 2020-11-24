using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.ReceivingPOCO
{
    public class ReceivingList
    {
        public int OrderDetailID { get; set; }
        public string ItemName { get; set; }
        
        public int QuantityOrdered { get; set; }
        public string OrderedUnits { get; set; }
        public int QuantityOutstanding { get; set; }
        public int? ReceivingUnits { get; set; }
        public int UnitSize { get; set; }
        public string UnitType {get; set;}

        private string _string;
        public string ReceivingUnitString
        {
            get
            {
                return _string;
            }
            set
            {
                _string = " x " + UnitType + " of " + UnitSize;
            }
        }

        public int? RejectedUnits { get; set; }
        public string RejectedReason { get; set; }
        public int? SalvagedItems { get; set; }


        //poco read data from database
        //dto read data from web front page
    }
}
