using eRaceSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL
{
    public class PositionController
    {
        public List<string> Positions_GetDescriptions()
        {
            using (var context = new eRaceContext())
            {
                var results = (from x in context.Positions
                               select x.Description).Distinct();
                return results.ToList();
            }
        }
    }
}
