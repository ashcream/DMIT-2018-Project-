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
using eRaceSystem.Data.DTOs.ReceivingDTO;
using eRaceSystem.BLL.EntitiesBLL;

#endregion

namespace eRaceSystem.BLL.ReceivingBLL
{
    [DataObject]
    public class ReceivingController
    {

        private List<string> reasons = new List<string>();
        #region ReceivingListForPurchaseOrderSelection
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ReceivingList> List_ReceivingListForPurchaseOrderSelection(int orderid)
        {
            using (var context = new eRaceContext())
            {
                var results = from x in context.OrderDetails
                              where x.OrderID.Equals(orderid)
                              select new ReceivingList
                              {
                                  OrderDetailID = x.OrderDetailID,

                                  ItemName = x.Product.ItemName,

                                  QuantityOrdered = x.Quantity * x.OrderUnitSize,
                                  OrderedUnits = x.Quantity + " x case of " + x.OrderUnitSize,
                                  QuantityOutstanding = x.Product.QuantityOnOrder,
                                  ReceivingUnits = 0,
                                  UnitSize = x.OrderUnitSize,
                                  UnitType = (from y in x.Product.VendorCatalogs
                                              select y.OrderUnitType).FirstOrDefault(),
                                  ReceivingUnitString = (from y in x.Product.VendorCatalogs
                                                         select y.OrderUnitType).FirstOrDefault() + " of " + x.OrderUnitSize,
                                  RejectedUnits = 0,
                                  RejectedReason = "",
                                  SalvagedItems = 0
                              };
                return results.ToList();
            }
        }
        #endregion



        public void ReceivingOrder_Transaction(ReceivingDTO receiveorderlist)
        {
            using (var context = new eRaceContext())
            {
                var closeResult = (from x in context.Orders
                                   where x.OrderID == receiveorderlist.OrderID
                                   select x.Closed).FirstOrDefault();
                if (closeResult is true)
                {
                   reasons.Add("This order has already been forced closed.");
                    //Ensure that the order has not been previously closed.
                }
                else
                {
                    //Create ReceiveOrders
                    ReceiveOrder newReceiveOrder = new ReceiveOrder();  //Create new receiveOrder entry                                       
                    newReceiveOrder.OrderID = receiveorderlist.OrderID;
                    newReceiveOrder.ReceiveDate = DateTime.Now;
                    newReceiveOrder.EmployeeID = receiveorderlist.EmployeeID;

                    context.ReceiveOrders.Add(newReceiveOrder);
                    //refresh template newReceiveOrder

                    context.SaveChanges();

                    //Create ReceiveOrderItems
                    int ReceiveOrderID = newReceiveOrder.ReceiveOrderID;  //auto generate               


                    #region receiveOrderItem
                    for (int i = 0; i < receiveorderlist.ReceivingLists.Count(); i++)
                    {
                        if (receiveorderlist.ReceivingLists[i].ReceivingUnits != 0)
                        {

                            ReceiveOrderItem newReceiveItem = new ReceiveOrderItem();
                            newReceiveItem.ReceiveOrderID = ReceiveOrderID;
                            newReceiveItem.OrderDetailID = receiveorderlist.ReceivingLists[i].OrderDetailID;
                            int UnitSize = (from x in context.OrderDetails
                                            where x.OrderDetailID == newReceiveItem.OrderDetailID
                                            select x.OrderUnitSize).FirstOrDefault();
                            
                            newReceiveItem.ItemQuantity = ((UnitSize) * (receiveorderlist.ReceivingLists[i].ReceivingUnits) + receiveorderlist.ReceivingLists[i].SalvagedItems).GetValueOrDefault();
                          
                            

                            context.ReceiveOrderItems.Add(newReceiveItem);


                            ReceivingList tempItem = receiveorderlist.ReceivingLists[i];
                            //query cant figure out i index

                            Product exists = (from x in context.Products
                                              where x.ItemName.Equals(tempItem.ItemName, StringComparison.OrdinalIgnoreCase)
                                              select x).FirstOrDefault();
                            if (exists == null)
                            {
                                reasons.Add("Product does not exist.");
                            }
                            else if (receiveorderlist.ReceivingLists[i].ReceivingUnits * UnitSize - receiveorderlist.ReceivingLists[i].QuantityOutstanding <= 0)
                            {
                                exists.QuantityOnOrder -= newReceiveItem.ItemQuantity;
                                exists.QuantityOnHand += newReceiveItem.ItemQuantity;
                                context.Entry(exists).Property(y => y.QuantityOnOrder).IsModified = true;
                                context.Entry(exists).Property(y => y.QuantityOnHand).IsModified = true;
                                newReceiveItem = new ReceiveOrderItem();
                            }
                            else if (receiveorderlist.ReceivingLists[i].ReceivingUnits * UnitSize - receiveorderlist.ReceivingLists[i].QuantityOutstanding < UnitSize)
                            {
                                exists.QuantityOnOrder = 0;
                                exists.QuantityOnHand += newReceiveItem.ItemQuantity;
                                context.Entry(exists).Property(y => y.QuantityOnOrder).IsModified = true;
                                context.Entry(exists).Property(y => y.QuantityOnHand).IsModified = true;
                                newReceiveItem = new ReceiveOrderItem();
                            }
                            else
                            {
                                reasons.Add("The overage received for does not exceed the order unit size on the original order");
                            } 
                        }
                    }
                    #endregion
                   
                    #region Create ReturnOrderItem
                    ReturnOrderItem newReturnItem = new ReturnOrderItem();
                    for (int i = 0; i < receiveorderlist.ReceivingLists.Count(); i++)
                    {
                        if (receiveorderlist.ReceivingLists[i].RejectedUnits != 0) //for rejected items
                        {
                            newReturnItem.UnOrderedItem = receiveorderlist.ReceivingLists[i].ItemName;
                            newReturnItem.ReceiveOrderID = ReceiveOrderID;
                            newReturnItem.OrderDetailID = receiveorderlist.ReceivingLists[i].OrderDetailID;
                            newReturnItem.Comment = receiveorderlist.ReceivingLists[i].RejectedReason;
                            int UnitSize = (from x in context.OrderDetails
                                            where x.OrderDetailID == newReturnItem.OrderDetailID
                                            select x.OrderUnitSize).FirstOrDefault();
                            newReturnItem.ItemQuantity += ((UnitSize) * receiveorderlist.ReceivingLists[i].RejectedUnits - receiveorderlist.ReceivingLists[i].SalvagedItems).GetValueOrDefault();

                            context.ReturnOrderItems.Add(newReturnItem);
                            newReturnItem = new ReturnOrderItem();
                        }  
                    }
                    #endregion

                    #region unordereditem
                    if (receiveorderlist.UnorderedItemLists.Count != 0)
                    {
                        for (int i = 0; i < receiveorderlist.UnorderedItemLists.Count(); i++)
                        {
                                UnorderedItemsList tempReturnItem = receiveorderlist.UnorderedItemLists[i];
                                ReturnOrderItem exists = (from x in context.ReturnOrderItems
                                                          where x.UnOrderedItem.Equals(tempReturnItem.ItemName, StringComparison.OrdinalIgnoreCase)
                                                          select x).FirstOrDefault();
                                if (exists == null)
                                {
                                    newReturnItem.ReceiveOrderID = ReceiveOrderID;
                                    newReturnItem.UnOrderedItem = receiveorderlist.UnorderedItemLists[i].ItemName;
                                    newReturnItem.VendorProductID = receiveorderlist.UnorderedItemLists[i].VendorProductID;
                                    newReturnItem.ItemQuantity = receiveorderlist.UnorderedItemLists[i].UnorderedItemQuantity;
                                    context.ReturnOrderItems.Add(newReturnItem);

                                    
                                }
                                else
                                {
                                    exists.VendorProductID = receiveorderlist.UnorderedItemLists[i].VendorProductID;
                                    exists.ItemQuantity += receiveorderlist.UnorderedItemLists[i].UnorderedItemQuantity;
                                    context.Entry(exists).Property(y => y.VendorProductID).IsModified = true;
                                    context.Entry(exists).Property(y => y.ItemQuantity).IsModified = true;
                                }
                           
                            //clean the unorderitem 

                        }
                    }
                    #endregion
                    


                    context.SaveChanges();
                }
            }
        }


