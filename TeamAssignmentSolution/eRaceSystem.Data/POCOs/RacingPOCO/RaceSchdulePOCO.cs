using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs.RacingPOCO
{
    public class RaceSchdulePOCO
    {
        public int itemID { get; set;}
        public DateTime raceTime { get; set; }
        public string competition { get; set; }
        public string run { get; set; }
        public int playerCount { get; set; }

        
    }
}
