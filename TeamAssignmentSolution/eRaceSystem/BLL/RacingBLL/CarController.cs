using eRaceSystem.DAL;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL.RacingBLL
{
    [DataObject]
    public class CarController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> avaliableCarList(string level,int raceid)
        {
            using (var context = new eRaceContext())
            {
                List<SelectionList> result = (from x in context.Cars
                                              where x.CarClass.CertificationLevel.Equals(level)&&
                                                 x.State.Equals("Certified") 
                                                //&& !(from y in context.RaceDetails
                                                  //   where y.RaceID == raceid
                                                   //  select y.Car).Contains(x)
                                                select new SelectionList
                                                {
                                                    IDValueField = x.CarID,
                                                    DisplayText = x.SerialNumber + " "+x.CarClass.CarClassName
                                                }
                                              ).ToList();
                return result;
            }
        }
        
    }
}
