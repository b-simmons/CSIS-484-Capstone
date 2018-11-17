<%@ Page Title="Product" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="CapstoneProject.Products.Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        //Apply input masks
        $j = jQuery.noConflict();
        $j(document).ready(function(){
            $j(".quantity").inputmask("9[999]");
            $j(".price").inputmask("9.99");
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
            <label for="TxtProductName">Product Name</label>
            <asp:TextBox ID="TxtProductName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtProductName"
                CssClass="text-danger" ErrorMessage="The Product Name field is required." EnableClientScript="true"
                ValidationGroup="VGProduct" Display="Dynamic" />
        </div>
        <div class="col-md-3">
            <label for="TxtProductDescription">Product Description</label>
            <asp:TextBox ID="TxtProductDescription" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtProductDescription"
                CssClass="text-danger" ErrorMessage="The Product Description field is required." EnableClientScript="true"
                ValidationGroup="VGProduct" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <label for="TxtQuantity">Quantity In Stock</label>
            <asp:TextBox ID="TxtQuantity" runat="server" CssClass="form-control quantity"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtQuantity"
                CssClass="text-danger" ErrorMessage="The Quantity In Stock field is required." EnableClientScript="true"
                ValidationGroup="VGProduct" />
            <asp:RangeValidator runat="server" CssClass="text-danger" ControlToValidate="TxtQuantity" ErrorMessage="Quantity In Stock must be a positive number!"
                EnableClientScript="true" ValidationGroup="VGProduct" MaximumValue="9999" MinimumValue="1" Display="Dynamic"></asp:RangeValidator>
        </div>
        <div class="col-md-3">
            <label for="TxtPrice">Price</label>
            <asp:TextBox ID="TxtPrice" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPrice"
                CssClass="text-danger" ErrorMessage="The Price field is required." EnableClientScript="true"
                ValidationGroup="VGProduct" />
            <asp:RangeValidator runat="server" CssClass="text-danger" ControlToValidate="TxtQuantity" ErrorMessage="Price must be a positive number!"
                EnableClientScript="true" ValidationGroup="VGProduct" MaximumValue="9999" MinimumValue="1" Display="Dynamic"></asp:RangeValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:HyperLink ID="LnkCancel" runat="server" class="btn btn-default" NavigateUrl="Products.aspx" Text="Cancel"></asp:HyperLink>
            <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" Text="Submit" ValidationGroup="VGProduct" />
        </div>
    </div>
</asp:Content>
