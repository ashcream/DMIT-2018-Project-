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
    public class ProductController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Product> Product_List()
        {
            using (var context = new eRaceContext())
            {
                return context.Products.ToList();
            }
        }
        public Product Product_Get(int Productid)
        {
            using (var context = new eRaceContext())
            {
                return context.Products.Find(Productid);
            }
        }
    }
}
