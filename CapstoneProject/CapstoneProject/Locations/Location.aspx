<%@ Page Title="Shipping Location" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Location.aspx.cs" Inherits="CapstoneProject.Locations.Location" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        //Apply input masks
        $j = jQuery.noConflict();
        $j(document).ready(function(){
            $j(".phone").inputmask("999-999-9999", { clearIncomplete: true });
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
            <label for="DDLCustomer">Customer/Business</label>
            <asp:DropDownList ID="DDLCustomer" runat="server" AppendDataBoundItems="true" DataValueField="CustomerID" CssClass="form-control" DataTextField="BusinessName">
                <asp:ListItem Value="-1" Text="--Select--"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="DDLCustomer"
                CssClass="text-danger" ErrorMessage="The Customer/Business field is required." EnableClientScript="true"
                ValidationGroup="VGLocation" Display="Dynamic" InitialValue="-1" />
        </div>
        <div class="col-md-3">
            <label for="TxtAddress">Address</label>
            <asp:TextBox ID="TxtAddress" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAddress"
                CssClass="text-danger" ErrorMessage="The Address field is required." EnableClientScript="true"
                ValidationGroup="VGLocation" />
        </div>
        <div class="col-md-3">
            <label for="TxtPhone">Phone Number</label>
            <asp:TextBox ID="TxtPhone" runat="server" CssClass="form-control phone"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPhone"
                CssClass="text-danger" ErrorMessage="The Phone Number field is required." EnableClientScript="true"
                ValidationGroup="VGLocation" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:HyperLink ID="LnkCancel" runat="server" class="btn btn-default" NavigateUrl="Locations.aspx" Text="Cancel"></asp:HyperLink>
            <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" Text="Submit" ValidationGroup="VGLocation" />
        </div>
    </div>
</asp:Content>
