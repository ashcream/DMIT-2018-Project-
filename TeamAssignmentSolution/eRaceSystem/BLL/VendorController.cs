using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
using System.ComponentModel;
using eRaceSystem.Data.Entities;
using eRaceSystem.DAL;
#endregion

namespace eRaceSystem.BLL
{ 
    [DataObject]
    public class VendorController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Vendor> Vendor_List()
        {
            using (var context = new eRaceContext())
            {
                return context.Vendors.ToList();
            }
        }
        public Vendor Vendor_Get(int Vendorid)
        {
            using (var context = new eRaceContext())
            {
                return context.Vendors.Find(Vendorid);
            }
        }
    }
}
