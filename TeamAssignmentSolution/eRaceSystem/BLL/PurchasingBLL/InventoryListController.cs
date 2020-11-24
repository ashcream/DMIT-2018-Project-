using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
using eRaceSystem.Data.Entities;
using eRaceSystem.DAL;
using System.ComponentModel;
using eRaceSystem.Data.POCOs.PurchasingPOCO;
using eRaceSystem.Data.DTOs.PurchasingDTO;
#endregion

namespace eRaceSystem.BLL.PurchasingBLL
{
    [DataObject]
    public class InventoryListController
    {
        public List<InventoryListDTO> GetVendorInventory(int vendorid)
        {
            using(var context = new eRaceContext())
            {
                var results = from x in context.Categories
                              select new InventoryListDTO
                              {
                                  Description = x.Description,
                                  ProductList = (from y in context.VendorCatalogs
                                  where y.VendorID == vendorid && y.Product.Category.Description == x.Description 
                                  select new ProductPOCO
                                  {
                                        ProductID = y.ProductID,
                                        ItemName = y.Product.ItemName,
                                        ReOrderLevel = y.Product.ReOrderLevel,
                                        QuantityOnHand = y.Product.QuantityOnHand,
                                        QuantityOnOrder = y.Product.QuantityOnOrder,
                                        UnitSize = y.OrderUnitSize,
                                        UnitCost = y.OrderUnitCost,
                                        Size = y.OrderUnitType + " (" + y.OrderUnitSize + ")"
                                  }).ToList()
                              };

                return results.ToList();
            }
        }
    }
}
