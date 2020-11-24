using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamAssignment.Security;

#region Addtional namespaces
using eRaceSystem.BLL;
using eRaceSystem.Data.Entities;
using eRaceSystem.BLL.PurchasingBLL;
using eRaceSystem.Data.POCOs.PurchasingPOCO;
using eRaceSystem.Data.DTOs.PurchasingDTO;
#endregion

namespace TeamAssignment.ProjectPages
{
    public partial class PurchasingSubSystem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Director")||User.IsInRole("Office Manager") || User.IsInRole("Clerk"))
                {
                    var username = User.Identity.Name;
                    SecurityController securitymgr = new SecurityController();
                    int? customerid = securitymgr.GetCurrentUserId(username);
                    if (customerid.HasValue)
                    {
                        MessageUserControl.TryRun(() => {
                            EmployeeController sysmgr = new EmployeeController();
                            Employee info = sysmgr.Get_Employee(customerid.Value);
                            UserName.Text = info.FirstName+ " "+info.LastName;
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
        }

        protected void Select_Click(object sender, EventArgs e)
        {
            var username = User.Identity.Name;
            SecurityController securitymgr = new SecurityController();
            int? customerid = securitymgr.GetCurrentUserId(username);
            Employee info = new Employee();
            if (customerid.HasValue)
            {
                MessageUserControl.TryRun(() => {
                    EmployeeController sysmgrUser = new EmployeeController();
                    info = sysmgrUser.Get_Employee(customerid.Value);
                });
            }


            VendorController VendorController = new VendorController();
            Vendor selectedVendor = VendorController.Vendor_Get(int.Parse(VendorNameDropDownList.SelectedValue));
            VendorInfo.Text = selectedVendor.Name + " - " + selectedVendor.Contact + " - " + selectedVendor.Phone;



            InventoryListController sysmgr = new InventoryListController();
            List<InventoryListDTO> datainfo = new List<InventoryListDTO>();
            List<InventoryListDTO> filteredDatainfo = new List<InventoryListDTO>();
            MessageUserControl.TryRun(() =>
            {
                datainfo = sysmgr.GetVendorInventory(int.Parse(VendorNameDropDownList.SelectedValue));
                List<int> empty = new List<int>();
                for (int index = 0; index < 4; index++)
                {
                    if (datainfo[index].ProductList.Count() > 0)
                    {
                        filteredDatainfo.Add(datainfo[index]);
                    }
                }
                if(filteredDatainfo.Count() == 0)
                {
                    RepeaterInventory.DataSource = "";
                    RepeaterInventory.DataBind();
                    throw new Exception("The vendor you selected has no vendorcatalog");
                }
                RepeaterInventory.DataSource = filteredDatainfo;
                RepeaterInventory.DataBind();

                OrderController OrderContoller = new OrderController();
                OrderLogDTO OrderLog = new OrderLogDTO();

                OrderLog = OrderContoller.GetVendorOrderLog(int.Parse(VendorNameDropDownList.SelectedValue), info.EmployeeID);
                ListView2.DataSource = OrderLog.ItemList;
                ListView2.DataBind();
                foreach (var item in ListView2.Items)
                {
                    double UnitSize = double.Parse((item.FindControl("UnitSizeLabel") as Label).Text);
                    double UnitCost = double.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                    double Qty = double.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);

                    (item.FindControl("PerItemCostLabel") as Label).Text = (UnitCost / UnitSize).ToString("0.00");
                    (item.FindControl("ExtendedCostLabel") as Label).Text = (Qty * UnitCost).ToString("0.00");
                }
                Subtotal.Text = OrderLog.SubTotal.ToString("0.00");
                Tax.Text = OrderLog.TaxGST.ToString("0.00");
                Total.Text = (OrderLog.SubTotal + OrderLog.TaxGST).ToString("0.00");
                Comments.Text = OrderLog.Comment;
                vendorValidate.Text = VendorNameDropDownList.SelectedValue;
                VendorNameDropDownList.Enabled = false;
                Select.Enabled = false;
            }, "Success", "VendorSelected");

        }

        protected void TracksSelectionList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var username = User.Identity.Name;
            SecurityController securitymgr = new SecurityController();
            int? customerid = securitymgr.GetCurrentUserId(username);
            Employee info = new Employee();
            if (customerid.HasValue)
            {
                MessageUserControl.TryRun(() =>
                {
                    EmployeeController sysmgrUser = new EmployeeController();
                    info = sysmgrUser.Get_Employee(customerid.Value);
                });
            }
            MessageUserControl.TryRun(() =>
            {
                List <OrderItemPOCO> SelectionList = new List<OrderItemPOCO>();
                foreach (ListViewItem item in ListView2.Items)
                {
                    OrderItemPOCO oldItems = new OrderItemPOCO();
                    oldItems.OrderDetailID = item.DataItemIndex;
                    // try it out
                    oldItems.Product = (item.FindControl("ProductLabel") as Label).Text;
                    if (!int.TryParse((item.FindControl("OrderQtyLabel") as TextBox).Text, out int test))
                    {
                        throw new Exception(oldItems.Product + ": Quantity can only be an integer that is greater than 0");
                    }
                    else if (int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) <= 0)
                    {
                        throw new Exception(oldItems.Product + ": Quantity can only be an integer that is greater than 0");
                    }
                    else
                    {
                        oldItems.OrderQty = int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);
                    }
                    oldItems.UnitSize = int.Parse((item.FindControl("UnitSizeLabel") as Label).Text);
                    if (!decimal.TryParse((item.FindControl("UnitCostLabel") as TextBox).Text, out decimal tesst))
                    {
                        throw new Exception(oldItems.Product + ": Unit Cost can only be an integer that is greater than 0");
                    }
                    else if (decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text) <= 0)
                    {
                        throw new Exception(oldItems.Product + ": Unit Cost can only be an integer that is greater than 0");
                    }
                    else
                    {
                        oldItems.UnitCost = decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                    }
                    SelectionList.Add(oldItems);
                }

