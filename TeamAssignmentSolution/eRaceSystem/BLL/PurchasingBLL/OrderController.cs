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
    public class OrderController
    {
        public ProductPOCO GetProductPOCO(int productid, int vendorid)
        {
            using (var context = new eRaceContext())
            {
                var exist = (from x in context.VendorCatalogs
                             where x.VendorID == vendorid && x.ProductID == productid
                             select new ProductPOCO
                             {
                                 ProductID = x.ProductID,
                                 ItemName = x.Product.ItemName,
                                 ReOrderLevel = x.Product.ReOrderLevel,
                                 QuantityOnHand = x.Product.QuantityOnHand,
                                 QuantityOnOrder = x.Product.QuantityOnOrder,
                                 UnitSize = x.OrderUnitSize,
                                 UnitCost = x.OrderUnitCost,
                                 Size = x.OrderUnitType + " (" + x.OrderUnitSize + ")"
                             }).FirstOrDefault();
                return exist;
            }
        }

        public int? PlaceOrder(OrderLogDTO newOrderInfo)
        {
            using (var context = new eRaceContext())
            {
                var ifexist = (from x in context.Orders
                              where x.VendorID == newOrderInfo.VendorID && x.OrderNumber == null
                              select x).FirstOrDefault();

                if (ifexist != null)
                {
                    var existOrderDetial = (from x in context.OrderDetails
                                            where x.OrderID == ifexist.OrderID
                                            select x).ToList();
                    if(existOrderDetial !=null)
                    {
                        foreach(var item in existOrderDetial)
                        {
                            context.OrderDetails.Remove(item);
                        }
                    }

                    context.Orders.Remove(ifexist);
                }
                
                Order newOrder = new Order();

                var exists = (from x in context.Orders
                              orderby x.OrderNumber descending
                              select x).FirstOrDefault();
                newOrder.OrderNumber = exists.OrderNumber + 1;
                newOrder.OrderDate = DateTime.Now;
                newOrder.EmployeeID = newOrderInfo.EmployeeID;
                newOrder.TaxGST = newOrderInfo.TaxGST;
                newOrder.SubTotal = newOrderInfo.SubTotal;
                newOrder.VendorID = newOrderInfo.VendorID;
                newOrder.Closed = false;
                newOrder.Comment = newOrderInfo.Comment;
                context.Orders.Add(newOrder);

                foreach(var item in newOrderInfo.ItemList)
                {
                    OrderDetail newOrderDetail = new OrderDetail();
                    var exist = (from x in context.Orders
                                  orderby x.OrderID descending
                                  select x).FirstOrDefault();
                    newOrderDetail.OrderID = exist.OrderID + 1;
                    var selectedproduct = (from x in context.Products
                                           where x.ItemName == item.Product
                                           select x).FirstOrDefault();
                    newOrderDetail.ProductID = selectedproduct.ProductID;
                    newOrderDetail.Quantity = item.OrderQty;
                    newOrderDetail.OrderUnitSize = item.UnitSize;
                    newOrderDetail.Cost = item.UnitCost;
                    selectedproduct.QuantityOnOrder += item.UnitSize * item.OrderQty;
                    context.Entry(selectedproduct).Property(y => y.QuantityOnOrder).IsModified = true;
                    context.OrderDetails.Add(newOrderDetail);
                }
                context.SaveChanges();
                return newOrder.OrderNumber;
            }
        }


        public OrderLogDTO GetVendorOrderLog(int vendorid, int employeeid)
        {
            using (var context = new eRaceContext())
            {
                var exists = (from x in context.Orders
                             where x.VendorID == vendorid && x.OrderNumber == null
                             select x).FirstOrDefault();
                var exist = new OrderLogDTO();
                if (exists == null)
                {
                    Order newOrder = new Order();
                    newOrder.OrderNumber = null;
                    newOrder.OrderDate = null;
                    newOrder.EmployeeID = employeeid;
                    newOrder.TaxGST = 0;
                    newOrder.SubTotal = 0;
                    newOrder.VendorID = vendorid;
                    newOrder.Closed = false;
                    newOrder.Comment = null;
                    context.Orders.Add(newOrder);
                    context.SaveChanges();
                }
                exist = (from x in context.Orders
                                where x.VendorID == vendorid && x.OrderNumber == null
                                select new OrderLogDTO
                                {
                                    OrderID = x.OrderID,
                                    OrderNumber = x.OrderNumber,
                                    OrderDate = x.OrderDate,
                                    EmployeeID = x.EmployeeID,
                                    TaxGST = x.TaxGST,
                                    SubTotal = x.SubTotal,
                                    VendorID = x.VendorID,
                                    Closed = x.Closed,
                                    Comment = x.Comment,
                                    ItemList = (from y in x.OrderDetails
                                                select new OrderItemPOCO
                                                {
                                                    OrderDetailID = y.OrderDetailID,
                                                    Product = y.Product.ItemName,
                                                    OrderQty = y.Quantity,
                                                    UnitSize = y.OrderUnitSize,
                                                    UnitCost = y.Cost
                                                }).ToList()
                                }).FirstOrDefault();
                return exist;
            }
        }

        public void RemoveOrder(int vendorid)
        {
            using(var context = new eRaceContext())
            {
                var orderDetails = from x in context.OrderDetails
                             where x.Order.VendorID == vendorid && x.Order.OrderNumber == null
                             select x;
                foreach(var item in orderDetails)
                {
                    context.OrderDetails.Remove(item);
                }
                var order = (from x in context.Orders
                         where x.VendorID == vendorid && x.OrderNumber == null
                         select x).FirstOrDefault();
                if(order == null)
                {
                    throw new Exception("There is nothing to delete, select the vendor to create a new pending order");
                }
                else
                {
                    context.Orders.Remove(order);
                }

                context.SaveChanges();
            }
        }
        public void UpdateOrder(int vendorid, OrderLogDTO itemDTO)
        {
            using (var context = new eRaceContext())
            {
                var orderDetails = from x in context.OrderDetails
                                   where x.Order.VendorID == vendorid && x.Order.OrderNumber == null
                                   select x;
                foreach (var item in orderDetails)
                {
                    context.OrderDetails.Remove(item);
                }
                var order = (from x in context.Orders
                             where x.VendorID == vendorid && x.OrderNumber == null
                             select x).FirstOrDefault();
                order.Comment = itemDTO.Comment;
                order.SubTotal = itemDTO.SubTotal;
                order.TaxGST = itemDTO.SubTotal * (decimal)0.05;

                foreach (var item in itemDTO.ItemList)
                {
                    OrderDetail orderDetailToAdd = new OrderDetail();
                    orderDetailToAdd.OrderID = order.OrderID;
                    var selectProduct = (from x in context.Products
                                        where x.ItemName == item.Product
                                        select x.ProductID).FirstOrDefault();
                    orderDetailToAdd.ProductID = selectProduct;
                    orderDetailToAdd.Quantity = item.OrderQty;
                    orderDetailToAdd.OrderUnitSize = item.UnitSize;
                    orderDetailToAdd.Cost = item.UnitCost;
                    context.OrderDetails.Add(orderDetailToAdd);
                }
                context.Entry(order).Property(y => y.Comment).IsModified = true;
                context.Entry(order).Property(y => y.SubTotal).IsModified = true;
                context.Entry(order).Property(y => y.TaxGST).IsModified = true;
                context.SaveChanges();

            }
        }




    }
}
