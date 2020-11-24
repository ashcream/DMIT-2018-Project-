using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.RacingPOCO
{
    public class RaceResultsPOCO
    {
        public int RaceDetailID { get; set; }
        public  string Name { get; set; }
        public int? PenaltyID { get; set; }

        public int? Placement { get; set; }

        private TimeSpan? _RunTime;
        public TimeSpan? RunTime
        {
            get
            {
                return _RunTime;
            }
            set
            {
                _RunTime = (TimeSpan?)value;
            }
        }



    }
}
