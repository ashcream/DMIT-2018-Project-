using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespace
using eRaceSystem.Data.Entities;
using eRaceSystem.DAL;
#endregion

namespace eRaceSystem.BLL.ReceivingBLL
{
    [DataObject]
    public class VendorsController
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Vendor GetVendorInfo_ByOrderID(int orderid)
        {
            using (var context = new eRaceContext())
            {
                Order order = context.Orders.Find(orderid);
                return order.Vendor;
            }
        }
    }
}
