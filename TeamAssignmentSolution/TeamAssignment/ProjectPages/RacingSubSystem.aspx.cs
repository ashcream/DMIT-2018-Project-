using eRaceSystem.BLL;
using eRaceSystem.BLL.RacingBLL;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs.RacingPOCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamAssignment.Security;

namespace TeamAssignment.ProjectPages
{
    public partial class RacingSubSystem : System.Web.UI.Page
    {
        private int? userid;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Race Coordinator") || User.IsInRole("Clerk"))
                {
                    var username = User.Identity.Name;
                    SecurityController securitymgr = new SecurityController();
                    userid = securitymgr.GetCurrentUserId(username);
                    if (userid.HasValue)
                    {
                        MessageUserControl.TryRun(() => {
                            EmployeeController sysmgr = new EmployeeController();
                            Employee info = sysmgr.Get_Employee(userid.Value);
                            UserName.Text = info.FirstName + " " + info.LastName;
                            if (!Page.IsPostBack)
                            {
                                RacingDatePicker.SelectedDate = DateTime.Today;
                            }
                            
                            
                            
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

        protected void RacingDatePicker_SelectionChanged(object sender, EventArgs e)
        {
            ScheduleGridView.DataBind();
            RaceID.Text = "0";
            RaceCertif.Text = "";
            RaceResultsODS.SelectParameters["raceid"].DefaultValue = RaceID.Text;
            RaceResultsODS.DataBind();
            RaceResultGridView.DataBind();

        }        

        protected void viewScheduleButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                Button btn = sender as Button;
                GridViewRow gvr = btn.NamingContainer as GridViewRow;
                for (int i = 0; i < ScheduleGridView.Rows.Count; i++)
                {
                    if (i == gvr.RowIndex)
                    {
                        (ScheduleGridView.Rows[i].FindControl("viewScheduleButton") as Button).Text = "Selected";
                    }
                    else
                    {
                        (ScheduleGridView.Rows[i].FindControl("viewScheduleButton") as Button).Text = "View";
                    }
                }
                
                
                //if (gvr.Cells[3].Text == "Y")
                //{
                //    //todo disable edit
                //    MessageUserControl.ShowInfo("Alert", "Race are finished, cannot be edited");
                //}                
                RaceID.Text = "0";
                RaceResultsODS.SelectParameters["raceid"].DefaultValue = RaceID.Text;
                RaceResultsODS.DataBind();
                RaceResultGridView.DataBind();
                RaceDetailODS.SelectParameters["raceid"].DefaultValue = ScheduleGridView.DataKeys[gvr.RowIndex].Value.ToString();
                RaceID.Text = ScheduleGridView.DataKeys[gvr.RowIndex].Value.ToString();
                RosterLabel.Text = ScheduleGridView.Rows[gvr.RowIndex].Cells[2].Text;
                RaceDetailODS.DataBind();
                RaceDetailListView.EditIndex = -1;
                RaceDetailListView.DataBind();   
                

                RaceController sysmgr = new RaceController();
                RaceCertif.Text = sysmgr.getCertificationLevel( int.Parse(ScheduleGridView.DataKeys[gvr.RowIndex].Value.ToString()));
                MemberODS.DataBind();
                

            }, "Success", "Showing the selected race roster");
            
        }
        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
            
        }

