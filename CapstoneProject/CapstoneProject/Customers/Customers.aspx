<%@ Page Title="Manage Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="CapstoneProject.Customers.Customers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            if (jQuery("[id$='GrCustomers'] tr").length > 2) {
                jQuery("[id$='GrCustomers'").DataTable();
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
    <asp:GridView ID="GrCustomers" runat="server" CssClass="table table-striped dt-table-responsive" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
        OnRowDataBound="GrCustomers_RowDataBound" DataKeyNames="CustomerID">
        <Columns>
            <asp:TemplateField HeaderText="Business Name">
                <ItemTemplate>
                    <asp:Label ID="LblCustomerName" runat="server" Text='<%# Bind("BusinessName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Billing Address">
                <ItemTemplate>
                    <asp:Label ID="LblBillingAddress" runat="server" Text='<%# Bind("BillingAddress") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone Number">
                <ItemTemplate>
                    <asp:Label ID="LblPhoneNumer" runat="server" Text='<%# Bind("PhoneNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email Address">
                <ItemTemplate>
                    <asp:Label ID="LblEmail" runat="server" Text='<%# Bind("EmailAddress") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="BtnEditCustomer" runat="server" CssClass="btn btn-default" OnClick="BtnEditCustomer_Click" Text="Edit" />
                    <asp:LinkButton ID="BtnDeleteCustomer" runat="server" CssClass="btn btn-danger" OnClick="BtnDeleteCustomer_Click" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="BtnNewCustomer" runat="server" CssClass="btn btn-primary" NavigateUrl="/Customers/Customer.aspx" Text="New Customer" />
</asp:Content>
