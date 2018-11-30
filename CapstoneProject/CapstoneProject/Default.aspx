<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CapstoneProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1 class="text-center">Welcome to the Syrup Inc. Ordering System!</h1>
    </div>
    <asp:LoginView runat="server" ViewStateMode="Disabled">
        <AnonymousTemplate>
           <div class="text-center">
                <a href="Account/Login.aspx" class="btn btn-lg btn-primary">Log In</a>
            </div>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <div class="text-center">
                <h3>Please use the navigation options above to contine!</h3>
            </div>
        </LoggedInTemplate>
    </asp:LoginView>
</asp:Content>
