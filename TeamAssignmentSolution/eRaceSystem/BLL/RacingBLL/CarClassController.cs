using eRaceSystem.DAL;
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
    public class CarClassController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> GetCarClassByCertification(string certificationLevel)
        {
            using (var context = new eRaceContext())
            {
                List<SelectionList> results = (from x in context.CarClasses
                                               where x.CertificationLevel.Equals(certificationLevel)
                                               select new SelectionList
                                               {
                                                   IDValueField = x.CarClassID,
                                                   DisplayText = x.CarClassName
                                               }).Distinct().ToList();

                return results;
            }
        }
    }
}
