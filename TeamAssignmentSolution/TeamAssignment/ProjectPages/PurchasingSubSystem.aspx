<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchasingSubSystem.aspx.cs" Inherits="TeamAssignment.ProjectPages.PurchasingSubSystem" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h1>Purchasing</h1>
        <asp:Label ID="UserName" runat="server"></asp:Label>
        <br />
    </div>
    <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-12">
                <div class="col-md-8">
                    <div class="col-md-1">
                        <asp:Label ID="Label1" runat="server" Text="Vendor:"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Vendor_List" TypeName="eRaceSystem.BLL.VendorController"></asp:ObjectDataSource>
                        <asp:DropDownList ID="VendorNameDropDownList" runat="server" DataSourceID="ObjectDataSource2" DataTextField="Name" DataValueField="VendorID" Height="23px"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="Select" runat="server" Text="Select" OnClick="Select_Click" />
                    </div>
                    <div class="col-md-6">
                        <asp:Button ID="PlaceOrder" runat="server" Text="Place Order" OnClick="PlaceOrder_Click" OnClientClick="return confirm('Are you sure you wish to place the order?')" />
                        <asp:Button ID="Save" runat="server" Text="Save" OnClick="Save_Click" />
                        <asp:Button ID="Cancel" runat="server" Text="Cancel" OnClick="Cancel_Click" OnClientClick="return confirm(' You will lose any data that is not saved, are you sure you wish to cancel?')" />
                        <asp:Button ID="Delete" runat="server" Text="Remove" OnClick="Delete_Click" OnClientClick="return confirm(' You will lose all data, are you sure you wish to REMOVE THE ORDER?')" />
                    </div>
                    <br />
                    <br />
                    <div class="col-md-7">
                        <asp:Label ID="VendorInfo" runat="server" Text="Vendor Name - Contact - Phone"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="SubtotalText" runat="server" Text="Subtotal"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="Subtotal" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="Comments" runat="server" Height="57px" Width="414px"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="TaxText" runat="server" Text="Tax"></asp:Label>
                        <asp:Label ID="TotalText" runat="server" Text="Total"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="Tax" runat="server" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="Total" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-md-8">
                        <br />
                        <br />
                    </div>
                    <div class="col-md-12">
                        <asp:ListView ID="ListView2" runat="server" AutoPostBack="true">
                            <AlternatingItemTemplate>
                                <tr style="background-color: #FFFFFF; color: #284775;">
                                    <td>

                                        <asp:LinkButton ID="RemovefromList" runat="server" OnCommand="Delete_Item"
                                            CssClass="btn" CommandArgument='<%# Eval("OrderDetailID") %>'>
                                            <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;</span>
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label Text='<%# Eval("Product") %>' runat="server" ID="ProductLabel" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("OrderQty") %>' runat="server" ID="OrderQtyLabel" Width="50px"></asp:TextBox></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("UnitSize") %>' runat="server" ID="UnitSizeLabel" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# string.Format("{0:0.00}", Eval("UnitCost")) %>' runat="server" ID="UnitCostLabel" Width="100px"></asp:TextBox></td>
                                    <td>
                                        <asp:LinkButton ID="Refresh" runat="server"
                                            CssClass="btn" OnClick="Refresh_Click"> 
                                            <span aria-hidden="true" class="glyphicon glyphicon-refresh">&nbsp;</span>
                                        </asp:LinkButton>
                                        <asp:Label runat="server" ID="PerItemCostLabel" /></td>
                                    <td>
                                        <asp:Label runat="server" ID="ExtendedCostLabel" /></td>
                                </tr>
                            </AlternatingItemTemplate>
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td>No prodct selected yet, please select an item from the Inventory.</td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #E0FFFF; color: #333333;">
                                    <td>
                                        <asp:LinkButton ID="RemovefromList" runat="server" OnCommand="Delete_Item"
                                            CssClass="btn" CommandArgument='<%# Eval("OrderDetailID") %>'>
                                            <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;</span>
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label Text='<%# Eval("Product") %>' runat="server" ID="ProductLabel" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("OrderQty") %>' runat="server" ID="OrderQtyLabel" Width="50px"></asp:TextBox></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("UnitSize")%>' runat="server" ID="UnitSizeLabel" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# string.Format("{0:0.00}", Eval("UnitCost")) %>' runat="server" ID="UnitCostLabel" Width="100px"></asp:TextBox></td>
                                    <td>
                                        <asp:LinkButton ID="Refresh" runat="server"
                                            CssClass="btn" OnClick="Refresh_Click"> 
                                            <span aria-hidden="true" class="glyphicon glyphicon-refresh">&nbsp;</span>
                                        </asp:LinkButton>
                                        <asp:Label runat="server" ID="PerItemCostLabel" /></td>
                                    <td>
                                        <asp:Label runat="server" ID="ExtendedCostLabel" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                    <th runat="server"></th>
                                                    <th runat="server">Product</th>
                                                    <th runat="server">Order Qty</th>
                                                    <th runat="server">Unit Size</th>
                                                    <th runat="server">Unit Cost</th>
                                                    <th runat="server">Per-Item Cost</th>
                                                    <th runat="server">Extended Cost</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>

                        </asp:ListView>
                    </div>
                </div>
                <div class="col-md-4">
                    <asp:Repeater ID="RepeaterInventory" runat="server" ItemType="eRaceSystem.Data.DTOs.PurchasingDTO.InventoryListDTO">
                        <HeaderTemplate>
                            <h3>Inventory</h3>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <h5><strong><%# Item.Description %></strong></h5>
                            <asp:ListView ID="ListView1" DataSource="<%# Item.ProductList %>" runat="server" OnItemCommand="TracksSelectionList_ItemCommand">
                                <AlternatingItemTemplate>
                                    <tr style="background-color: #FFFFFF; color: #284775;">
                                        <td>
                                            <asp:LinkButton ID="AddtoPlaylist" runat="server"
                                                CssClass="btn" CommandArgument='<%# Eval("ProductID") %>'>
                                            <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("ReOrderLevel") %>' runat="server" ID="ReOrderLevelLabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" ID="QuantityOnHandLabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("QuantityOnOrder") %>' runat="server" ID="QuantityOnOrderlabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("Size") %>' runat="server" ID="SizeLabel" /></td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <EmptyDataTemplate>
                                    <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                        <tr>
                                            <td>No data was returned.</td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: #E0FFFF; color: #333333;">
                                        <td>
                                            <asp:LinkButton ID="AddtoPlaylist" runat="server"
                                                CssClass="btn" CommandArgument='<%# Eval("ProductID") %>'>
                            <span aria-hidden="true" class="glyphicon glyphicon-plus">&nbsp;</span>
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label Text='<%# Eval("ItemName") %>' runat="server" ID="ItemNameLabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("ReOrderLevel") %>' runat="server" ID="ReOrderLevelLabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("QuantityOnHand") %>' runat="server" ID="QuantityOnHandLabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("QuantityOnOrder") %>' runat="server" ID="QuantityOnOrderlabel" /></td>
                                        <td>
                                            <asp:Label Text='<%# Eval("Size") %>' runat="server" ID="SizeLabel" /></td>

                                    </tr>
                                </ItemTemplate>
                                <LayoutTemplate>
                                    <table runat="server">
                                        <tr runat="server">
                                            <td runat="server">
                                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                    <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                        <th runat="server"></th>
                                                        <th runat="server">Product</th>
                                                        <th runat="server">ReOrder</th>
                                                        <th runat="server">In Stock</th>
                                                        <th runat="server">On Order</th>
                                                        <th runat="server">Size</th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder"></tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                            </asp:ListView>
                        </ItemTemplate>
                        <FooterTemplate>
                            &copy; DMIT2028 NAIT Course all rights reserved
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <asp:Label Text="" runat="server" ID="vendorValidate" Visible="false" />
        </div>
    </div>
</asp:Content>
