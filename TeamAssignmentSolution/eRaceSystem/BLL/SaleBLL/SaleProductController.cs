using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespaces
using eRaceSystem.Data.Entities;
using eRaceSystem.DAL;
using System.ComponentModel;
using eRaceSystem.Data.POCOs.SalePOCO;
using eRaceSystem.Data.DTOs.SalesDTO;
using eRaceSystem.BLL.EntitiesBLL;
#endregion

namespace eRaceSystem.BLL.SaleBLL
{
    [DataObject]
    public class SaleProductController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Product> ProductForSale_List()
        {
            using (var context = new eRaceContext())
            {
                return context.Products.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Product> Product_FindByCategory(int categoryid)
        {
            using (var context = new eRaceContext())
            {
                var results = from x in context.Products
                              where x.CategoryID == categoryid
                              select x;

                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Category> CategoryName()
        {
            using (var context = new eRaceContext())
            {
                return context.Categories.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SaleItemsPOCO> List_ItemsNames( int id)
        {
            using (var context = new eRaceContext())
            {
                var results = from x in context.SalesCartItems
                              where x.EmployeeID == id
                              select new SaleItemsPOCO
                              {
                                  Product = x.Product.ItemName,
                                  Quantity = x.Quantity,
                                  Price = x.Product.ItemPrice,
                                  TotalAmount = x.Product.ItemPrice * x.Quantity
                              };
                return results.ToList();
            }
        }
        public void Clear_SaleItemslist(int id)
        {
            using (var context = new eRaceContext())
            {
                List<string> reasons = new List<string>();

                var exists = (from x in context.SalesCartItems
                                        where x.EmployeeID.Equals(id)
                                        select x).ToList();
                if (exists==null)
                {
                    reasons.Add("There are no products exists");
                    
                }
                else
                {
                   foreach(var item in exists)
                    {
                        context.SalesCartItems.Remove(item);
                    }
                }
                context.SaveChanges();
            }
        }
        public void Clear_SaleItemsforonerow(int id, string productname)
        {
            using (var context = new eRaceContext())
            {
               
                var exists = (from x in context.SalesCartItems
                              where x.EmployeeID.Equals(id)
                              && x.Product.ItemName== productname
                              select x).FirstOrDefault();
              
               context.SalesCartItems.Remove(exists);
                context.SaveChanges();
            }
        }
        public void Update_SaleItemAmount(int id, List<SaleItemsPOCO> SaleItemList)
        {
            using(var context = new eRaceContext())
            {
                var exists = (from x in context.SalesCartItems
                              where x.EmployeeID.Equals(id)
                              select x).ToList();

                foreach (var items in exists)
                {
                    context.SalesCartItems.Remove(items);

                }

                foreach (var item in SaleItemList)
                {
                    SalesCartItem newItem = new SalesCartItem();

                    newItem.EmployeeID = id;
                    var product = (from x in context.Products
                                   where x.ItemName.Equals(item.Product)
                                   select x).FirstOrDefault();
                    newItem.ProductID = product.ProductID;
                  
                        newItem.Quantity = item.Quantity;
                        newItem.UnitPrice = item.Price;
                        context.SalesCartItems.Add(newItem);

                    
                   
                }

               
                context.SaveChanges();


            }
        }
        public int PayForSalesItem(SalesItemDTO newItems)
        {
            using (var context = new eRaceContext())
            {

                /*   var exist = (from x in context.Invoices
                                  where x.EmployeeID==newItems.EmployeeID
                                  && x.InvoiceID == null
                                  select x).FirstOrDefault();

                   if(exist != null)
                   {
                       var invoicedetail = (from x in context.InvoiceDetails
                                            where x.InvoiceID == exist.InvoiceID
                                            select x).ToList();

                   }
                     
   */
               
                Invoice newInvoice = new Invoice();
                var exists = (from x in context.Invoices
                              orderby x.InvoiceID descending
                              select x).FirstOrDefault();
                newInvoice.InvoiceDate = DateTime.Now;
                newInvoice.EmployeeID = newItems.EmployeeID;
                newInvoice.SubTotal = newItems.Subtotal;
                newInvoice.GST = newItems.Tax;
                newInvoice.Total = newItems.Total;
                context.Invoices.Add(newInvoice);
         
                foreach (var items in newItems.SaleItemsList)
                {
                    InvoiceDetail newInvoiceDetail = new InvoiceDetail();
                
                    var existInvoicedetail=(from x in context.Invoices
                                            orderby x.InvoiceID descending
                                            select x).FirstOrDefault();
                   
                    var selectproductname= (from x in context.Products
                                            where x.ItemName == items.Product
                                            select x).FirstOrDefault();

                    newInvoiceDetail.ProductID = selectproductname.ProductID;
                    newInvoiceDetail.InvoiceID = existInvoicedetail.InvoiceID + 1;
                    newInvoiceDetail.Quantity = items.Quantity;
                    newInvoiceDetail.Price = items.Price;


                    selectproductname.QuantityOnHand -= items.Quantity;
                    context.Entry(selectproductname).Property(y => y.QuantityOnHand).IsModified = true;
                    context.InvoiceDetails.Add(newInvoiceDetail);


                    var existsinslaecartitems = (from x in context.SalesCartItems
                                                 where x.EmployeeID.Equals(newItems.EmployeeID)
                                                 select x).ToList();
                    foreach (var item in existsinslaecartitems)
                    {
                        context.SalesCartItems.Remove(item);
                    }

                }

                context.SaveChanges();
                var existsInvoiceID = (from x in context.Invoices
                              orderby x.InvoiceID descending
                              select x.InvoiceID).FirstOrDefault();
                return existsInvoiceID;
            }

        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public void Add_SaleItemslist(string productname, int qty, int id)
        {
            using (var context = new eRaceContext())
            {
                List<string> reasons = new List<string>();
                int productid = int.Parse(productname);
                SalesCartItem exists = (from x in context.SalesCartItems
                                       where x.Product.ProductID.Equals(productid)
                                       && x.EmployeeID.Equals(id)
                                       select x).FirstOrDefault();
                
                if (exists == null)
                {
                   /* InvoiceDetail unitprice = (from y in context.InvoiceDetails
                                               where y.Product.ItemName == productname
                                               select y
                                               ).FirstOrDefault();

*/
                    
                    exists = new SalesCartItem();
                    /*   exists.ProductID = from x in context.Products
                                             where x.ItemName.Equals(productname)
                                             select x.ProductID;*/

                        /*product name is product id*/
                 
                    var selectedProduct = (from x in context.Products
                                           where x.ProductID == productid
                                           select x).FirstOrDefault();
                    exists.ProductID = selectedProduct.ProductID;
                   
                    /*can't add name, not in table*/
                    /*exists.Product.ItemName = productname;*/
                    /*Object reference not set to an instance of an object.*/
                
                    exists.EmployeeID = id;
                    exists.Quantity = qty;
                    /*temperaty*/
                    /*exists.UnitPrice = context.Products.FirstOrDefault(b=>b.ProductID==exists.ProductID).ItemPrice;*/
                    exists.UnitPrice = selectedProduct.ItemPrice;
                    /* exists.ProductID =0;*/

                    exists = context.SalesCartItems.Add(exists);
                   
                    context.SaveChanges();

                }
                else
                {
                    /*update the quantity of exists value*/
                    /*doesn't work, fix later*/
                    //SalesCartItem currentQuantity = context.SalesCartItems.Find(productid,id);
                    exists.Quantity += qty;
                    //currentQuantity.ProductID = productid;
                    //if(productid>0)
                    //{
                    //   currentQuantity.Quantity+=qty;
                    //}
                    context.Entry(exists).Property(y => y.Quantity).IsModified = true;
                    context.SaveChanges();

                }
             
             
            }
        }
        public void PlaceNewOrder (int id, SalesItemDTO newOrder)
        {
            using(var context = new eRaceContext())
            {
                Invoice newInvoice = new Invoice();
                newInvoice.InvoiceDate = DateTime.Now;
                newInvoice.EmployeeID = id;
                newInvoice.SubTotal = newOrder.Subtotal;
                newInvoice.GST = newOrder.Tax;
                newInvoice.Total = newOrder.Total;
                context.Invoices.Add(newInvoice);

                var exists = (from x in context.Invoices
                              where x.InvoiceDate == DateTime.Now
                              select x).FirstOrDefault();
                if(exists == null)
                {
                    throw new Exception("What should i do");
                }
                else
                {
                    foreach (var item in newOrder.SaleItemsList)
                    {
                        InvoiceDetail newInvoiceDetail = new InvoiceDetail();
                        newInvoiceDetail.InvoiceID = exists.InvoiceID;
                        newInvoiceDetail.ProductID = item.ProductID;
                        newInvoiceDetail.Quantity = item.Quantity;
                        newInvoiceDetail.Price = item.Price;
                        context.InvoiceDetails.Add(newInvoiceDetail);
                    }
                }
                context.SaveChanges();

            }

        }





    }
}
