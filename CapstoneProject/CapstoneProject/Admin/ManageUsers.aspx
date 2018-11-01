<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="CapstoneProject.Admin.ManageUsers" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            if (!jQuery.fn.DataTable.isDataTable("[id$='GrUsers']") && jQuery("[id$='GrUsers'] tr").length > 2) {
                jQuery("[id$='GrUsers'").DataTable();
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center"><%: Title %></h2>
    <hr />
    <asp:GridView ID="GrUsers" runat="server" CssClass="table table-striped" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
        OnRowDataBound="GrUsers_RowDataBound" DataKeyNames="Id, UserName">
        <Columns>
            <asp:TemplateField HeaderText="UserName">
                <ItemTemplate>
                    <asp:Label ID="LblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate>
                    <asp:Label ID="LblUserEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="BtnEditUser" runat="server" CssClass="btn btn-default" OnClick="BtnEditUser_Click" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="BtnNewUser" runat="server" CssClass="btn btn-primary" NavigateUrl="~/Admin/User.aspx" Text="New User" />
</asp:Content>
