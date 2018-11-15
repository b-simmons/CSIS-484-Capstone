﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="CapstoneProject.Orders.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        $('document').ready(function () {
            $(".number-only-2").mask("9?9");
            $(".date-field").mask("99/99/99");
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
            <label for="TxtOrderDate">Order Date</label>
            <asp:TextBox ID="TxtOrderDate" runat="server" CssClass="form-control date-field"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtOrderDate"
                CssClass="text-danger" ErrorMessage="The Order Date field is required." EnableClientScript="true"
                ValidationGroup="vgOrder" />
        </div>
        <div class="col-md-3">
            <label for="DDLRole">Customer</label>
            <asp:DropDownList ID="DDLCustomer" runat="server" AppendDataBoundItems="true" CssClass="form-control" DataTextField="BusinessName" DataValueField="CustomerID"
                OnSelectedIndexChanged="DDLCustomer_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True" Text="--Select--" Value="-1"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="DDLCustomer"
                CssClass="text-danger" ErrorMessage="A Customer must be selected." EnableClientScript="true"
                ValidationGroup="vgOrder" InitialValue="-1" />
        </div>
        <div class="col-md-3">
            <label for="DDLLocation">Location</label>
            <asp:DropDownList ID="DDLLocation" runat="server" AppendDataBoundItems="false" Enabled="false" CssClass="form-control" DataTextField="Address" DataValueField="LocationID">
                <asp:ListItem Selected="True" Text="--Select--" Value="-1"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="DDLLocation"
                CssClass="text-danger" ErrorMessage="A Location must be selected." EnableClientScript="true"
                ValidationGroup="vgOrder" InitialValue="-1" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <label>Order Contents</label>
            <asp:GridView ID="GrOrderContents" runat="server" CssClass="table table-striped" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
                DataKeyNames="ProductID, Quantity, OrderLineID" ShowHeaderWhenEmpty="true" EmptyDataText="No contents">
                <Columns>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="LblProduct" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="LblQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Line Total">
                        <ItemTemplate>
                            <asp:Label ID="LblLineTotal" runat="server" Text='<%# Bind("LineTotal", "${0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="BtnRemove" runat="server" CssClass="btn btn-danger" OnClick="BtnRemove_Click" Text="Remove" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3">
            <label for="DDLProduct">Product</label>
            <asp:DropDownList ID="DDLProduct" runat="server" AppendDataBoundItems="true" CssClass="form-control" DataTextField="DisplayField" DataValueField="ProductID">
                <asp:ListItem Selected="True" Text="--Select--" Value="-1"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="DDLProduct"
                CssClass="text-danger" ErrorMessage="A Product must be selected." EnableClientScript="true"
                ValidationGroup="vgOrderLine" InitialValue="-1" />
        </div>
        <div class="col-md-3">
            <label for="TxtQuantity">Quantity</label>
            <asp:TextBox ID="TxtQuantity" runat="server" CssClass="form-control number-only-2"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtQuantity"
                CssClass="text-danger" ErrorMessage="The Quantity field is required." EnableClientScript="true"
                ValidationGroup="vgOrderLine" />
        </div>
        <div class="col-md-3">
            <br />
            <asp:Button ID="BtnAddToOrder" runat="server" CssClass="btn btn-primary" OnClick="BtnAddToOrder_Click" Text="Add to Order" ValidationGroup="vgOrderLine" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:HyperLink ID="LnkCancel" runat="server" class="btn btn-default" NavigateUrl="Orders.aspx" Text="Cancel"></asp:HyperLink>
            <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" Text="Submit" ValidationGroup="vgOrder" />
        </div>
    </div>
</asp:Content>