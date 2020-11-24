using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using eRaceSystem.DAL;
using eRaceSystem.Data.POCOs;
using eRaceSystem.Data.Entities;

namespace eRaceSystem.BLL
{
    [DataObject]
    public class CategoryController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Category> Category_List()
        {
            using (var context = new eRaceContext())
            {
                return context.Categories.ToList();
            }
        }
        public Category Category_Get(int Categoryid)
        {
            using (var context = new eRaceContext())
            {
                return context.Categories.Find(Categoryid);
            }
        }

    }
}
