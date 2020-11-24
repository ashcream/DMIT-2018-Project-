
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamAssignment.Security;

#region Additional Namespace
using eRaceSystem.BLL;
using eRaceSystem.BLL.ReceivingBLL;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.DTOs.ReceivingDTO;
using eRaceSystem.Data.POCOs.ReceivingPOCO;
using eRaceSystem.BLL.EntitiesBLL;

#endregion

namespace TeamAssignment.ProjectPages
{
    public partial class ReceivingSubSystem : System.Web.UI.Page
    {
        private Vendor orderVendor = null;
        List<string> reasons = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Food Service") || User.IsInRole("Clerk"))
                {
                    var username = User.Identity.Name;
                    SecurityController securitymgr = new SecurityController();
                    int? customerid = securitymgr.GetCurrentUserId(username);
                    if (customerid.HasValue)
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            EmployeeController sysmgr = new EmployeeController();
                            Employee info = sysmgr.Get_Employee(customerid.Value);
                            UserName.Text = info.FirstName + " " + info.LastName;
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

        protected void OpenButton_Click(object sender, EventArgs e)
        {
            
            
            if (POForReceivingDDL.SelectedValue == "0")
            {
                VendorName.Text = "";
                VendorAddress.Text = "";
                VendorContact.Text = "";
                VendorPhone.Text = "";
                MessageUserControl.ShowInfo("Warning", "You need to select a purchase order.");
            }
            else
            {
                VendorsController sysmgr = new VendorsController();
                orderVendor = sysmgr.GetVendorInfo_ByOrderID(int.Parse(POForReceivingDDL.SelectedValue));
                VendorName.Text = orderVendor.Name;
                VendorAddress.Text = orderVendor.Address + "" + orderVendor.City;
                VendorContact.Text = orderVendor.Contact;
                VendorPhone.Text = orderVendor.Phone;

                UnorderedItemListView.Visible = true;
            }
        }

        protected void ReceiveButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                ReceivingDTO templateDTO = new ReceivingDTO();
                templateDTO.ReceivingLists = new List<ReceivingList>();
                templateDTO.UnorderedItemLists = new List<UnorderedItemsList>();
                //UnorderedItemsList oneUnorderedItem = new UnorderedItemsList();
                ReceivingList oneItem = new ReceivingList();

                if (POForReceivingDDL.SelectedValue == "0")
                {
                    reasons.Add("Purchase order number should be selected");
                    //no purchase order number selected
                }
                else
                {
                    int UnitSize = 0;
                    int OrderDetailID = 0;
                    int ReceivingUnits = 0;
                    int OutstandingQuantity = 0;
                    int RejectedUnits = 0;
                    int SalvagedItems = 0;
                    int rowCount = 0;

                    string ItemName = null;
                    string RejectedReason = null;

                    for (int i = 0; i < ReceivingListGridView.Rows.Count; i++)
                    {
                        ItemName = ReceivingListGridView.Rows[i].Cells[0].Text;
                        //CategoryID = int.Parse((ReceivingListGridView.Rows[i].Cells[8].FindControl("CategoryID") as Label).Text);
                        //special index out of range
                        OrderDetailID = int.Parse(ReceivingListGridView.DataKeys[i].Value.ToString());
                        UnitSize = int.Parse((ReceivingListGridView.Rows[i].Cells[7].FindControl("UnitSize") as Label).Text);
                        ReceivingUnits = int.Parse((ReceivingListGridView.Rows[i].Cells[4].FindControl("ReceivingUnits") as TextBox).Text);
                        OutstandingQuantity = int.Parse((ReceivingListGridView.Rows[i].Cells[3].Text));

                        RejectedUnits = int.Parse((ReceivingListGridView.Rows[i].Cells[5].FindControl("RejectedUnits") as TextBox).Text);
                        RejectedReason = (ReceivingListGridView.Rows[i].Cells[5].FindControl("RejectedReason") as TextBox).Text;
                        SalvagedItems = int.Parse(((TextBox)ReceivingListGridView.Rows[i].Cells[6].FindControl("SalvagedItems")).Text);

                        //ReceivingDTO templateDTO = new ReceivingDTO();
                        //templateDTO.ReceivingLists = new List<ReceivingList>();
                        //ReceivingList oneItem = new ReceivingList();
                        //templateDTO.UnorderedItemLists = new List<UnorderedItemsList>();
                        //ReceivingList is POCO

                        if (ReceivingUnits < 0 || RejectedUnits < 0)
                        {
                            ShowList();
                            reasons.Add(string.Format("{0}'s Receiving Units and Rejected Units should be greater or equal to 0.", ItemName));
                            //only positive values are acceptable for quantities received and returned.
                        }
                        if (ReceivingUnits * UnitSize - OutstandingQuantity > UnitSize)
                        {

                            ShowList();
                            reasons.Add(string.Format("The overage received for {0} does not exceed the order unit size on the original order", ItemName));
                            //The overage does not exceed the order unit size on the original order.
                            //if the outstanding is 50, and unit size is 10, no more than 6.
                        }

                        if (ReceivingUnits * UnitSize - OutstandingQuantity > 0)
                        {
                            OutstandingQuantity = 0;

                        }
                        if (RejectedUnits != 0 && string.IsNullOrEmpty(RejectedReason))
                        {
                            ShowList();
                            reasons.Add(string.Format("A reason need to be filled in for rejecting {0}.", ItemName));
                        }
                        if (RejectedUnits == 0 && SalvagedItems != 0)
                        {
                            ShowList();
                            reasons.Add(string.Format("{0}'s rejected unit need to be filled, in order to fill the salvaged items.", ItemName));
                        }

                        else
                        {
                            rowCount++;
                            templateDTO.OrderID = int.Parse(POForReceivingDDL.SelectedValue);
                            templateDTO.EmployeeID = 20;
                            oneItem.OrderDetailID = OrderDetailID;
                            oneItem.ItemName = ItemName;
                            oneItem.ReceivingUnits = ReceivingUnits;
                            oneItem.RejectedUnits = RejectedUnits;
                            oneItem.RejectedReason = RejectedReason;
                            oneItem.SalvagedItems = SalvagedItems;
                            oneItem.QuantityOutstanding = OutstandingQuantity;
                            templateDTO.ReceivingLists.Add(oneItem);
                            oneItem = new ReceivingList();




                            (ReceivingListGridView.Rows[i].Cells[4].FindControl("ReceivingUnits") as TextBox).Text = "0";
                            (ReceivingListGridView.Rows[i].Cells[5].FindControl("RejectedUnits") as TextBox).Text = "0";
                            (ReceivingListGridView.Rows[i].Cells[5].FindControl("RejectedReason") as TextBox).Text = "";
                            ((TextBox)ReceivingListGridView.Rows[i].Cells[6].FindControl("SalvagedItems")).Text = "0";

                           

                        }

                    }

                }
                
                foreach (var item in UnorderedItemListView.Items)
                {
                    //ReceivingDTO templateDTO = new ReceivingDTO();
                    //templateDTO.ReceivingLists = new List<ReceivingList>();
                    //templateDTO.UnorderedItemLists = new List<UnorderedItemsList>();
                    UnorderedItemsList oneUnorderedItem = new UnorderedItemsList();
                    oneUnorderedItem.ItemID = item.DataItemIndex;
                    oneUnorderedItem.ItemName = (item.FindControl("ItemName") as Label).Text;
                    oneUnorderedItem.VendorProductID = (item.FindControl("VendorProductID") as Label).Text;
                    oneUnorderedItem.UnorderedItemQuantity = int.Parse((item.FindControl("UnorderedItemQuantity") as Label).Text);

                    templateDTO.UnorderedItemLists.Add(oneUnorderedItem);
                    oneUnorderedItem = new UnorderedItemsList();

                }
                


                if (reasons.Count != 0)
                {
                    throw new BusinessRuleException("Business Rule Errors:", reasons);
                }
                else
                {

                    ReceivingController sysmgr = new ReceivingController();
                    sysmgr.ReceivingOrder_Transaction(templateDTO); //pass the DTO data for processing

                    UnorderedItemListView.DataSource = null;
                    UnorderedItemListView.Items.Clear();
                    POForReceivingDDL.SelectedValue = "0";
                    VendorName.Text = "";
                    VendorAddress.Text = "";
                    VendorContact.Text = "";
                    VendorPhone.Text = "";
                }


            }, "Purchase order received", "Purchase order has been received");



        }

        protected void ForceCloseButton_Click(object sender, EventArgs e)
        {
            string reason = Reason.Text;
            if (reason == "")
            {
                MessageUserControl.ShowInfo("Business Rule", "Please provide a reason to force close this order!");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    ReceivingDTO templateDTO = new ReceivingDTO();
                    templateDTO.ReceivingLists = new List<ReceivingList>();
                    ReceivingList oneItem = new ReceivingList();


                    templateDTO.Reason = Reason.Text;
                    int orderid = int.Parse(POForReceivingDDL.SelectedValue);
                    ReceivingController sysmgr = new ReceivingController();
                    sysmgr.ForceClose(templateDTO,orderid);


                    UnorderedItemListView.Visible = false;
                    POForReceivingDDL.SelectedValue = "0";
                    
                    

                    ReceivingListGridView.Visible = false;
                }, "Success", "This purchase order has been closed.");
            }
            
            POForReceivingDDL.DataBind();

        }

        private void ShowList()
        {
            VendorsController sysmgr = new VendorsController();
            orderVendor = sysmgr.GetVendorInfo_ByOrderID(int.Parse(POForReceivingDDL.SelectedValue));
            VendorName.Text = orderVendor.Name;
            VendorAddress.Text = orderVendor.Address + "" + orderVendor.City;
            VendorContact.Text = orderVendor.Contact;
            VendorPhone.Text = orderVendor.Phone;
            UnorderedItemListView.Visible = true;
            ReceivingListGridView.Visible = true;
        }
    }

    
}