        protected void RecordRaceButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {
                
                    RaceResultsODS.SelectParameters["raceid"].DefaultValue = RaceID.Text;
                    RaceResultsODS.DataBind();
                    RaceResultGridView.DataBind();
                    if (RaceDetailListView.Items.Count !=0 && RaceResultGridView.Rows.Count == 0)
                    {
                        throw new Exception("All playes are refunded");
                    }
                    else if(RaceResultGridView.Rows.Count == 0)
                    {
                        throw new Exception("Please select a race first.");
                    }
                    
                
                
            },"Success","Enter race reults on the right table");
                    
        }

        protected void SaveTimeButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {
                List<RaceResultsPOCO> resultsList = new List<RaceResultsPOCO>();
                RaceResultsPOCO item;
                if (RaceResultGridView.Rows.Count==0)
                {
                    throw new Exception("Please select a race first.Press that Record Race Time button");
                }
                for (int i = 0; i < RaceResultGridView.Rows.Count; i++)
                {
                    item = new RaceResultsPOCO();
                    item.RaceDetailID = int.Parse(RaceResultGridView.DataKeys[i].Value.ToString());
                    string timeplaceholder = (RaceResultGridView.Rows[i].FindControl("RunTimeTextBox") as TextBox).Text;
                    if (string.IsNullOrEmpty(timeplaceholder))
                    {
                        item.RunTime = null;
                    }
                    else
                    {
                        TimeSpan placeholder;                       
                        while (!TimeSpan.TryParse(timeplaceholder, out placeholder))
                        {
                            throw new Exception("Please enter the time as hh:mm:ss. ex: 05:42:59");
                        }
                        item.RunTime = placeholder;
                    }
                    if (string.IsNullOrEmpty((RaceResultGridView.Rows[i].FindControl("PenaltyDropDown") as DropDownList).SelectedValue))
                    {
                        item.PenaltyID = null;
                    }
                    else
                    {
                        item.PenaltyID = int.Parse((RaceResultGridView.Rows[i].FindControl("PenaltyDropDown") as DropDownList).SelectedValue);
                    }                    
                    resultsList.Add(item);
                }

                RaceDetailController sysmgr = new RaceDetailController();
                sysmgr.RacePlacement(resultsList);

                RaceDetailODS.DataBind();
                RaceDetailListView.DataBind();
            },"Success","Race time and placement set");
            
        }

       

        protected void InsertButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {
                if (string.IsNullOrEmpty(RaceID.Text))
                {
                    throw new Exception("Select a race first");
                }
                RaceDetailController sysmgr = new RaceDetailController();

                RaceDetailPOCO newDetailPOCO = new RaceDetailPOCO();
                newDetailPOCO.Name = (RaceDetailListView.InsertItem.FindControl("MemberDropDown") as DropDownList).Text;
                newDetailPOCO.RaceFee = decimal.Parse((RaceDetailListView.InsertItem.FindControl("RaceFeeDropDown") as DropDownList).SelectedValue);
                if (string.IsNullOrEmpty((RaceDetailListView.InsertItem.FindControl("CarNumberDropDownI") as DropDownList).SelectedValue))
                {
                    newDetailPOCO.CarserialNumber = null;
                }
                else
                {
                    newDetailPOCO.CarserialNumber = int.Parse((RaceDetailListView.InsertItem.FindControl("CarNumberDropDownI") as DropDownList).SelectedValue);
                }
                //newDetailPOCO.CarserialNumber = string.IsNullOrEmpty((RaceDetailListView.InsertItem.FindControl("CarNumberDropDownI") as DropDownList).SelectedValue) ? null :
                //                                 int.Parse((RaceDetailListView.InsertItem.FindControl("CarNumberDropDownI") as DropDownList).SelectedValue);

                sysmgr.RaceDetail_Add(newDetailPOCO, int.Parse(RaceID.Text),userid.GetValueOrDefault());
                
                RaceDetailODS.DataBind();
                RaceDetailListView.DataBind();

                RaceResultsODS.SelectParameters["raceid"].DefaultValue = "0";
                RaceResultsODS.DataBind();
                RaceResultGridView.DataBind();
            }, "Success", "Member added to roster");
        }

       

        protected void CarNumberDropDownI_DataBound(object sender, EventArgs e)
        {
            DropDownList dpl = sender as DropDownList;
            //DropDownList insertCar = RaceDetailListView.InsertItem.FindControl("CarNumberDropDownI") as DropDownList;
            dpl.Items.Insert(0, new ListItem("..Select a car", ""));
        }

        protected void RaceDetailListView_ItemInserted(object sender, ListViewInsertedEventArgs e)
        {
            
        }

        protected void RaceDetailListView_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            RaceResultsODS.SelectParameters["raceid"].DefaultValue ="0";
            RaceResultsODS.DataBind();
            RaceResultGridView.DataBind();
        }

        












        //protected void ClaClassDropDown_SelectedIndexChanged(object sender,  EventArgs e)
        //{

        //        CarSerialNumberODS.SelectParameters["carClassName"].DefaultValue = (RaceDetailListView.EditItem.FindControl("CarNumberDropDown") as DropDownList).SelectedValue;
        //        CarSerialNumberODS.DataBind();


        //        //(RaceDetailListView.EditItem.FindControl("CarNumberDropDown") as DropDownList).DataBind();

        //}

        //protected void RaceDetailListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    if (Page.IsPostBack)
        //    {
        //        if (e.Item.ItemType == ListViewItemType.DataItem)
        //        {
        //            DropDownList dll = e.Item.FindControl("ClaClassDropDown") as DropDownList;
        //            dll.SelectedIndexChanged += new EventHandler(ClaClassDropDown_SelectedIndexChanged);
        //        }
        //    }
        //}
    }
}