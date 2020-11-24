using eRaceSystem.BLL;
using eRaceSystem.BLL.SaleBLL;
using eRaceSystem.Data.DTOs.SalesDTO;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs.SalePOCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamAssignment.Security;

namespace TeamAssignment.ProjectPages
{
    public partial class SaleSubSystem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Request.IsAuthenticated)
            {
                if ( User.IsInRole("Clerk"))
                {
                    var username = User.Identity.Name;
                    SecurityController securitymgr = new SecurityController();
                    int? userid = securitymgr.GetCurrentUserId(username);
                    if (userid.HasValue)
                    {
                        MessageUserControl.TryRun(() => {
                            EmployeeController sysmgr = new EmployeeController();
                            Employee info = sysmgr.Get_Employee(userid.Value);
                            UserName.Text = info.FirstName + " " + info.LastName;
                          /*  SaleProductController sysmgrA = new SaleProductController();
                            List<SaleItemsPOCO> datainfo = sysmgrA.List_ItemsNames(info.EmployeeID);
                            ListItem.DataSource = datainfo;
                            ListItem.DataBind();*/

                        });
                    }
                    else
                    {
                        MessageUserControl.ShowInfo("UnRegistered User", "This user is not a registered customer");
                        UserName.Text = "Unregistered User";
                    }
                }
                else
                {
                    //redirect to a page that states no authorization fot the request action
                    Response.Redirect("~/Security/AccessDenied.aspx");
                }
            }
            else
            {
                //redirect to login page
                Response.Redirect("~/Account/Login.aspx");
            }
            SearchRefund.DataSource = null;
          
        }
        protected void AddSale_OnClick(object sender, EventArgs e)
        {

            if (InvoiceID.Text != "")
            {
                MessageUserControl.ShowInfo("Error", "Sorry, you must clear the table to be able to add new items");

            }
            else
            {
                if (string.IsNullOrEmpty(Quantity.Text))
                {
                    MessageUserControl.ShowInfo("Missing Data",
                        "Please enter something to add.");

                }
                else if (!int.TryParse(Quantity.Text, out int result))
                {
                    MessageUserControl.ShowInfo("Missing Data",
                        "Please enter a number.");
                }
                else if (int.Parse(Quantity.Text) <= 0)
                {
                    MessageUserControl.ShowInfo("Error",
                        "Number must be bigger than 0.");
                }
                else if (SelectCategory.SelectedIndex == 0)
                {
                    MessageUserControl.ShowInfo("Missing Data",
                        "Please select the category for product.");
                }
                else
                {
                    MessageUserControl.TryRun(() =>
                    {
                        var username = User.Identity.Name;
                        SecurityController securitymgr = new SecurityController();
                    /* string categoryName = SelectCategory.SelectedValue;*/
                        string productName = SelectProductFromCategory.SelectedValue;
                        int quantity = int.Parse(Quantity.Text);
                        int? userid = securitymgr.GetCurrentUserId(username);
                        int id;

                        if (userid.HasValue)
                        {
                            id = userid.Value;


                            SaleProductController sysmgr = new SaleProductController();
                            sysmgr.Add_SaleItemslist(productName, quantity, id);

                            List<SaleItemsPOCO> datainfo = sysmgr.List_ItemsNames(id);
                            ListItem.DataSource = datainfo;

                            ListItem.DataBind();
                        /*not done yet*/

                        //SalesItemDTO Orders = new SalesItemDTO();
                        //SaleProductController addPrice = new SaleProductController();
                        //Orders = addPrice.GetListInforForPrice(id, productName);
                        decimal subtotal = 0;
                            foreach (ListViewItem item in ListItem.Items)
                            {
                                subtotal += decimal.Parse((item.FindControl("TotalAmountLabel") as Label).Text);
                            /* decimal UnitPrice = decimal.Parse((item.FindControl("ItemPriceLabel") as Label).Text);
                             int Qty = int.Parse((item.FindControl("QuantityLabel") as TextBox).Text);*/
                            }
                        /*            addPrice.GetListInforForPrice(id, productName);*/
                            SaleItemSubTotal.Text = subtotal.ToString("0.00");
                            SaleItemTax.Text = (subtotal * (decimal)0.05).ToString("0.00");
                            SaleItemTotal.Text = (subtotal * (decimal)1.05).ToString("0.00");


                        }


                    }, "Success", "Item Added");

                }
            }
        }
       
        protected void SearchInvoice_Onclick(object sender, EventArgs e)
        {
            if (!int.TryParse(InvoiceNumberDDL.Text, out int result))
            {
                /*must be interger*/
                MessageUserControl.ShowInfo("Error",
                    "Please enter a invoice id.");
                List<RefundPOCO> datainfo = new List<RefundPOCO>();
                SearchRefund.DataSource = datainfo;
                SearchRefund.DataBind();
            }
            else
            {
             
                MessageUserControl.TryRun(() =>
                {
                    int invoicenumber = int.Parse(InvoiceNumberDDL.Text);
                    InvisibleInvoiceID.Text = invoicenumber.ToString();
                    RefundController sysmgr = new RefundController();
                    List<RefundPOCO> datainfo = sysmgr.List_Invocie(invoicenumber);
                    if(datainfo.Count() == 0)
                    {
                        List<RefundPOCO> datainfod = new List<RefundPOCO>();
                        SearchRefund.DataSource = datainfod;
                        SearchRefund.DataBind();
                        throw new Exception("Can't find any Invoice for entered Invoice ID");
                    }
                    RefundPOCO RefundItem = new RefundPOCO();
                    SearchRefund.DataSource = datainfo;
                    SearchRefund.DataBind();
                    CheckBox checkforrefund = null;
                    CheckBox checkforrefund2 = null;

                    SubtotalForFefund.Text = 0.ToString("0.00");
                    TaxForRefund.Text = 0.ToString("0.00");
                    RefundTotalAmount.Text = 0.ToString("0.00");


                    foreach (ListViewItem items in SearchRefund.Items)
                    {
                        StoreRefund storeRefund = sysmgr.LookUpIfRefunded(invoicenumber, (items.FindControl("ProductLabel") as Label).Text); 
                        if (storeRefund.Reason != null)
                        {
                            (items.FindControl("CheckForRefundReason") as CheckBox).Enabled = false;
                            (items.FindControl("RefundReasonLabel") as TextBox).Enabled = false;
                            (items.FindControl("CheckForRefundReason") as CheckBox).Checked = true;
                            (items.FindControl("RestockChargeCheck") as CheckBox).Checked = true;
                            (items.FindControl("RefundReasonLabel") as TextBox).Text = storeRefund.Reason;
                        }

                        RefundItem.Category = (items.FindControl("CategoryDescription") as Label).Text;
                        RefundItem.RestockChg = decimal.Parse((items.FindControl("RestockChgLabel") as Label).Text);
                    /*    RefundItem.RefundReason = (items.FindControl("RefundReasonLabel") as Label).Text;*/
                        checkforrefund = items.FindControl("CheckForRefundReason") as CheckBox;
                        checkforrefund2 = items.FindControl("RestockChargeCheck") as CheckBox;

                        if (RefundItem.Category == "Confectionary")
                        {
                            (items.FindControl("RestockChgLabel") as Label).Visible = false;
                            (items.FindControl("CheckForRefundReason") as CheckBox).Visible = false;
                            (items.FindControl("RestockChargeCheck") as CheckBox).Visible = false;
                            (items.FindControl("RefundReasonLabel") as TextBox).Visible = false;

                        }
                        else if (RefundItem.RestockChg<=0)
                        {
                            (items.FindControl("RestockChgLabel") as Label).Visible = false;
                        }
                      
                    }
                  
                }, "OrderList", "Below are the order for refund");
            }
        }
        protected void Check_Changed(object sender, EventArgs e)
        {

            MessageUserControl.TryRun(() =>
            {
                RefundPOCO RefundItem = new RefundPOCO();
                CheckBox checkforrefund = null;
                CheckBox checkforrefund2 = null;
                decimal subtotal = 0;
                foreach (ListViewItem items in SearchRefund.Items)
                {
                    if ((items.FindControl("CheckForRefundReason") as CheckBox).Enabled == true)
                    {
                        RefundItem.RestockChg = decimal.Parse((items.FindControl("RestockChgLabel") as Label).Text);
                        /*    RefundItem.RefundReason = (items.FindControl("RefundReasonLabel") as Label).Text;*/
                        checkforrefund = items.FindControl("CheckForRefundReason") as CheckBox;
                        checkforrefund2 = items.FindControl("RestockChargeCheck") as CheckBox;

                        if (checkforrefund.Checked)
                        {
                            if (RefundItem.RestockChg > 0)
                            {
                                (items.FindControl("RestockChargeCheck") as CheckBox).Checked = true;

                            }
                            else
                            {
                                checkforrefund2.Checked = false;
                            }

                            subtotal -= decimal.Parse((items.FindControl("AmountLabel") as Label).Text);
                            subtotal += RefundItem.RestockChg;
                        }
                        else
                        {
                            (items.FindControl("RestockChargeCheck") as CheckBox).Checked = false;
                        }

                    }
                    SubtotalForFefund.Text = subtotal.ToString("0.00");
                    TaxForRefund.Text = (subtotal * (decimal)0.05).ToString("0.00");
                    RefundTotalAmount.Text = (subtotal * (decimal)1.05).ToString("0.00");
                }
            });
        }
        protected void Clearrefund(object sender, EventArgs e)
        {
           
            InvoiceNumberDDL.Text = "";
            SubtotalForFefund.Text = null;
            TaxForRefund.Text = null;
            RefundTotalAmount.Text = null;
            NewRefundInvoiceID.Text = "";
            InvisibleInvoiceID.Text = "";
            List<RefundPOCO> datainfo = new List<RefundPOCO>();
            SearchRefund.DataSource = datainfo;
            SearchRefund.DataBind();

        }

        protected void ClearSaleItem(object sender, EventArgs e)
        {
            SelectCategory.SelectedIndex = 0;
            SelectProductFromCategory.SelectedValue = null;
            Quantity.Text = 1.ToString();
            SaleItemSubTotal.Text = null;
            SaleItemTax.Text = null;
            SaleItemTotal.Text = null;
            InvoiceID.Text = "";
            var username = User.Identity.Name;
            SecurityController securitymgr = new SecurityController();

            int? userid = securitymgr.GetCurrentUserId(username);
            int id;

            MessageUserControl.TryRun(() =>
            {
                if (userid.HasValue)
                {
                    id = userid.Value;

                    SaleProductController deleteall = new SaleProductController();
                    deleteall.Clear_SaleItemslist(id);
                    SaleProductController sysmgrA = new SaleProductController();
                    List<SaleItemsPOCO> datainfo = sysmgrA.List_ItemsNames(id);
                    ListItem.DataSource = datainfo;
                    ListItem.DataBind();

                }
            }, "Success", "All items deleted");


        }
      
        protected void Delete_SingleItem(object sender, CommandEventArgs e)
        {

            if (InvoiceID.Text != "")
            {
                MessageUserControl.ShowInfo("Error", "Sorry, you must clear the table to be able to add new items");

            }
            else
            {
                foreach (var item in ListItem.Items)
                {
                    if ((item.FindControl("DeleteItem") as LinkButton).CommandArgument == e.CommandArgument.ToString())
                    {
                        /*MessageUserControl.ShowInfo("OOOOPSSS",
                        e.CommandArgument.ToString());*/
                        string productname = (item.FindControl("ItemNameLabel") as Label).Text;

                        var username = User.Identity.Name;
                        SecurityController securitymgr = new SecurityController();

                        int? userid = securitymgr.GetCurrentUserId(username);
                        int id;

                        MessageUserControl.TryRun(() =>
                        {
                            if (userid.HasValue)
                            {
                                id = userid.Value;

                                SaleProductController deleteonerow = new SaleProductController();
                                deleteonerow.Clear_SaleItemsforonerow(id, productname);
                                List<SaleItemsPOCO> datainfo = deleteonerow.List_ItemsNames(id);
                                ListItem.DataSource = datainfo;
                                ListItem.DataBind();

                            }
                        }, "Success", productname + " has been deleted");

                    }
                    decimal subtotal = 0;
                    foreach (ListViewItem items in ListItem.Items)
                    {
                        subtotal += decimal.Parse((item.FindControl("TotalAmountLabel") as Label).Text);

                    }
                    SaleItemSubTotal.Text = subtotal.ToString("0.00");
                    SaleItemTax.Text = (subtotal * (decimal)0.05).ToString("0.00");
                    SaleItemTotal.Text = (subtotal * (decimal)1.05).ToString("0.00");

                }

            }
        }
        protected void Pay_Payment(object sender, EventArgs e)
        {

            if (InvoiceID.Text != "")
            {
                MessageUserControl.ShowInfo("Error", "Sorry, you must clear the table to be able to add new items");

            }
            else
            {

                var username = User.Identity.Name;
                SecurityController securitymgr = new SecurityController();

                int? userid = securitymgr.GetCurrentUserId(username);
                int id;


                /*get the employee id first*/

                if (userid.HasValue)
                {
                    id = userid.Value;
                    /*display the items and update the items, if no info form database, display error message*/
                    SalesItemDTO additems = new SalesItemDTO();
                    additems.EmployeeID = id;
                    additems.InvoiceData = DateTime.Now;


                    MessageUserControl.TryRun(() =>
                    {
                        /*update first*/
                        int errorcount = 0;
                        decimal subtotal = 0;
                        List<SaleItemsPOCO> SaleItemList = new List<SaleItemsPOCO>();
                        foreach (ListViewItem item in ListItem.Items)
                        {

                            SaleItemsPOCO SaleItem = new SaleItemsPOCO();
                            SaleItem.Product = (item.FindControl("ItemNameLabel") as Label).Text;
                            SaleItem.Price = decimal.Parse((item.FindControl("ItemPriceLabel") as Label).Text);

                            if (!int.TryParse((item.FindControl("QuantityLabel") as TextBox).Text, out int result))
                            {
                                errorcount += 1;
                                throw new Exception("The quantity must be an interger");
                            }
                            else if (int.Parse((item.FindControl("QuantityLabel") as TextBox).Text) <= 0)
                            {
                                errorcount += 1;
                                throw new Exception("The quantity must be bigger than 0");
                            }
                            else
                            {
                                SaleItem.Quantity = int.Parse((item.FindControl("QuantityLabel") as TextBox).Text);
                            }
                            SaleItem.TotalAmount = decimal.Parse((item.FindControl("TotalAmountLabel") as Label).Text);
                            SaleItemList.Add(SaleItem);
                        }

                        if (errorcount > 0)
                        {
                            MessageUserControl.ShowInfo("Error", "The quantity must be an interger");
                        }
                        else
                        {


                            SaleProductController sysmgr = new SaleProductController();
                            sysmgr.Update_SaleItemAmount(id, SaleItemList);

                            SaleProductController sysmgrA = new SaleProductController();
                            List<SaleItemsPOCO> datainfo = sysmgrA.List_ItemsNames(id);
                            ListItem.DataSource = datainfo;
                            ListItem.DataBind();
                            foreach (ListViewItem item in ListItem.Items)
                            {
                                subtotal += decimal.Parse((item.FindControl("TotalAmountLabel") as Label).Text);
                            }
                            SaleItemSubTotal.Text = subtotal.ToString("0.00");
                            SaleItemTax.Text = (subtotal * (decimal)0.05).ToString("0.00");
                            SaleItemTotal.Text = (subtotal * (decimal)1.05).ToString("0.00");
                            additems.Subtotal = decimal.Parse(SaleItemSubTotal.Text);
                            additems.Tax = decimal.Parse(SaleItemTax.Text);
                            additems.Total = decimal.Parse(SaleItemTotal.Text);

                        }
                        /*pass records*/
                        if (SaleItemList.Count() == 0)
                        {
                            throw new Exception("No item for paid");
                        }
                        else
                        {
                            additems.SaleItemsList = SaleItemList;
                        }


                        SaleProductController sys = new SaleProductController();
                        int newInvoiceid = sys.PayForSalesItem(additems);
                        InvoiceID.Text = newInvoiceid.ToString();

                    }, "Success", "Paid successfully");


                    if(InvoiceID.Text != "")
                    {
                        foreach (var item in ListItem.Items)
                        {
                            (item.FindControl("QuantityLabel") as TextBox).Enabled=false;
                        }
                    }

                }

            }


        }


        protected void Refund_Generate(object sender, EventArgs e)
        {
            if(InvisibleInvoiceID.Text == "")
            {
                MessageUserControl.ShowInfo("Please enter a Invoice ID first");
            }
            else
            {
                var username = User.Identity.Name;
                SecurityController securitymgr = new SecurityController();

                int? userid = securitymgr.GetCurrentUserId(username);
                int id;

                MessageUserControl.TryRun(() =>
                {

                    if (userid.HasValue)
                    {
                        id = userid.Value;
                        /*display the items and update the items, if no info form database, display error message*/
                        RefundDTO refunditems = new RefundDTO();
                        refunditems.EmployeeID = id;
                        refunditems.InvoiceDate = DateTime.Now;
                        refunditems.Subtotal = decimal.Parse(SubtotalForFefund.Text);
                        refunditems.GST = decimal.Parse(TaxForRefund.Text);
                        refunditems.Total = decimal.Parse(RefundTotalAmount.Text);
                        refunditems.OriginalInvoiceID = int.Parse(InvisibleInvoiceID.Text);
                        CheckBox checkforrefund = null;
                        CheckBox checkforrefund2 = null;
                        List<RefundPOCO> RefundItemList = new List<RefundPOCO>();
                        foreach (ListViewItem item in SearchRefund.Items)
                        {
                            if ((item.FindControl("CheckForRefundReason") as CheckBox).Enabled == true)
                            {
                                checkforrefund = item.FindControl("CheckForRefundReason") as CheckBox;
                                checkforrefund2 = item.FindControl("RestockChargeCheck") as CheckBox;

                                if (checkforrefund.Checked)
                                {

                                    if ((item.FindControl("RefundReasonLabel") as TextBox).Text == "")
                                    {
                                        throw new Exception("You must enter a reason for the selected product");
                                    }
                                    else
                                    {
                                        RefundPOCO refunditem = new RefundPOCO();
                                        refunditem.RefundReason = (item.FindControl("RefundReasonLabel") as TextBox).Text;
                                        refunditem.Product = (item.FindControl("ProductLabel") as Label).Text;
                                        refunditem.Qty=int.Parse((item.FindControl("QtyLabel") as Label).Text);
                                        RefundItemList.Add(refunditem);
                                    }
                                }
                            }
                        }
                        if (RefundItemList.Count() == 0)
                        {
                            // show info
                            throw new Exception("You must select at lease one product for refund");
                        }
                        else
                        {
                            refunditems.RefundList = RefundItemList;
                            RefundController sys = new RefundController();
                            int newInvoiceid = sys.RefundForSalesItem(refunditems);
                            NewRefundInvoiceID.Text = newInvoiceid.ToString();
                        }




                    }
                }, "Success", "Selected Item refunded.");

                MessageUserControl.TryRun(() =>
                {
                    RefundController sysmgr = new RefundController();
                    foreach (ListViewItem items in SearchRefund.Items)
                    {
                        StoreRefund storeRefund = sysmgr.LookUpIfRefunded(int.Parse(InvisibleInvoiceID.Text), (items.FindControl("ProductLabel") as Label).Text);
                        if (storeRefund.Reason != null)
                        {
                            (items.FindControl("CheckForRefundReason") as CheckBox).Enabled = false;
                            (items.FindControl("RefundReasonLabel") as TextBox).Enabled = false;
                            (items.FindControl("CheckForRefundReason") as CheckBox).Checked = true;
                            (items.FindControl("RestockChargeCheck") as CheckBox).Checked = true;
                            (items.FindControl("RefundReasonLabel") as TextBox).Text = storeRefund.Reason;

                        }

                    }

                });
            }
            
        }




        protected void Refresh_Quantity(object sender, EventArgs e)
        {


            if (InvoiceID.Text != "")
            {
                MessageUserControl.ShowInfo("Error","Sorry, you must clear the table to be able to add new items");
                
            }
            else
            {
                List<SaleItemsPOCO> SaleItemList = new List<SaleItemsPOCO>();

                /*refresh need to update the Database*/
                int errorcount = 0;
                MessageUserControl.TryRun(() =>
                {
                    decimal subtotal = 0;
                    foreach (ListViewItem item in ListItem.Items)
                    {
                        SaleItemsPOCO SaleItem = new SaleItemsPOCO();
                        SaleItem.Product = (item.FindControl("ItemNameLabel") as Label).Text;
                        SaleItem.Price = decimal.Parse((item.FindControl("ItemPriceLabel") as Label).Text);

                        if (!int.TryParse((item.FindControl("QuantityLabel") as TextBox).Text, out int result))
                        {
                            errorcount += 1;
                            throw new Exception("The quantity must be an interger");
                        }
                        else if (int.Parse((item.FindControl("QuantityLabel") as TextBox).Text) <= 0)
                        {
                            errorcount += 1;
                            throw new Exception("The quantity must be bigger than 0");
                        }
                        else
                        {
                            SaleItem.Quantity = int.Parse((item.FindControl("QuantityLabel") as TextBox).Text);
                        }
                        SaleItem.TotalAmount = decimal.Parse((item.FindControl("TotalAmountLabel") as Label).Text);
                        SaleItemList.Add(SaleItem);


                    }

                    if (errorcount > 0)
                    {
                        MessageUserControl.ShowInfo("Error", "The quantity must be an interger");
                    }
                    else
                    {
                        var username = User.Identity.Name;
                        SecurityController securitymgr = new SecurityController();

                        int? userid = securitymgr.GetCurrentUserId(username);
                        int id;
                        if (userid.HasValue)
                        {
                            id = userid.Value;

                            SaleProductController sysmgr = new SaleProductController();
                            sysmgr.Update_SaleItemAmount(id, SaleItemList);

                            SaleProductController sysmgrA = new SaleProductController();
                            List<SaleItemsPOCO> datainfo = sysmgrA.List_ItemsNames(id);
                            ListItem.DataSource = datainfo;
                            ListItem.DataBind();
                            foreach (ListViewItem item in ListItem.Items)
                            {
                                subtotal += decimal.Parse((item.FindControl("TotalAmountLabel") as Label).Text);
                            }
                            SaleItemSubTotal.Text = subtotal.ToString("0.00");
                            SaleItemTax.Text = (subtotal * (decimal)0.05).ToString("0.00");
                            SaleItemTotal.Text = (subtotal * (decimal)1.05).ToString("0.00");

                        }
                    }

                }, "success", "Item quantities and amount updated");
            }
           
          
        }


    }
}