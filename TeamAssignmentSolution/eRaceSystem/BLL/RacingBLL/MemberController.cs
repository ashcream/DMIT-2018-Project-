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
    public class MemberController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectionList> GetMembersByLevel(string level)
        {
            using (var context =new eRaceContext())
            {
                level = string.IsNullOrEmpty(level) ? "" : level;
                List<SelectionList> results = new List<SelectionList>() ;
                if (level.Equals("D"))
                {
                    results = (from x in context.Members
                               where x.CertificationLevel.Equals(level)
                               select new SelectionList
                               {
                                   IDValueField = x.MemberID,
                                   DisplayText = x.FirstName + " " + x.LastName
                               }
                        ).ToList();
                }
                else if (level.Equals("C"))
                {
                    results = (from x in context.Members
                               where !x.CertificationLevel.Equals("D") 
                               select new SelectionList
                               {
                                   IDValueField = x.MemberID,
                                   DisplayText = x.FirstName + " " + x.LastName
                               }).ToList();
                }
                else if (level.Equals("B"))
                {
                    results = (from x in context.Members
                               where x.CertificationLevel.Equals("B") || x.CertificationLevel.Equals("A")
                               select new SelectionList
                               {
                                   IDValueField=x.MemberID,
                                   DisplayText = x.FirstName + " " + x.LastName
                               }).ToList();
                }
                else if (level.Equals("A"))                
                {
                    results = (from x in context.Members
                               where x.CertificationLevel.Equals("A")
                               select new SelectionList
                               {
                                   IDValueField = x.MemberID,
                                   DisplayText = x.FirstName + " " + x.LastName
                               }).ToList();
                }
                return results;
                
            }
        }
    }
}
