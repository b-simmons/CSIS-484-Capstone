<%@ Page Title="Manage Shipping Locations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locations.aspx.cs" Inherits="CapstoneProject.Locations.Locations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            if (jQuery("[id$='GrLocations'] tr").length > 2) {
                jQuery("[id$='GrLocations'").DataTable();
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
    <asp:GridView ID="GrLocations" runat="server" CssClass="table table-striped dt-table-responsive" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
        OnRowDataBound="GrLocations_RowDataBound" DataKeyNames="LocationID">
        <Columns>
            <asp:TemplateField HeaderText="Customer/Business Name">
                <ItemTemplate>
                    <asp:Label ID="LblBusinessName" runat="server" Text='<%# Bind("BusinessName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Label ID="LblAddress" runat="server" Text='<%# Bind("Address") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone Number">
                <ItemTemplate>
                    <asp:Label ID="LblPhoneNumer" runat="server" Text='<%# Bind("PhoneNumber") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="BtnEditLocation" runat="server" CssClass="btn btn-default" OnClick="BtnEditLocation_Click" Text="Edit" />
                    <asp:LinkButton ID="BtnDeleteLocation" runat="server" CssClass="btn btn-danger" OnClick="BtnDeleteLocation_Click" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HyperLink ID="BtnNewLocation" runat="server" CssClass="btn btn-primary" NavigateUrl="/Locations/Location.aspx" Text="New Location" />
</asp:Content>
