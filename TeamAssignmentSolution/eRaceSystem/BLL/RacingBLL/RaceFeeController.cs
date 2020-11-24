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
    public class RaceFeeController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> GetRaceFee()
        {
            using (var context = new eRaceContext())
            {
                return (from x in context.RaceFees
                        select new SelectionList
                        {
                            IDValueField = x.RaceFeeID,
                            DisplayText = x.Fee.ToString()
                        }).Distinct().ToList();
            }
        }
    }
}
