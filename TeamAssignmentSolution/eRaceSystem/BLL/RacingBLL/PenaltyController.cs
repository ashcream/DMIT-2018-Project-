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
    public class PenaltyController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> PenaltySelection() {
            using (var context = new eRaceContext())
            {
                return (from x in context.RacePenalties
                        select new SelectionList
                        {
                            IDValueField = x.PenaltyID,
                            DisplayText = x.Description
                        }).ToList();
            }
        }
    }
}
