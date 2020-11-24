using eRaceSystem.DAL;
using eRaceSystem.Data.POCOs.RacingPOCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL.RacingBLL
{
    [DataObject]
    public class RaceController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RaceSchdulePOCO> raceSchduleList(DateTime raceTime)
        {
            using (var context = new eRaceContext())
            {

                List<RaceSchdulePOCO> result = (from x in context.Races
                                                where x.RaceDate.Month == raceTime.Month
                                                    && x.RaceDate.Year == raceTime.Year
                                                    && x.RaceDate.Day == raceTime.Day
                                                select new RaceSchdulePOCO
                                                {
                                                    itemID = x.RaceID,
                                                    raceTime = x.RaceDate,
                                                    competition = x.Certification.Description + "-" + x.Comment,
                                                    run = x.Run,
                                                    playerCount = x.NumberOfCars
                                                }
                                                ).ToList();
                return result;
            }
        }

        public string getCertificationLevel(int raceid)
        {
            using (var context = new eRaceContext())
            {
                string result = (from x in context.Races
                                 where x.RaceID == raceid
                                 select x.Certification.CertificationLevel).FirstOrDefault();
                return result;
            }
        }
    }
}
