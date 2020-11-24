<%@ Page Title="Receiving Subsystem by Chris" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceivingSubSystem.aspx.cs" Inherits="TeamAssignment.ProjectPages.ReceivingSubSystem" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" HeaderText="Correct the following concerns on the insert record."  ValidationGroup="IGroup">
    
    
    <div class="row jumbotron">
        <h1>Receiving</h1>
        <asp:Label ID="Label5" runat="server" Text="User: "></asp:Label>
        <asp:Label ID="UserName" runat="server" ></asp:Label>
    </div>
    
    
    <uc1:MessageUserControl runat="server" id="MessageUserControl" />
    <div class="row">
        <asp:DropDownList ID="POForReceivingDDL" runat="server" DataSourceID="POForReceivingODS" DataTextField="POForReceiving" AppendDataBoundItems="true" DataValueField="OrderID">
            <asp:ListItem Value="0" Text="Select..."></asp:ListItem>
        </asp:DropDownList>
        <asp:ObjectDataSource ID="POForReceivingODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_POForReceiving" TypeName="eRaceSystem.BLL.ReceivingBLL.OrdersController"></asp:ObjectDataSource>

        <asp:Button ID="OpenButton" runat="server" Text="Open" OnClick="OpenButton_Click" />
    </div>
    &nbsp;
    &nbsp;
    <div class="row">
        <div class="col-md-8">
            <div class="row">
                <div class="col-md-3">
                    <asp:Label ID="VendorName" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="VendorAddress" runat="server" Text=""></asp:Label>
                </div>                                
            </div>
            <div class="row">
                <div class="col-md-3">
                    <asp:Label ID="VendorContact" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="VendorPhone" runat="server" Text=""></asp:Label>
                </div>               
            </div>
        </div>
        <div class="col-md-4">
            <asp:Button ID="ReceiveButton" runat="server" Text="Receive Shipment" OnClick="ReceiveButton_Click" BackColor="lightblue" />
        </div>      
    </div>
    &nbsp;
    &nbsp;
    <div>
        <asp:GridView ID="ReceivingListGridView" runat="server" DataSourceID="ReceivingListODS" AllowPaging="True" PageSize="6" AllowSorting="True"  AutoGenerateColumns="False" DataKeyNames="OrderDetailID">
            <Columns>
                
                <%--<asp:BoundField DataField="OrderID" HeaderText="OrderID" SortExpression="OrderID"></asp:BoundField>--%>
                <asp:BoundField DataField="ItemName" HeaderText="Item" HeaderStyle-HorizontalAlign="Center" SortExpression="ItemName" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField DataField="QuantityOrdered" HeaderText="Quantity Ordered" HeaderStyle-HorizontalAlign="Center" SortExpression="QuantityOrdered" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:BoundField DataField="OrderedUnits" HeaderText="Ordered Units" HeaderStyle-HorizontalAlign="Center" SortExpression="OrderedUnits"></asp:BoundField>
                <asp:BoundField DataField="QuantityOutstanding" HeaderText="Quantity Outstanding" HeaderStyle-HorizontalAlign="Center" SortExpression="QuantityOutstanding" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                <asp:TemplateField HeaderText="Receiving Units" HeaderStyle-HorizontalAlign="Center" SortExpression="ReceivingUnits" ControlStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="ReceivingUnits" runat="server" Text='<%#Bind("ReceivingUnits")%>' width="30" ></asp:TextBox>                         
                        <%--<asp:BoundField DataField="ReceivingQuantity" HeaderText="ReceivingQuantity" SortExpression="ReceivingQuantity"></asp:BoundField>--%>                         
                        <asp:Label ID="Label2" runat="server" Text='<%#Bind("ReceivingUnitString")%>'></asp:Label>
                        
                        <%--<asp:BoundField DataField="ReceivingUnits" HeaderText="ReceivingUnits" SortExpression="ReceivingUnits"></asp:BoundField>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rejected Units / Reason" HeaderStyle-HorizontalAlign="Center" SortExpression="RejectedUnitsReason">
                    <ItemTemplate>
                        <%--<asp:BoundField DataField="RejectedQuantity" HeaderText="RejectedUnits" SortExpression="RejectedQuantity"></asp:BoundField>--%>
                        <asp:TextBox ID="RejectedUnits" runat="server" Text='<%#Bind("RejectedUnits")%>' width="40"></asp:TextBox>                   
                         <%--<asp:BoundField DataField="RejectedReason" HeaderText="RejectedReason" SortExpression="RejectedReason"></asp:BoundField>--%>
                        <asp:TextBox ID="RejectedReason" runat="server" Text='<%#Bind("RejectedReason")%>' width="140" ></asp:TextBox> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Salvaged Items" HeaderStyle-HorizontalAlign="Center" SortExpression="SalvagedItems">
                    <ItemTemplate>
                        <%--<asp:BoundField DataField="SalvagedItems" HeaderText="SalvagedItems" SortExpression="SalvagedItems"></asp:BoundField>--%>
                        <asp:TextBox ID="SalvagedItems" runat="server" Text='<%#Bind("SalvagedItems")%>' ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UnitSize" HeaderStyle-HorizontalAlign="Center" SortExpression="UnitSize" ControlStyle-Width="100" Visible="false">
                    <ItemTemplate>                                               
                        <asp:Label ID="UnitSize" runat="server" Text='<%#Bind("UnitSize")%>'></asp:Label>                        
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="CategoryID" HeaderStyle-HorizontalAlign="Center" SortExpression="CategoryID" ControlStyle-Width="100" Visible="false">
                    <ItemTemplate>                                               
                        <asp:Label ID="CategoryID" runat="server" Text='<%#Bind("CategoryID")%>'></asp:Label>                        
                    </ItemTemplate>
                </asp:TemplateField>--%>
                
                
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ReceivingListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_ReceivingListForPurchaseOrderSelection" TypeName="eRaceSystem.BLL.ReceivingBLL.ReceivingController">
            <SelectParameters>
                <asp:ControlParameter ControlID="POForReceivingDDL" PropertyName="SelectedValue" Name="orderid" Type="Int32"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
     &nbsp;
    &nbsp;
    <div class="row">
        <div class="col-md-8">
            <asp:Label ID="Label1" runat="server" Text="Unordered Items List"></asp:Label>
            <div>
                <asp:ValidationSummary ID="ValidationSummaryInsert" runat="server"
                HeaderText="Correct the following concerns on the insert record."
                ValidationGroup="IGroup" />
            </div>
            <asp:ListView ID="UnorderedItemListView" runat="server" DataSourceID="UnorderedItemsODS" InsertItemPosition="LastItem" DataKeyNames="ItemID">
                <AlternatingItemTemplate>
                    <tr style="background-color: #FFF8DC;">
                        <td>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" CausesValidation="false"/>
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorProductID") %>' runat="server" ID="VendorProductIDLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="UnorderedItemQuantityLabel" /></td>
                    </tr>
                </AlternatingItemTemplate>
                <EditItemTemplate>
                    <tr style="background-color: #008A8C; color: #FFFFFF;">
                        <td>
                            <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                            <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                        </td>
                        <td>
                            <asp:TextBox Text='<%# Bind("ItemName") %>' runat="server" ID="ItemName" /></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("VendorProductID") %>' runat="server" ID="VendorProductID" /></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="UnorderedItemQuantity" /></td>
                    </tr>
                </EditItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorItemName" runat="server" 
                         ErrorMessage="Item Name is required" 
                         ControlToValidate="ItemNameTextBox" ValidationGroup="IGroup"></asp:RequiredFieldValidator>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorVendorProductID" runat="server" 
                         ErrorMessage="Vendor product ID is required" 
                         ControlToValidate="VendorProductIDTextBox" ValidationGroup="IGroup"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredValidatorQuantity" runat="server" 
                         ErrorMessage="Unordered item quantity is required"  
                         ControlToValidate="UnorderedItemQuantityTextBox" ValidationGroup="IGroup"></asp:RequiredFieldValidator>

                    <tr style="">
                        <td>
                            <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" CausesValidation="true" ValidationGroup="IGroup"/>
                            <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" />
                        </td>
                        <td>
                            <asp:TextBox Text='<%# Bind("ItemName") %>' runat="server" ID="ItemNameTextBox" /></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("VendorProductID") %>'  runat="server" ID="VendorProductIDTextBox" /></td>
                        <td>
                            <asp:TextBox Text='<%# Bind("Quantity") %>'  runat="server" ID="UnorderedItemQuantityTextBox" /></td>
                    </tr>
                </InsertItemTemplate>
                <ItemTemplate>
                    <tr style="background-color: #DCDCDC; color: #000000;">
                        <td>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" CausesValidation="false"/>
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemName" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorProductID") %>' runat="server" ID="VendorProductID" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="UnorderedItemQuantity" /></td>
                    </tr>
                </ItemTemplate>
                <LayoutTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td runat="server">
                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                    <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                        <th runat="server"></th>
                                        <th runat="server">Item Name</th>
                                        <th runat="server">Vendor Product ID</th>
                                        <th runat="server">Quantity</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder"></tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server">
                            <td runat="server" style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;"></td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <SelectedItemTemplate>
                    <tr style="background-color: #008A8C; font-weight: bold; color: #FFFFFF;">
                        <td>
                            <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                        </td>
                        <td>
                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("VendorProductID") %>' runat="server" ID="VendorProductIDLabel" /></td>
                        <td>
                            <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="UnorderedItemQuantityLabel" /></td>
                    </tr>
                </SelectedItemTemplate>
            </asp:ListView>
            
            <asp:ObjectDataSource ID="UnorderedItemsODS" runat="server" DataObjectTypeName="eRaceSystem.Data.Entities.UnOrderedItem" 
                DeleteMethod="UnorderedItems_Delete" 
                InsertMethod="UnorderedItems_Add" 
                OldValuesParameterFormatString="original_{0}" 
                SelectMethod="UnorderedItemsList_Select" 
                TypeName="eRaceSystem.BLL.ReceivingBLL.ReceivingController">
                
            </asp:ObjectDataSource>
        </div>
        <div class="col-md-4s">
            <asp:Button ID="ForceCloseButton" runat="server" Text="Force Close" backcolor="green" OnClick="ForceCloseButton_Click" OnClientClick="return confirm(' This purchase order will be forced closed forever, are you sure you wish to close it?')"/>
            &nbsp;
            &nbsp;
            <asp:TextBox ID="Reason" runat="server" Width="1000" Height="50" ></asp:TextBox>
            
        </div>
    </div>  
</asp:Content>
