<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="CapstoneProject.Products.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            if (jQuery("[id$='GrProducts'] tr").length > 2) {
                jQuery("[id$='GrProducts'").DataTable();
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
    <asp:GridView ID="GrProducts" runat="server" CssClass="table table-striped dt-table-responsive" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
        OnRowDataBound="GrProducts_RowDataBound" DataKeyNames="ProductID">
        <Columns>
            <asp:TemplateField HeaderText="Product Name">
                <ItemTemplate>
                    <asp:Label ID="LblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Product Description">
                <ItemTemplate>
                    <asp:Label ID="LblProductDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Quantity In-Stock">
                <ItemTemplate>
                    <asp:Label ID="LblQuantityInStock" runat="server" Text='<%# Bind("QuantityInStock") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    <asp:Label ID="LblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="LblOnOrder" runat="server" Visible="false" Text="Cannot edit or delete due to product existing on an order"></asp:Label>
                    <asp:LinkButton ID="BtnEditProduct" runat="server" CssClass="btn btn-default" OnClick="BtnEditProduct_Click" Text="Edit" />
                    <asp:LinkButton ID="BtnDeleteProduct" runat="server" CssClass="btn btn-danger" OnClick="BtnDeleteProduct_Click" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="BtnNewProduct" runat="server" CssClass="btn btn-primary" NavigateUrl="/Products/Product.aspx" Text="New Product" />
</asp:Content>
