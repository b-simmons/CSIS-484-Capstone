<%@ Page Title="User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="CapstoneProject.Admin.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="text-center">
        <asp:Label ID="LblPageTitle" runat="server" CssClass="text-center h2" Text=""></asp:Label>
    </div>
    <hr />
    <p class="text-danger">
        <asp:Literal runat="server" ID="LtlErrorMessage" />
    </p>
    <p class="text-success">
        <asp:Literal runat="server" ID="LtlSuccessMessage" />
    </p>
    <div class="row">
        <div class="col-md-3">
            <label for="TxtUsername">Username</label>
            <asp:TextBox ID="TxtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUsername"
                CssClass="text-danger" ErrorMessage="The Username field is required." EnableClientScript="true"
                ValidationGroup="vgUser" />
        </div>
        <div class="col-md-3">
            <label for="DDLRole">Role</label>
            <asp:DropDownList ID="DDLRole" runat="server" AppendDataBoundItems="true" CssClass="form-control" DataTextField="Name" DataValueField="Id">
                <asp:ListItem Selected="True" Text="--Select--" Value="-1"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="DDLRole"
                CssClass="text-danger" ErrorMessage="A role must be selected." EnableClientScript="true"
                ValidationGroup="vgUser" InitialValue="-1" />
        </div>
        <div class="col-md-3">
            <label for="TxtEmail">Email</label>
            <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEmail"
                CssClass="text-danger" ErrorMessage="The Email field is required." EnableClientScript="true"
                ValidationGroup="vgUser" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <label for="TxtPhone">Phone Number</label>
            <asp:TextBox ID="TxtPhone" runat="server" CssClass="form-control" TextMode="Phone"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPhone"
                CssClass="text-danger" ErrorMessage="The Phone Number field is required." EnableClientScript="true"
                ValidationGroup="vgUser" />
        </div>
        <div class="col-md-3">
            <label for="TxtPassword">Password</label>
            <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVPassword" runat="server" ControlToValidate="TxtPassword"
                CssClass="text-danger" ErrorMessage="The Password field is required." EnableClientScript="true"
                ValidationGroup="vgUser" />
            <asp:CompareValidator ID="CVPassword" runat="server" ControlToCompare="TxtPassword" ControlToValidate="TxtConfirmPassword"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." EnableClientScript="true"
                ValidationGroup="vgUser"/>
        </div>
        <div class="col-md-3">
            <label for="TxtConfirmPassword">Confirm Password</label>
            <asp:TextBox ID="TxtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVConfirmPassword" runat="server" ControlToValidate="TxtConfirmPassword"
                CssClass="text-danger" ErrorMessage="The Confirm Password field is required." EnableClientScript="true"
                ValidationGroup="vgUser" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:HyperLink id="LnkCancel" runat="server" class="btn btn-default" NavigateUrl="ManageUsers.aspx" Text="Cancel"></asp:HyperLink>
            <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" Text="Submit" ValidationGroup="vgUser" />
        </div>
    </div>
</asp:Content>
