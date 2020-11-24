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
#endregion

namespace eRaceSystem.BLL.SaleBLL
{
    [DataObject]
    public class RefundController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RefundPOCO> List_Invocie(int invoicenumber)
        {
            using (var context = new eRaceContext())
            {


                var results = from x in context.InvoiceDetails
                              where x.Invoice.InvoiceID == invoicenumber
                              select new RefundPOCO
                              {
                                  Category=x.Product.Category.Description,
                                  Product = x.Product.ItemName,
                                  Qty = x.Quantity,
                                  Price = x.Price,
                                   Amount=x.Quantity*x.Price,
                                  RestockChg = x.Product.ReStockCharge,
                                  RefundReason = null
                              };
               
                return results.ToList();
            }
        }

        public int RefundForSalesItem(RefundDTO newItems)
        {
            using (var context = new eRaceContext())
            {

                Invoice newInvoice = new Invoice();
                var exists = (from x in context.Invoices
                              orderby x.InvoiceID descending
                              select x).FirstOrDefault();
                newInvoice.InvoiceDate = DateTime.Now;
                newInvoice.EmployeeID = newItems.EmployeeID;
                newInvoice.SubTotal = newItems.Subtotal;
                newInvoice.GST = newItems.GST;
                newInvoice.Total = newItems.Total;

                context.Invoices.Add(newInvoice);

                foreach (var items in newItems.RefundList)
                {
                    StoreRefund newRefundItems = new StoreRefund();

                    var existInvoicedetail = (from x in context.Invoices
                                              orderby x.InvoiceID descending
                                              select x).FirstOrDefault();
                    newRefundItems.InvoiceID = existInvoicedetail.InvoiceID + 1;
                    newRefundItems.OriginalInvoiceID = newItems.OriginalInvoiceID;

                    var selectproductname = (from x in context.Products
                                             where x.ItemName == items.Product
                                             select x).FirstOrDefault();

                    selectproductname.QuantityOnHand += items.Qty;
                    context.Entry(selectproductname).Property(y => y.QuantityOnHand).IsModified = true;
                    newRefundItems.ProductID = selectproductname.ProductID;
                    newRefundItems.Reason = items.RefundReason;
                    context.StoreRefunds.Add(newRefundItems);
                }

                context.SaveChanges();

                var existsInvoiceID = (from x in context.Invoices
                                       orderby x.InvoiceID descending
                                       select x.InvoiceID).FirstOrDefault();
                return existsInvoiceID;

            }
        }
        public StoreRefund LookUpIfRefunded(int originalInvoiceID, string productname)
        {
            using(var context = new eRaceContext())
            {
                var selectedProduct = (from x in context.Products
                                       where x.ItemName == productname
                                       select x.ProductID).FirstOrDefault();


                var exist = (from x in context.StoreRefunds
                             where x.OriginalInvoiceID == originalInvoiceID && x.ProductID == selectedProduct
                             select x).FirstOrDefault();
                if(exist == null)
                {
                    exist = new StoreRefund();
                }


                return exist;
            }
        }


    }
}
