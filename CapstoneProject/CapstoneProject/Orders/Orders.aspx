<%@ Page Title="Manage Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="CapstoneProject.Orders.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            if (!jQuery.fn.DataTable.isDataTable("[id$='GrOrders']") && jQuery("[id$='GrOrders'] tr").length > 2) {
                jQuery("[id$='GrOrders'").DataTable();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center"><%: Title %></h2>
    <hr />
    <div id="DivSuccessMessage" class="alert alert-success" runat="server" visible="false">
        <asp:Label ID="LblSuccessMessage" runat="server" Text=""></asp:Label>
    </div>
    <asp:GridView ID="GrOrders" runat="server" CssClass="table table-striped" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
        OnRowDataBound="GrOrders_RowDataBound" DataKeyNames="OrderID, CustomerID, LocationID">
        <Columns>
            <asp:TemplateField HeaderText="Order Date">
                <ItemTemplate>
                    <asp:Label ID="LblOrderDate" runat="server" Text='<%# Bind("OrderDate", "{0: MM/dd/yy}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Customer Name">
                <ItemTemplate>
                    <asp:Label ID="LblCustomerName" runat="server" Text='<%# Bind("BusinessName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Shipping Location">
                <ItemTemplate>
                    <asp:Label ID="LblShippingLocation" runat="server" Text='<%# Bind("Address") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Shipped">
                <ItemTemplate>
                    <asp:Label ID="LblShipped" runat="server" Text='<%# Bind("Shipped") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Order Total">
                <ItemTemplate>
                    <asp:Label ID="LblOrderTotal" runat="server" Text='<%# Bind("OrderTotal") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="BtnEditOrder" runat="server" CssClass="btn btn-default" OnClick="BtnEditOrder_Click" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="BtnNewOrder" runat="server" CssClass="btn btn-primary" NavigateUrl="/Orders/Order.aspx" Text="New Order" />
</asp:Content>
