<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SaleSubSystem.aspx.cs" Inherits="TeamAssignment.ProjectPages.SaleSubSystem" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="UserName" runat="server" Font-Size="35px"></asp:Label>
    <uc1:MessageUserControl runat="server" id="MessageUserControl" />

     <div class="row">
         <h1>In store purchase</h1>
         <div class="col-sm-10">
             
             <asp:DropDownList ID="SelectCategory" runat="server" DataSourceID="SelectCategoryODS" DataTextField="Description" DataValueField="CategoryID" Width="150px" AutoPostBack="True" AppendDataBoundItems="true">
                 <asp:ListItem Value="0" Text="Select a Category..."></asp:ListItem>
             </asp:DropDownList>
       
             <asp:DropDownList ID="SelectProductFromCategory" runat="server" Width="200px" DataSourceID="SelectProductODS" DataTextField="ItemName" DataValueField="ProductID">
                 <asp:ListItem Value="0" Text="Select a Product..."></asp:ListItem>
             </asp:DropDownList>
         
            <asp:TextBox ID="Quantity" runat="server" Text="1"></asp:TextBox>
                    
       
        
                <asp:Button ID="AddNewSalesItem" runat="server" 
                Text="+Add" OnClick="AddSale_OnClick"/>
              
             </div>
     </div>
        <br /><br />

    <div class="row">
    
        <asp:ListView ID="ListItem" runat="server">
            <AlternatingItemTemplate>
                <tr style="background-color: #FFFFFF; color: #284775;">
                    <td>
                        <asp:Label Text='<%# Eval("Product") %>' runat="server" ID="ItemNameLabel"></asp:Label></td>
                    <td>
                         <asp:TextBox Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel"></asp:TextBox></td>
                    <td>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("Price")) %>' runat="server" ID="ItemPriceLabel"></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="Refresh" runat="server"
                             CssClass="btn" onclick="Refresh_Quantity">
                            <span aria-hidden="true" class="glyphicon glyphicon-refresh" >&nbsp;</span>
                        </asp:LinkButton>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("TotalAmount")) %>' runat="server" ID="TotalAmountLabel"></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="DeleteItem" runat="server"
                             CssClass="btn" OnCommand="Delete_SingleItem" CommandArgument='<%# Eval("Product") %>' OnClientClick="return confirm(' Are you sure you wish to delete this item?')">
                            <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;</span>
                        </asp:LinkButton>
                    </td>
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
                        <asp:Label Text='<%# Eval("Product") %>' runat="server" ID="ItemNameLabel"></asp:Label></td>
                    <td>
                         <asp:TextBox Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel"></asp:TextBox></td>
                      <td>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("Price")) %>' runat="server" ID="ItemPriceLabel"></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="Refresh" runat="server"
                             CssClass="btn" onclick="Refresh_Quantity">
                            <span aria-hidden="true" class="glyphicon glyphicon-refresh" >&nbsp;</span>
                        </asp:LinkButton>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("TotalAmount")) %>' runat="server" ID="TotalAmountLabel"></asp:Label></td>
                    <td>
                      <asp:LinkButton ID="DeleteItem" runat="server"
                             CssClass="btn" OnCommand="Delete_SingleItem" CommandArgument='<%# Eval("Product") %>' OnClientClick="return confirm(' Are you sure you wish to delete this item?')">
                            <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;</span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                    <th runat="server"  Width="200px">Product</th>
                                    <th runat="server"  Width="200px">Quantity</th>
                                    <th runat="server"  Width="200px">Price</th>
                                    <th runat="server"  Width="200px">Amount</th>
                                    <th runat="server"  Width="50px"></th>
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

         
        </asp:ListView>

    </div>
    <div class="row">
        <div  class="col-sm-8">
        <div class="col-sm-3">
            <asp:Button ID="SubmitPayment" runat="server" Text="Payment: Cash/Debit" OnClick="Pay_Payment"/>
        </div>
        <div class="col-sm-3">
        <asp:Button ID="DeleteAllCarItems" runat="server" Text="Clear Cart" OnClick="ClearSaleItem"  OnClientClick="return confirm(' You will delete all products in your cart, are you sure you wish to clear?')"/>
            </div>
        <div class="col-sm-3">
            
       
        <asp:Label ID="SaleItemSubTotalLabel" runat="server" Text="Subtotal"></asp:Label>
            <asp:TextBox ID="SaleItemSubTotal" runat="server" Enabled="false"></asp:TextBox>
            <asp:Label ID="SaleItemTaxLabel" runat="server" Text="Tax" ></asp:Label>
            <asp:TextBox ID="SaleItemTax" runat="server" Enabled="false"></asp:TextBox>
            <asp:Label ID="SaleItemTotalLabel" runat="server" Text="Total"></asp:Label>
            <asp:TextBox ID="SaleItemTotal" runat="server" Enabled="false"></asp:TextBox>

             </div>
            </div>
    </div>
    <div class="row">
        <asp:Label ID="NewInvoiceID" runat="server" Text="InvoiceID"></asp:Label>
        <asp:TextBox ID="InvoiceID" runat="server" Enabled="false"></asp:TextBox>
    </div>


    <div class="row">
         <h1>Refund</h1>
         <div class="col-sm-10">
             <asp:TextBox ID="InvoiceNumberDDL" runat="server" placeholder="Original Invoice #"></asp:TextBox>
             <asp:Button ID="LookInvoice" runat="server" Text="Look Up Invoice" OnClick="SearchInvoice_Onclick"/>
             <asp:Button ID="Button4" runat="server" Text="Clear" OnClick="Clearrefund" OnClientClick="return confirm(' You wish to clear the refund table?')"/>
         </div>
    </div>
      <br />
      <br />
    <div class="row"> 
        <asp:ListView ID="SearchRefund" runat="server">
            <AlternatingItemTemplate>
                <tr style="background-color: #FFFFFF; color: #284775;">
                   <td>  <asp:Label Text='<%# Eval("Category") %>' runat="server" ID="CategoryDescription"/></td>
                    <td>
                        <asp:Label Text='<%# Eval("Product") %>' runat="server" ID="ProductLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Qty") %>' runat="server" ID="QtyLabel" /></td>
                    <td>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("Price")) %>' runat="server" ID="PriceLabel" /></td>
                     <td>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("Amount")) %>' runat="server" ID="AmountLabel" /></td>
                    <td>
                         <asp:CheckBox runat="server" ID="RestockChargeCheck" Enabled="false" />
                          <asp:Label Text='<%# string.Format("{0:0.00}",Eval("RestockChg")) %>' runat="server" ID="RestockChgLabel" /></td>
                    <td>
                         <asp:CheckBox runat="server" ID="CheckForRefundReason" OnCheckedChanged="Check_Changed" AutoPostBack="true"/>
                        <asp:TextBox Text='<%# Eval("RefundReason") %>' runat="server" ID="RefundReasonLabel" /></td>
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
                    <td>  <asp:Label Text='<%# Eval("Category") %>' runat="server" ID="CategoryDescription"/></td>
                      
                    <td>
                        <asp:Label Text='<%# Eval("Product") %>' runat="server" ID="ProductLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Qty") %>' runat="server" ID="QtyLabel" /></td>
                    <td>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("Price")) %>' runat="server" ID="PriceLabel" /></td>
                      <td>
                        <asp:Label Text='<%# string.Format("{0:0.00}",Eval("Amount")) %>' runat="server" ID="AmountLabel" /></td>
                        <td>
                         <asp:CheckBox runat="server" ID="RestockChargeCheck" Enabled="false" />
                          <asp:Label Text='<%# string.Format("{0:0.00}",Eval("RestockChg")) %>' runat="server" ID="RestockChgLabel" /></td>
                    <td>
                         <asp:CheckBox runat="server" ID="CheckForRefundReason" OnCheckedChanged="Check_Changed"  AutoPostBack="true" />
                        <asp:TextBox Text='<%# Eval("RefundReason") %>' runat="server" ID="RefundReasonLabel" /></td>



                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                    <th runat="server">Cate</th>
                                    <th runat="server">Product</th>
                                    <th runat="server" Width="50px">Qty</th>
                                    <th runat="server" Width="100px">Price</th>
                                    <th runat="server" Width="100px">Amount</th>
                                    <th runat="server" Width="140px">Restock Chg</th>
                                    <th runat="server">Refund Reason</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                            <asp:DataPager runat="server" ID="DataPager1">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True"></asp:NextPreviousPagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
    
        </asp:ListView>
    </div>
    
    <br />
      <br />
    <div class="row">
        <div class="col-sm-9">
            <div class="col-sm-3">
            <asp:Label ID="SubtotalLabel" runat="server" Text="Subtotal"></asp:Label>
            <asp:TextBox ID="SubtotalForFefund" runat="server" Enabled="false" Text="0.00"></asp:TextBox>
            <asp:Label ID="SubtotalForRefund" runat="server" Text="Tax"></asp:Label>
            <asp:TextBox ID="TaxForRefund" runat="server" Enabled="false" Text="0.00"></asp:TextBox>
            <asp:Label ID="RefundTotalLabel" runat="server" Text="RefundTotal"></asp:Label>
            <asp:TextBox ID="RefundTotalAmount" runat="server" Enabled="false" Text="0.00"></asp:TextBox>
            </div>
            

          <div class="col-sm-5">
                <asp:Button ID="RefundNow" runat="server" Text="Refund: Debit/Credit" OnClick="Refund_Generate"/>
           </div>
            <div class="col-sm-2">
                <asp:Label ID="RefundInvocieID" runat="server">Refund Invocie ID</asp:Label>
                <asp:TextBox ID="NewRefundInvoiceID" runat="server" Enabled="false"></asp:TextBox>
            </div>
            </div>
        </div>
    
    <asp:TextBox ID="InvisibleInvoiceID" runat="server" Visible="false" Enabled="false"></asp:TextBox>











    <asp:ObjectDataSource ID="SelectCategoryODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="CategoryName" TypeName="eRaceSystem.BLL.SaleBLL.SaleProductController"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="SearchRefundODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_Invocie" TypeName="eRaceSystem.BLL.SaleBLL.RefundController">
        <SelectParameters>
            <asp:ControlParameter ControlID="InvoiceNumberDDL" PropertyName="Text" DefaultValue="0" Name="invoicenumber" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>


    <asp:ObjectDataSource ID="SelectProductODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Product_FindByCategory" TypeName="eRaceSystem.BLL.SaleBLL.SaleProductController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SelectCategory" PropertyName="SelectedValue" DefaultValue="0" Name="categoryid" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

</asp:Content>