        #region UnorderedItems_Select
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnOrderedItem> UnorderedItemsList_Select()
        {
            using (var context = new eRaceContext())
            {

                return context.UnOrderedItems.ToList();
            }
        }
        #endregion

        #region UnorderedItems_Add
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int UnorderedItems_Add(UnOrderedItem item)
        {
            using (var context = new eRaceContext())
            {
                if (item.ItemID.ToString() == null)
                {
                    reasons.Add("The information of unordered item was not completely filled in. Please fill in all the information first.");
                }
                else if (string.IsNullOrEmpty(item.ItemName))
                {
                    reasons.Add("The information of unordered item was not completely filled in. Please fill in the item name.");
                }
                else if (string.IsNullOrEmpty(item.VendorProductID))
                {
                    reasons.Add("The information of unordered item was not completely filled in. Please fill in the Vendor Product ID.");
                }
                else if (string.IsNullOrEmpty(item.Quantity.ToString()))
                {
                    reasons.Add("The information of unordered item was not completely filled in. Please fill in the quantity.");
                }
                else
                {

                    context.UnOrderedItems.Add(item);

                }
                
                return context.SaveChanges();
            }
        }
        #endregion

        #region UnorderedItems_Delete
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int UnorderedItems_Delete(UnOrderedItem item)
        {
            return UnorderedItems_Delete(item.ItemID);
        }
        public int UnorderedItems_Delete(int itemid)
        {
            using (var context = new eRaceContext())
            {

                var existing = context.UnOrderedItems.Find(itemid);
                if (existing == null)
                {
                    throw new Exception("Item not on file. Delete unnecessary.");
                }
                else
                {
                    context.UnOrderedItems.Attach(existing);
                    context.UnOrderedItems.Remove(existing);
                    return context.SaveChanges();
                }
            }
        }

        #endregion


        public void ForceClose(ReceivingDTO receiveorderlist, int orderid)
        {
            using (var context = new eRaceContext())
            {
                /*ReceiveOrder newReceiveOrder = new ReceiveOrder(); */ //Create new receiveOrder entry    
                Order exists = (from x in context.Orders
                                where x.OrderID == orderid
                                select x).FirstOrDefault();

                if (exists == null)
                {
                    throw new Exception("Purchase order does not exist.");
                }
                else
                {
                    exists.Closed = true;
                    exists.Comment = receiveorderlist.Reason;
                    context.Entry(exists).Property(y => y.Closed).IsModified = true;
                    context.Entry(exists).Property(y => y.Comment).IsModified = true;
                }
                context.SaveChanges();

            }
        }
    }
}


        //Items not on the original PO are simply identified and returned.

        //no insert unorder item entry validation

        //After either Receiving or Force Close, refresh the display so a new order can be selected from the outstanding list of orders

        //When a PO is selected for receiving, Clear the contents of the UnorderedItems database table.
      
                           
       
    