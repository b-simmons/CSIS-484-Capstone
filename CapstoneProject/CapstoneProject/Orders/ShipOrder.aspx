<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShipOrder.aspx.cs" Inherits="CapstoneProject.Orders.ShipOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            if (jQuery("[id$='GrOrders'] tr").length > 2) {
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
    <asp:HiddenField ID="HFOrderID" runat="server" Value="0" />
    <asp:GridView ID="GrOrders" runat="server" CssClass="table table-striped dt-table-responsive" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
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
                    <asp:LinkButton ID="BtnSelectOrder" runat="server" CssClass="btn btn-primary" OnClick="BtnSelectOrder_Click" Text="View Details" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <hr />
    <div id="DivDetails" runat="server" visible="false">
        <div class="panel panel-default">
            <div class="panel-heading">Order Details</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">
                        <label for="LblOrderDate">Order Date:</label>
                        <asp:Label ID="LblOrderDate" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <label for="LblCustomer">Customer:</label>
                        <asp:Label ID="LblCustomer" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <label for="LblLocation">Location:</label>
                        <asp:Label ID="LblLocation" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <label for="LblLocation">Location Contact Number:</label>
                        <asp:Label ID="LblLocationContactNum" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <label>Order Contents</label>
                        <asp:GridView ID="GrOrderContents" runat="server" CssClass="table table-striped dt-table-responsive" AllowPaging="false" UseAccessibleHeader="true" AutoGenerateColumns="false"
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
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">Shipment Details</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">
                        <label for="TxtShipmentDate">Shipment Date</label>
                        <asp:TextBox ID="TxtShipmentDate" runat="server" CssClass="form-control date-field"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtShipmentDate"
                            CssClass="text-danger" ErrorMessage="The Shipment Date field is required." EnableClientScript="true"
                            ValidationGroup="vgShipment" Display="Dynamic" />
                        <asp:CompareValidator ID="CVOrderDateFuture" runat="server" Type="Date" ControlToValidate="TxtShipmentDate" ValidationGroup="vgShipment" CssClass="text-danger"
                            EnableClientScript="true" ErrorMessage="Date cannot be in the future!" ValueToCompare="01/01/18" Display="Dynamic">
                        </asp:CompareValidator>
                        <asp:CompareValidator ID="CVOrderDateTypeCheck" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="TxtShipmentDate" CssClass="text-danger"
                            ValidationGroup="vgShipment" ErrorMessage="Not a valid date!" EnableClientScript="true" Display="Dynamic">
                        </asp:CompareValidator>
                    </div>
                    <div class="col-md-3">
                        <label for="TxtShippingService">Shipping Service</label>
                        <asp:TextBox ID="TxtShippingService" runat="server" CssClass="form-control date-field" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtShippingService"
                            CssClass="text-danger" ErrorMessage="The Shipping Service field is required." EnableClientScript="true"
                            ValidationGroup="vgShipment" Display="Dynamic" />
                    </div>
                    <div class="col-md-3">
                        <label for="TxtTrackingNum">Tracking Number</label>
                        <asp:TextBox ID="TxtTrackingNum" runat="server" CssClass="form-control date-field" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTrackingNum"
                            CssClass="text-danger" ErrorMessage="The Tracking Number field is required." EnableClientScript="true"
                            ValidationGroup="vgShipment" Display="Dynamic" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Button ID="BtnCancel" runat="server" class="btn btn-default" OnClick="BtnCancel_Click" Text="Cancel"></asp:Button>
                        <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btn-primary" OnClick="BtnSubmit_Click" Text="Submit" ValidationGroup="vgOrder" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <asp:Button ID="BtnRemoveShipment" runat="server" CssClass="btn btn-danger" OnClick="BtnRemoveShipment_Click" Text="Remove Shipment Details" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
