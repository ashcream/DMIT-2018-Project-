<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeamAssignment._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

        <div class="jumbotron">
            <h1>
                eRace System
            </h1>
            <br />
            <asp:Image ID="Image1" ImageUrl="TeamLogo.png" runat="server" />
            <br /><br /><br />
            <p>TeamName:TeamJYCY</p>
            <p>Team Members: Jiaqi Chen, Yuyang Wang, Chris Li, Yishuai Liu</p>
    </div>

    <div class="row">
        <div class="col-md-3">
            <h2>Sale</h2>
            <p>
                Jiaqi Chen
            </p>
            <p>
            <a href="ProjectPages/saleSubSystem" class="btn btn-primary btn-lg">Go &raquo;</a>
            </p>
        </div>
        <div class="col-md-3">
            <h2>Purchasing</h2>
            <p>
                by Yuyang Wang
            </p>
            <p>
                <a href="ProjectPages/PurchasingSubSystem" class="btn btn-primary btn-lg">Go &raquo;</a>
            </p>
        </div>
        <div class="col-md-3">
            <h2>Receiving</h2>
            <p>
                by Chris Li
            </p>
            <p>
                <a href="ProjectPages/ReceivingSubSystem" class="btn btn-primary btn-lg">Go &raquo;</a>
            </p>
        </div>
        <div class="col-md-3">
            <h2>Racing</h2>
            <p>
                by Yishuai Liu
            </p>
            <p>
                <a href="ProjectPages/RacingSubSystem" class="btn btn-primary btn-lg">Go &raquo;</a>
            </p>
        </div>
        <hr />
        <hr />
        <hr />

        <div class="col-md-4">
            <h2>Work Assigned</h2>
            <p>
                Setup/ReverseEngineering assigned to Jiaqi and Chris
            </p>
            <p>
                Default Page, about page assigned to Yuyang
            </p>
           <p>
                Security assigned to Yishuai
            </p>
        </div>
        <div class="col-md-4">
            <h2>Known Bugs</h2>
            <p>
               none             
            </p>
            
        </div>
    </div>

</asp:Content>
