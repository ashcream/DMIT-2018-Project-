<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="TeamAssignment.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <p>Team Members: Jiaqi Chen, Yuyang Wang, Chris Li, Yishuai Liu</p>
    <br />
    <h3 >Role: WebMaster</h3>
    <p>User: Webmaster</p>
    <p>Password: Pa$$w0rd</p>
    <br />
    <h4>Default Password for Employees: Pa$$w0rd</h4>
    <br />
    <h3><a runat="server" href="~/ProjectPages/ReceivingSubSystem">Receiving</a></h3>
    <p>User: FoodServiceHolly46</p>
    <p>Role: FoodService</p>
    <br />
    <p>User: ClerkJames20</p>
    <p>Role: Clerk</p>
    <br />
    <h3><a runat="server"  href="~/ProjectPages/PurchasingSubSystem">Purchasing</a></h3>
    <p>User: DirectorAlex24</p>
    <p>Role: Director</p>
    <br />
    <p>User: OfficeManagerKaren12</p>
    <p>Role: OfficeManager</p>
    <br />
    <h3><a runat="server" href="~/ProjectPages/RacingSubSystem">Racing</a></h3>
    <p>User: RaceCoordinatorJohn40</p>
    <p>Role: RaceCoordinator</p>
    <br />
    <h3><a runat="server" href="~/ProjectPages/SaleSubSystem">Sale</a></h3>
    <p>User: ClerkJames20</p>
    <p>Role: Clerk</p>
    <br />
    <h3>Database Connection:</h3>
    <p>

name="DefaultConnection" connectionString="Data Source=.;Initial Catalog=eRace2018;Integrated Security=True"
     providerName="System.Data.SqlClient" 
        </p>
    <p>
name ="eRaceDB"
       connectionString ="Data Source =.; Initial Catalog =eRace2018; Integrated Security =true"
       providerName = "System.Data.SqlClient"


    </p>
    <asp:Button ID="Download" runat="server" Text="Download Database" OnClick="Download_Click" />
    <asp:Label ID="Label1" runat="server" Text="eRaceDatabase"></asp:Label>
</asp:Content>
