<%@ Page Title="Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="CapstoneProject.Customers.Customer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        //Apply input masks
        $j = jQuery.noConflict();
        $j(document).ready(function(){
            $j(".phone").inputmask("999-999-9999");
        });
    </script>
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
            <label for="TxtBusinessName">Business Name</label>
            <asp:TextBox ID="TxtBusinessName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtBusinessName"
                CssClass="text-danger" ErrorMessage="The Business Name field is required." EnableClientScript="true"
                ValidationGroup="VGCustomer" Display="Dynamic" />
        </div>
        <div class="col-md-3">
            <label for="TxtBillingAddress">Billing Address</label>
            <asp:TextBox ID="TxtBillingAddress" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtBillingAddress"
                CssClass="text-danger" ErrorMessage="The Billing Address field is required." EnableClientScript="true"
                ValidationGroup="VGCustomer" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <label for="TxtPhone">Phone Number</label>
            <asp:TextBox ID="TxtPhone" runat="server" CssClass="form-control phone"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPhone"
                CssClass="text-danger" ErrorMessage="The Phone Number field is required." EnableClientScript="true"
                ValidationGroup="VGCustomer" />
        </div>
        <div class="col-md-3">
            <label for="TxtEmailAddress">Email Address</label>
            <asp:TextBox ID="TxtEmailAddress" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEmailAddress"
                CssClass="text-danger" ErrorMessage="The Email Address field is required." EnableClientScript="true"
                ValidationGroup="VGCustomer" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:HyperLink ID="LnkCancel" runat="server" class="btn btn-default" NavigateUrl="Customers.aspx" Text="Cancel"></asp:HyperLink>
            <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" Text="Submit" ValidationGroup="VGCustomer" />
        </div>
    </div>
</asp:Content>