                OrderController orderController = new OrderController();
                int productid = int.Parse(e.CommandArgument.ToString());
                OrderItemPOCO newItem = new OrderItemPOCO();
                newItem.OrderDetailID = ListView2.Items.Count();
                int errorcount = 0;
                newItem.Product = orderController.GetProductPOCO(productid, int.Parse(vendorValidate.Text)).ItemName;
                newItem.OrderQty = 1;
                foreach (var item in ListView2.Items)
                {
                    if(newItem.Product == (item.FindControl("ProductLabel") as Label).Text)
                    {
                        int quantity = int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) + 1;
                        (item.FindControl("OrderQtyLabel") as TextBox).Text = quantity.ToString();
                        errorcount += 1;
                    }
                }
                newItem.UnitSize = orderController.GetProductPOCO(productid, int.Parse(vendorValidate.Text)).UnitSize;
                newItem.UnitCost = orderController.GetProductPOCO(productid, int.Parse(vendorValidate.Text)).UnitCost;
                if(errorcount == 0)
                {
                    SelectionList.Add(newItem);
                    ListView2.DataSource = SelectionList;
                    ListView2.DataBind();
                }
                
            });
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if(vendorValidate.Text == "")
            {
                MessageUserControl.ShowInfo("Alert","Please select a vendor first to continue");
            }
            else
            {
                decimal subtotal = 0;
                var username = User.Identity.Name;
                SecurityController securitymgr = new SecurityController();
                int? customerid = securitymgr.GetCurrentUserId(username);
                Employee info = new Employee();
                if (customerid.HasValue)
                {
                    MessageUserControl.TryRun(() => {
                        EmployeeController sysmgrUser = new EmployeeController();
                        info = sysmgrUser.Get_Employee(customerid.Value);
                    });
                }
                MessageUserControl.TryRun(() => {
                    List<OrderItemPOCO> SelectionList = new List<OrderItemPOCO>();
                    OrderController orderController = new OrderController();
                    foreach (ListViewItem item in ListView2.Items)
                    {
                        OrderItemPOCO oldItems = new OrderItemPOCO();
                        oldItems.OrderDetailID = null;
                        oldItems.Product = (item.FindControl("ProductLabel") as Label).Text;

                        if (!int.TryParse((item.FindControl("OrderQtyLabel") as TextBox).Text, out int test))
                        {
                            throw new Exception(oldItems.Product + ": Quantity can only be an integer that is greater than 0");
                        }
                        else if (int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) <= 0)
                        {
                            throw new Exception(oldItems.Product + ": Quantity can only be an integer that is greater than 0");
                        }
                        else
                        {
                            oldItems.OrderQty = int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);
                        }
                        oldItems.UnitSize = int.Parse((item.FindControl("UnitSizeLabel") as Label).Text);

                        if (!decimal.TryParse((item.FindControl("UnitCostLabel") as TextBox).Text, out decimal tesst))
                        {
                            throw new Exception(oldItems.Product + ": Unit Cost can only be an integer that is greater than 0");
                        }
                        else if (decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text) <= 0)
                        {
                            throw new Exception(oldItems.Product + ": Unit Cost can only be an integer that is greater than 0");
                        }
                        else
                        {
                            oldItems.UnitCost = decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                        }

                        SelectionList.Add(oldItems);
                        subtotal += (oldItems.UnitCost * oldItems.OrderQty);
                    }
                    OrderLogDTO AnewOrder = new OrderLogDTO();
                    AnewOrder.ItemList = SelectionList;
                    AnewOrder.Comment = Comments.Text;
                    AnewOrder.SubTotal = subtotal;

                    orderController.UpdateOrder(int.Parse(vendorValidate.Text), AnewOrder);
                    VendorNameDropDownList.Enabled = true;
                    Select.Enabled = true;
                }, "Success", "Order saved.");
            }
        }

        protected void PlaceOrder_Click(object sender, EventArgs e)
        {
            if (vendorValidate.Text == "")
            {
                MessageUserControl.ShowInfo("Alert", "Please select a vendor first to continue");
            }
            else
            {
                var username = User.Identity.Name;
                SecurityController securitymgr = new SecurityController();
                int? customerid = securitymgr.GetCurrentUserId(username);
                Employee info = new Employee();
                if (customerid.HasValue)
                {
                    MessageUserControl.TryRun(() => {
                        EmployeeController sysmgrUser = new EmployeeController();
                        info = sysmgrUser.Get_Employee(customerid.Value);
                    });
                }

                OrderLogDTO NewOrder = new OrderLogDTO();
                NewOrder.OrderDate = DateTime.Now;
                NewOrder.EmployeeID = info.EmployeeID;
                MessageUserControl.TryRun(() => {
                    double subtotal = 0;
                    foreach (var item in ListView2.Items)
                    {
                        double UnitCost;
                        double Qty;
                        string productmessage = (item.FindControl("ProductLabel") as Label).Text;
                        if (!decimal.TryParse((item.FindControl("UnitCostLabel") as TextBox).Text, out decimal tesst))
                        {
                            throw new Exception(productmessage + ": Unit Cost can only be an integer that is greater than 0");
                        }
                        else if (decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text) <= 0)
                        {
                            throw new Exception(productmessage + ": Unit Cost can only be an integer that is greater than 0");
                        }
                        else
                        {
                            UnitCost = double.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                        }

                        if (!int.TryParse((item.FindControl("OrderQtyLabel") as TextBox).Text, out int test))
                        {
                            throw new Exception(productmessage + ": Quantity can only be an integer that is greater than 0");
                        }
                        else if (int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) <= 0)
                        {
                            throw new Exception(productmessage + ": Quantity can only be an integer that is greater than 0");
                        }
                        else
                        {
                            Qty = double.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);
                        }
                        double UnitSize = double.Parse((item.FindControl("UnitSizeLabel") as Label).Text);
                        (item.FindControl("PerItemCostLabel") as Label).Text = (UnitCost / UnitSize).ToString("0.00");
                        (item.FindControl("ExtendedCostLabel") as Label).Text = (Qty * UnitCost).ToString("0.00");
                        subtotal += Qty * UnitCost;
                        Subtotal.Text = subtotal.ToString("0.00");
                        Tax.Text = (subtotal * 0.05).ToString("0.00");
                        Total.Text = (subtotal * 1.05).ToString("0.00");
                    }
                });
                NewOrder.TaxGST = decimal.Parse(Tax.Text);
                NewOrder.SubTotal = decimal.Parse(Subtotal.Text);
                NewOrder.SubTotal = decimal.Parse(Total.Text);
                if (Comments.Text == "")
                {
                    NewOrder.Comment = null;
                }
                else
                {
                    NewOrder.Comment = Comments.Text;
                }
                MessageUserControl.TryRun(() => {
                    List<OrderItemPOCO> SelectionList = new List<OrderItemPOCO>();
                    foreach (ListViewItem item in ListView2.Items)
                    {
                        OrderItemPOCO oldItems = new OrderItemPOCO();
                        oldItems.OrderDetailID = null;
                        oldItems.Product = (item.FindControl("ProductLabel") as Label).Text;

                        if (!int.TryParse((item.FindControl("OrderQtyLabel") as TextBox).Text, out int test))
                        {
                            throw new Exception("Quantity can only be an integer that is greater than 0");
                        }
                        else if (int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) <= 0)
                        {
                            throw new Exception("Quantity can only be an integer that is greater than 0");
                        }
                        else
                        {
                            oldItems.OrderQty = int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);
                        }
                        oldItems.UnitSize = int.Parse((item.FindControl("UnitSizeLabel") as Label).Text);

                        if (!decimal.TryParse((item.FindControl("UnitCostLabel") as TextBox).Text, out decimal tesst))
                        {
                            throw new Exception("Unit Cost can only be an integer that is greater than 0");
                        }
                        else if (decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text) <= 0)
                        {
                            throw new Exception("Unit Cost can only be an integer that is greater than 0");
                        }
                        else
                        {
                            oldItems.UnitCost = decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                        }

                        SelectionList.Add(oldItems);
                    }
                    NewOrder.VendorID = int.Parse(vendorValidate.Text);
                    if (SelectionList.Count() == 0)
                    {
                        throw new Exception("Add at least one item beofre placing the order");
                    }
                    else
                    {
                        NewOrder.ItemList = SelectionList;
                    }
                    OrderController sysmgr = new OrderController();
                    int newOrderNumber = int.Parse(sysmgr.PlaceOrder(NewOrder).ToString());
                    VendorNameDropDownList.Enabled = true;
                    Select.Enabled = true;
                }, "Success", "Order Placed.");
            }
            
        }
        protected void Delete_Item(object sender, CommandEventArgs e)
        {
            
            // list.remove(item)

            int itemToDelete = 0;
            foreach(var item in ListView2.Items)
            {
                if((item.FindControl("RemovefromList") as LinkButton).CommandArgument == e.CommandArgument.ToString())
                {
                    itemToDelete = item.DataItemIndex;
                }
            }

            var username = User.Identity.Name;
            SecurityController securitymgr = new SecurityController();
            int? customerid = securitymgr.GetCurrentUserId(username);
            Employee info = new Employee();
            if (customerid.HasValue)
            {
                MessageUserControl.TryRun(() =>
                {
                    EmployeeController sysmgrUser = new EmployeeController();
                    info = sysmgrUser.Get_Employee(customerid.Value);
                });
            }
            MessageUserControl.TryRun(() =>
            {
                List<OrderItemPOCO> SelectionList = new List<OrderItemPOCO>();
                foreach (ListViewItem item in ListView2.Items)
                {
                    OrderItemPOCO oldItems = new OrderItemPOCO();
                    oldItems.OrderDetailID = item.DataItemIndex;
                    // try it out
                    oldItems.Product = (item.FindControl("ProductLabel") as Label).Text;
                    if (!int.TryParse((item.FindControl("OrderQtyLabel") as TextBox).Text, out int test))
                    {
                        throw new Exception(oldItems.Product + ": Quantity can only be an integer that is greater than 0");
                    }
                    else if (int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) <= 0)
                    {
                        throw new Exception(oldItems.Product + ": Quantity can only be an integer that is greater than 0");
                    }
                    else
                    {
                        oldItems.OrderQty = int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);
                    }
                    oldItems.UnitSize = int.Parse((item.FindControl("UnitSizeLabel") as Label).Text);
                    if (!decimal.TryParse((item.FindControl("UnitCostLabel") as TextBox).Text, out decimal tesst))
                    {
                        throw new Exception(oldItems.Product + ": Unit Cost can only be an integer that is greater than 0");
                    }
                    else if (decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text) <= 0)
                    {
                        throw new Exception(oldItems.Product + ": Unit Cost can only be an integer that is greater than 0");
                    }
                    else
                    {
                        oldItems.UnitCost = decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                    }
                    SelectionList.Add(oldItems);
                }

                SelectionList.Remove(SelectionList[itemToDelete]);
                ListView2.DataSource = SelectionList;
                ListView2.DataBind();
            });
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (vendorValidate.Text == "")
            {
                MessageUserControl.ShowInfo("Alert", "Please select a vendor first to continue");
            }
            else
            {
                var username = User.Identity.Name;
                SecurityController securitymgr = new SecurityController();
                int? customerid = securitymgr.GetCurrentUserId(username);
                Employee info = new Employee();
                if (customerid.HasValue)
                {
                    MessageUserControl.TryRun(() => {
                        EmployeeController sysmgrUser = new EmployeeController();
                        info = sysmgrUser.Get_Employee(customerid.Value);
                    });
                }


                VendorController VendorController = new VendorController();
                Vendor selectedVendor = VendorController.Vendor_Get(int.Parse(vendorValidate.Text));
                VendorInfo.Text = selectedVendor.Name + " - " + selectedVendor.Contact + " - " + selectedVendor.Phone;



                InventoryListController sysmgr = new InventoryListController();
                List<InventoryListDTO> datainfo = new List<InventoryListDTO>();
                List<InventoryListDTO> filteredDatainfo = new List<InventoryListDTO>();
                MessageUserControl.TryRun(() =>
                {
                    datainfo = sysmgr.GetVendorInventory(int.Parse(vendorValidate.Text));
                    List<int> empty = new List<int>();
                    for (int index = 0; index < 4; index++)
                    {
                        if (datainfo[index].ProductList.Count() > 0)
                        {
                            filteredDatainfo.Add(datainfo[index]);
                        }
                    }
                    if (filteredDatainfo.Count() == 0)
                    {
                        RepeaterInventory.DataSource = "";
                        RepeaterInventory.DataBind();
                        ListView2.DataSource = null;
                        ListView2.DataBind();
                        throw new Exception("The vendor you selected has no vendorcatalog");
                    }
                    RepeaterInventory.DataSource = filteredDatainfo;
                    RepeaterInventory.DataBind();

                    OrderController OrderContoller = new OrderController();
                    OrderLogDTO OrderLog = new OrderLogDTO();

                    OrderLog = OrderContoller.GetVendorOrderLog(int.Parse(vendorValidate.Text), info.EmployeeID);
                    ListView2.DataSource = OrderLog.ItemList;
                    ListView2.DataBind();

                    Subtotal.Text = OrderLog.SubTotal.ToString("0.00");
                    Tax.Text = OrderLog.TaxGST.ToString("0.00");
                    Total.Text = (OrderLog.SubTotal + OrderLog.TaxGST).ToString("0.00");
                    Comments.Text = OrderLog.Comment;
                    VendorNameDropDownList.Enabled = true;
                    Select.Enabled = true;
                }, "Cancel", "All changes are gone.");
            }
        }
        protected void Refresh_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {
                double subtotal = 0;
                foreach (var item in ListView2.Items)
                {
                    double UnitCost;
                    double Qty;
                    string product = (item.FindControl("ProductLabel") as Label).Text;
                    if (!decimal.TryParse((item.FindControl("UnitCostLabel") as TextBox).Text, out decimal tesst))
                    {
                        throw new Exception(product + ": Unit Cost can only be an integer that is greater than 0");
                    }
                    else if (decimal.Parse((item.FindControl("UnitCostLabel") as TextBox).Text) <= 0)
                    {
                        throw new Exception(product + ": Unit Cost can only be an integer that is greater than 0");
                    }
                    else
                    {
                        UnitCost = double.Parse((item.FindControl("UnitCostLabel") as TextBox).Text);
                    }

                    if (!int.TryParse((item.FindControl("OrderQtyLabel") as TextBox).Text, out int test))
                    {
                        throw new Exception(product + ": Quantity can only be an integer that is greater than 0");
                    }
                    else if (int.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text) <= 0)
                    {
                        throw new Exception(product + ": Quantity can only be an integer that is greater than 0");
                    }
                    else
                    {
                        Qty = double.Parse((item.FindControl("OrderQtyLabel") as TextBox).Text);
                    }
                    double UnitSize = double.Parse((item.FindControl("UnitSizeLabel") as Label).Text);
                    (item.FindControl("PerItemCostLabel") as Label).Text = (UnitCost / UnitSize).ToString("0.00");
                    (item.FindControl("ExtendedCostLabel") as Label).Text = (Qty * UnitCost).ToString("0.00");
                    subtotal += Qty * UnitCost;
                    Subtotal.Text = subtotal.ToString("0.00");
                    Tax.Text = (subtotal * 0.05).ToString("0.00");
                    Total.Text = (subtotal * 1.05).ToString("0.00");
                }
            });
        }
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (vendorValidate.Text == "")
            {
                MessageUserControl.ShowInfo("Alert", "Please select a vendor first to continue");
            }
            else
            {
                // remove order details
                MessageUserControl.TryRun(() => {
                    OrderController sysmgrUser = new OrderController();

                    sysmgrUser.RemoveOrder(int.Parse(vendorValidate.Text));
                    List<OrderItemPOCO> refreshitem = new List<OrderItemPOCO>();
                    List<ProductPOCO> refreshpoco = new List<ProductPOCO>();
                    ListView2.DataSource = refreshitem;
                    ListView2.DataBind();
                    RepeaterInventory.DataSource = refreshpoco;
                    RepeaterInventory.DataBind();
                    vendorValidate.Text = "";
                    Subtotal.Text = 0.ToString();
                    Tax.Text = 0.ToString();
                    Total.Text = 0.ToString();
                    VendorNameDropDownList.Enabled = true;
                    Select.Enabled = true;
                }, "Success", "Order deleted, please reselect the vendor if you want to continue with this vendor");
            }

            // remove order
        }
    }
}