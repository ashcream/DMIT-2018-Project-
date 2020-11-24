    <%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RacingSubSystem.aspx.cs" Inherits="TeamAssignment.ProjectPages.RacingSubSystem" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Racing</h1>
        <asp:Label ID="Label1" runat="server" Text="Welcome: "></asp:Label><asp:Label ID="UserName" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="col-md-12">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            <asp:ValidationSummary ID="ValidationSummary" ValidationGroup="EditGroup" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-md-offset-1">
            <h2>Pick a Date</h2>
            <br />
            <asp:Calendar ID="RacingDatePicker" runat="server" OnSelectionChanged="RacingDatePicker_SelectionChanged"></asp:Calendar>
        </div>
        <div class="col-md-7">
            <h2>Schedule</h2>
            <br />
            <asp:Label ID="RaceCertif" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="RaceID" runat="server" Text="" Visible="false"></asp:Label>
            <asp:GridView ID="ScheduleGridView" runat="server" AutoGenerateColumns="False" DataSourceID="RaceScheduleODS" DataKeyNames="itemID">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="viewScheduleButton" runat="server"  OnClick="viewScheduleButton_Click"  Text="View" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ControlStyle-Width="50px" DataField="raceTime" HeaderText="raceTime" SortExpression="raceTime" DataFormatString="{0:HH tt}"></asp:BoundField>
                    <asp:BoundField ControlStyle-Width="250px" DataField="competition" HeaderText="competition" SortExpression="competition"></asp:BoundField>
                    <asp:BoundField ControlStyle-Width="50px" DataField="run" HeaderText="run" SortExpression="run"></asp:BoundField>
                    <asp:BoundField ControlStyle-Width="80px" DataField="playerCount" HeaderText="playerCount" SortExpression="playerCount"></asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label runat="server">No race on selected date</asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 col-md-offset-1">
            <h2 style="display: inline-block;">Roster</h2>
            <asp:Label runat="server" ID="RosterLabel"></asp:Label>
            <asp:LinkButton OnClick="RecordRaceButton_Click" runat="server" CssClass="btn btn-primary" ID="RecordRaceButton">
                    <span aria-hidden="true" class="glyphicon glyphicon-time"> Record Race Time</span>       
            </asp:LinkButton>
            <br />
            <asp:ListView ID="RaceDetailListView" runat="server" DataSourceID="RaceDetailODS"  InsertItemPosition="LastItem" DataKeyNames="RaceDetailID" OnItemInserted="RaceDetailListView_ItemInserted" OnItemUpdated="RaceDetailListView_ItemUpdated">
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFFFFF; color: #284775;">
                        <td>
                            <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                        </td>
                        <td>
                            <asp:Label Width="150px" Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" /></td>
                        <td>
                            <asp:Label Width="100px" Text='<%# Eval("RaceFee") %>' runat="server" ID="RaceFeeLabel" /></td>
                        <td>
                            <asp:Label Width="100px" Text='<%# Eval("RentalFee") %>' runat="server" ID="RentalFeeLabel" /></td>
                        <td>
                            <asp:Label Width="100px" Text='<%# Eval("Placement") %>' runat="server" ID="PlacementLabel" /></td>
                        <td>
                            <asp:CheckBox Width="100px" Checked='<%# Eval("Refunded") %>' runat="server" ID="RefundedCheckBox" Enabled="false" /></td>

                    </tr>
                </AlternatingItemTemplate>
                <EditItemTemplate>

                    <tr style="background-color: #E2DED6;">
                        <td>
                            <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                            <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                        </td>
                        <td colspan="2">
                            <asp:Label Width="120px" Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" Enabled="false" />
                            <asp:label Width="150px" Text='<%# Bind("RaceFee") %>' runat="server" ID="NameTextBoxI" />
                            <br />                            
                            <asp:TextBox Width="250px" Text='<%# Bind("Comment") %>' MaxLength="1048" runat="server" ID="CommentTextBox" placeholder="Comment"></asp:TextBox>
                        </td>



                        <td colspan="3">                            
                            <%--<asp:DropDownList Width="110px" runat="server" ID="ClaClassDropDown" SelectedValue='<%# Bind("CarClass") %>' DataValueField="DisplayText" AppendDataBoundItems="true" DataSourceID="CarClassODS" DataTextField="DisplayText">
                                <asp:ListItem Text="...Select a Car Class" Value="0" ></asp:ListItem>
                            </asp:DropDownList>--%>

                            <asp:DropDownList Width="150px" runat="server" ID="CarNumberDropDown" SelectedValue='<%# Bind("CarserialNumber") %>' AppendDataBoundItems="true" DataSourceID="CarSerialNumberODS" DataTextField="DisplayText" DataValueField="IDValueField">
                                <asp:ListItem Text="...Select a Car" Value="" ></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <%--<asp:Label Width="190px" Text='<%# Bind("RefundReason") %>' runat="server" ID="RefundReasonLabel" Enabled="false" />--%>
                            <asp:TextBox Width="190px" runat="server" Text='<%# Bind("RefundReason") %>' MaxLength="150" ID="ReasonBox" placeholder="Reason" ></asp:TextBox>
                            <asp:CheckBox Width="90px" Checked='<%# Bind("Refunded") %>' runat="server" ID="CheckBox1" Text="Refund"  /></td>

                    </tr>
                    <asp:ObjectDataSource  OnSelected="CheckForException" ID="CarClassODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetCarClassByCertification" TypeName="eRaceSystem.BLL.RacingBLL.CarClassController">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="RaceCertif" PropertyName="Text" Name="certificationLevel" Type="String"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <asp:ObjectDataSource  OnSelected="CheckForException" ID="CarSerialNumberODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="avaliableCarList" TypeName="eRaceSystem.BLL.RacingBLL.CarController">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="RaceCertif" PropertyName="Text" Name="level" Type="String"></asp:ControlParameter>            

                            <asp:ControlParameter ControlID="RaceID" PropertyName="Text" Name="raceid" Type="Int32"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <tr style="">
                        <td>
                            <asp:Button runat="server" OnClick="InsertButton_Click" Text="Insert" ID="InsertButton" />
                            <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" />
                        </td>
                        <td>
                            <%--<asp:TextBox Width="150px" Text='<%# Bind("Name") %>' runat="server" ID="NameTextBoxI" />--%>
                            <asp:DropDownList ID="MemberDropDown" runat="server" DataSourceID="MemberODS"  DataTextField="DisplayText" DataValueField="DisplayText">
                                <asp:ListItem Text="..Select" Value=""></asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                        <td>
                            <%--<asp:Label Width="100px" Text='<%# Bind("RaceFee") %>' runat="server" ID="RaceFeeLabel" />--%>
                            <%--<asp:TextBox Width="150px" Text='<%# Bind("RaceFee") %>' runat="server" ID="RaceFeeTextBoxI" />--%>
                            <asp:DropDownList ID="RaceFeeDropDown" runat="server" DataSourceID="RaceFeeODS" DataTextField="DisplayText" DataValueField="DisplayText">
                                <asp:ListItem Text="..Select" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <%--<td>
                            <asp:DropDownList Width="100px" runat="server" ID="ClaClassDropDownI"  DataValueField="DisplayText" DataSourceID="CarClassODS" DataTextField="DisplayText">
                                <asp:ListItem Text="...Select a CarClass" Value="0" ></asp:ListItem>
                            </asp:DropDownList></td>--%>
                        <td colspan="3">
                            <asp:DropDownList OnDataBound="CarNumberDropDownI_DataBound" Width="250px" runat="server" ID="CarNumberDropDownI"   DataSourceID="CarSerialNumberODS" DataTextField="DisplayText" DataValueField="IDValueField">
                                <asp:ListItem Text="...Select a Car" Value="" ></asp:ListItem>
                            </asp:DropDownList></td>

                    </tr>
                        <asp:ObjectDataSource  OnSelected="CheckForException" ID="CarClassODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetCarClassByCertification" TypeName="eRaceSystem.BLL.RacingBLL.CarClassController">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="RaceCertif" PropertyName="Text" Name="certificationLevel" Type="String"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:ObjectDataSource  OnSelected="CheckForException" ID="CarSerialNumberODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="avaliableCarList" TypeName="eRaceSystem.BLL.RacingBLL.CarController">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="RaceCertif" PropertyName="Text" Name="level" Type="String"></asp:ControlParameter>            

                                <asp:ControlParameter ControlID="RaceID" PropertyName="Text" Name="raceid" Type="Int32"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr style="background-color: #E0FFFF; color: #333333;">
                        <td>
                            <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                        </td>
                        <td>
                            <asp:Label Width="150px" Text='<%# Eval("Name") %>' runat="server" ID="NameLabel" /></td>
                        <td>
                            <asp:Label Width="100px" Text='<%# Eval("RaceFee") %>' runat="server" ID="RaceFeeLabel" /></td>
                        <td>
                            <asp:Label Width="100px" Text='<%# Eval("RentalFee") %>' runat="server" ID="RentalFeeLabel" /></td>
                        <td>
                            <asp:Label Width="100px" Text='<%# Eval("Placement") %>' runat="server" ID="PlacementLabel" /></td>
                        <td>
                            <asp:CheckBox Width="100px" Checked='<%# Eval("Refunded") %>' runat="server" ID="RefundedCheckBox" Enabled="false" /></td>

                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                        <th runat="server"></th>
                                        <th style="width: 100px;" runat="server">Name</th>
                                        <th style="width: 100px;" runat="server">RaceFee</th>
                                        <th style="width: 100px;" runat="server">RentalFee</th>
                                        <th runat="server">Placement</th>
                                        <th runat="server">Refunded</th>

                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF"></td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <SelectedItemTemplate>
                    <tr style="background-color: #E2DED6; font-weight: bold; color: #333333;">
                        <td>
                            <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                        </td>
                        <td>
                            <asp:TextBox Width="150px" Text='<%# Bind("Name") %>' runat="server" ID="NameTextBox" /></td>
                        <td>
                            <asp:TextBox Width="100px" Text='<%# Bind("RaceFee") %>' runat="server" ID="RaceFeeTextBox" /></td>
                        <td>
                            <asp:TextBox Width="100px" Text='<%# Bind("RentalFee") %>' runat="server" ID="RentalFeeTextBox" /></td>
                        <td>
                            <asp:TextBox Width="100px" Text='<%# Bind("Placement") %>' runat="server" ID="PlacementTextBox" /></td>
                        <td>
                            <asp:CheckBox Width="100px" Checked='<%# Bind("Refunded") %>' runat="server" ID="RefundedCheckBox" /></td>


                    </tr>
                </SelectedItemTemplate>
                <EmptyDataTemplate>
                     <table class="emptyTable" cellpadding="5" cellspacing="5">
                <tr>                  
                  <td>
                    Select Race First.
                  </td>
                </tr>
              </table>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
        <div class="col-md-4 col-md-offset-1">
            <h2 style="display: inline-block;" class="mb-0">Race Results</h2>
            <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="SaveTimeButton" OnClick="SaveTimeButton_Click">                   
                <span aria-hidden="true" class="glyphicon glyphicon-time"> Save Times</span>       
            </asp:LinkButton>
            <br />
            <asp:GridView ID="RaceResultGridView"  DataKeyNames="RaceDetailID" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" DataSourceID="RaceResultsODS">

                <%-- <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                    <asp:BoundField DataField="PenaltyID" HeaderText="PenaltyID" SortExpression="PenaltyID"></asp:BoundField>
                    <asp:BoundField DataField="RunTime" HeaderText="RunTime" SortExpression="RunTime"></asp:BoundField>
                    <asp:TemplateField HeaderText="PenaltyID" SortExpression="PenaltyID">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="Penalty"  Text='<%# Bind("PenaltyID") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RunTime" SortExpression="RunTime">
                        <ItemTemplate>
                            <asp:TextBox  runat="server" ID="RunTimeTextBox" TextMode="Time" Text='<%# Bind("RunTime") %>' ></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>--%>
                <Columns>                    
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                   <%-- <asp:BoundField DataField="PenaltyID" HeaderText="PenaltyID" SortExpression="PenaltyID"></asp:BoundField>--%>
                    <asp:TemplateField HeaderText="PenaltyID" SortExpression="PenaltyID">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="PenaltyDropDown" AppendDataBoundItems="true" SelectedValue='<%# Bind("PenaltyID") %>' DataSourceID="PenaltyODS" DataTextField="DisplayText" DataValueField="IDValueField">
                                <asp:ListItem Text="NONE" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RunTime" SortExpression="RunTime">
                        <ItemTemplate>
                            <asp:TextBox  runat="server" ID="RunTimeTextBox"  Text='<%# Bind("RunTime") %>' ></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <asp:Label runat="server">Select a race to record results</asp:Label>
                </EmptyDataTemplate>
                <FooterStyle BackColor="#CCCCCC"></FooterStyle>

                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White"></HeaderStyle>

                <PagerStyle HorizontalAlign="Left" BackColor="#CCCCCC" ForeColor="Black"></PagerStyle>

                <RowStyle BackColor="White"></RowStyle>

                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

                <SortedAscendingCellStyle BackColor="#F1F1F1"></SortedAscendingCellStyle>

                <SortedAscendingHeaderStyle BackColor="#808080"></SortedAscendingHeaderStyle>

                <SortedDescendingCellStyle BackColor="#CAC9C9"></SortedDescendingCellStyle>

                <SortedDescendingHeaderStyle BackColor="#383838"></SortedDescendingHeaderStyle>
            </asp:GridView>
        </div>
    </div>


    <asp:ObjectDataSource ID="RaceScheduleODS" runat="server" OldValuesParameterFormatString="original_{0}"  OnSelected="CheckForException" SelectMethod="raceSchduleList" TypeName="eRaceSystem.BLL.RacingBLL.RaceController">
        <SelectParameters>
            <asp:ControlParameter ControlID="RacingDatePicker" PropertyName="SelectedDate" Name="raceTime" Type="DateTime"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="RaceDetailODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="RaceDetailList" TypeName="eRaceSystem.BLL.RacingBLL.RaceDetailController" 
        InsertMethod="RaceDetail_Add" 
        UpdateMethod="RaceDetail_Update" 
        DataObjectTypeName="eRaceSystem.Data.POCOs.RacingPOCO.RaceDetailPOCO"
        OnInserted="CheckForException"
        OnSelected="CheckForException"
        OnUpdated="CheckForException"
        >

        <InsertParameters>
            <asp:Parameter Name="detail" Type="Object"></asp:Parameter>
            <asp:Parameter Name="raceid" Type="Int32"></asp:Parameter>
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="RaceID" PropertyName="Text" DefaultValue="0" Name="raceid" Type="Int32"></asp:ControlParameter>

        </SelectParameters>

    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="MemberODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetMembersByLevel"  OnSelected="CheckForException" TypeName="eRaceSystem.BLL.RacingBLL.MemberController">
        <SelectParameters>
            <asp:ControlParameter ControlID="RaceCertif" PropertyName="Text" Name="level" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <%--<asp:ObjectDataSource ID="RaceResultsODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ReceResults" TypeName="eRaceSystem.BLL.RacingBLL.RaceDetailController">
        <SelectParameters>
            <asp:Parameter Name="raceid" Type="Int32" DefaultValue="0"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>--%>
    <asp:ObjectDataSource ID="RaceResultsODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ReceResults"  OnSelected="CheckForException" TypeName="eRaceSystem.BLL.RacingBLL.RaceDetailController">
        <SelectParameters>
            <asp:Parameter Name="raceid" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="RaceFeeODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetRaceFee"  OnSelected="CheckForException" TypeName="eRaceSystem.BLL.RacingBLL.RaceFeeController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PenaltyODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="PenaltySelection"  OnSelected="CheckForException" TypeName="eRaceSystem.BLL.RacingBLL.PenaltyController"></asp:ObjectDataSource>
</asp:Content>
