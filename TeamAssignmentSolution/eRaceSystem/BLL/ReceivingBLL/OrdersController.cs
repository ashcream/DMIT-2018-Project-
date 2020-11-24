using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
using eRaceSystem.Data.Entities;
using eRaceSystem.DAL;
using System.ComponentModel;
using eRaceSystem.Data.POCOs.ReceivingPOCO;
#endregion

namespace eRaceSystem.BLL.ReceivingBLL
{
    [DataObject]
    public class OrdersController
    {
        #region purchase order list selection
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<POForReceivingList> List_POForReceiving()
        {
            using (var context = new eRaceContext())
            {
                var results = from x in context.Orders
                              where x.Closed.Equals(false) && x.OrderNumber != null && x.OrderDate != null
                              select new POForReceivingList
                              {
                                  OrderID = x.OrderID,
                                  POForReceiving = x.OrderNumber + "-" + x.Vendor.Name                                
                              };
                return results.ToList();
            }
        }
        #endregion

        public int FindOrder_byOrderNumber (int ordernumber)
        {
            using (var context = new eRaceContext())
            {
                return (from x in context.Orders
                        where x.OrderNumber == ordernumber
                        select x.OrderID).FirstOrDefault();
            }
            
        }
    }
}
