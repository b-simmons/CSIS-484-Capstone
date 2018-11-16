using CapstoneProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Orders
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Order.aspx.cs
     */
    public partial class Order : System.Web.UI.Page
    {
        private DataTable orderContents
        {
            get
            {
                return (ViewState["OrderContents"] == null ? null : (DataTable)ViewState["OrderContents"]);
            }
            set
            {
                ViewState["OrderContents"] = value;
            }
        }

        private CapstoneEntities context = new CapstoneEntities();
        private bool isEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if edit
            if (!String.IsNullOrWhiteSpace(Request.QueryString["orderid"]))
            {
                isEdit = true;
                LblPageTitle.Text = "Edit Order";
            }
            else
            {
                LblPageTitle.Text = "New Order";
            }

            //Set validator
            CVOrderDateFuture.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");

            if (!IsPostBack)
            {
                //Get all customers
                List<Models.Customer> allCustomers = context.Customers.ToList();
                DDLCustomer.DataSource = allCustomers;
                DDLCustomer.DataBind();

                //Get all products and bind the products drop-down
                List<Models.Product> allProducts = context.Products.ToList();
                DDLProduct.DataSource = from product in allProducts
                                        select new
                                        {
                                            product.ProductID,
                                            product.ProductName,
                                            product.Description,
                                            DisplayField = String.Format("{0}  ${1}", product.ProductName, product.Price)
                                        };
                DDLProduct.DataBind();

                //Create the datatable columns
                DataTable table = new DataTable();
                table.Columns.Add("OrderLineID");
                table.Columns.Add("ProductID");
                table.Columns.Add("ProductName");
                table.Columns.Add("Quantity");
                table.Columns.Add("LineTotal");
                orderContents = table;

                if(isEdit)
                {
                    //Get the order ID
                    int orderID = Convert.ToInt32(Request.QueryString["orderid"]);

                    //Get the order object
                    Models.Order order = context.Orders.Where(o => o.OrderID == orderID).FirstOrDefault();

                    //Set the input field values
                    TxtOrderDate.Text = order.OrderDate.ToString("MM/dd/yy");
                    DDLCustomer.SelectedValue = order.CustomerID.ToString();

                    //Get all locations for the customer
                    var locations = from l in context.Locations
                                    where l.CustomerID == order.CustomerID
                                    select l;

                    //Bind the locations drop-down
                    DDLLocation.DataSource = locations.ToList();
                    DDLLocation.DataBind();
                    DDLLocation.Items.Insert(0, new ListItem("--Select--", "-1"));

                    //Enable the locations drop-down and select the proper value
                    DDLLocation.Enabled = true;
                    DDLLocation.SelectedValue = order.LocationID.ToString();

                    //Fill the datatable
                    foreach(Models.OrderLine line in order.OrderLines)
                    {
                        DataRow row = table.NewRow();
                        row["OrderLineID"] = line.OrderLineID;
                        row["ProductID"] = line.ProductID;
                        row["ProductName"] = line.Product.ProductName;
                        row["Quantity"] = line.Quantity;
                        row["LineTotal"] = line.Product.Price * line.Quantity;
                        orderContents.Rows.Add(row);
                    }

                    //If order shipped, lock controls and show shipping info
                    if(order.Shipments.Count > 0)
                    {
                        //Get shipping info
                        Models.Shipment shipment = order.Shipments.FirstOrDefault();
                        LblShipmentDate.Text = shipment.ShipmentDate.ToString("MM/dd/yy");
                        LblTrackingNum.Text = shipment.TrackingNumber;
                        LblShippingService.Text = shipment.ShippingServiceName;
                        DivShipmentDetails.Visible = true;

                        //Lock the controls
                        LockControls();
                    }
                }
            }
            //Bind the order contents gridview
            GrOrderContents.DataSource = orderContents;
            GrOrderContents.DataBind();
        }

        /// <summary>
        /// This method handles the BtnAddToOrder click event.
        /// </summary>
        /// <param name="sender">The BtnAddToOrder button</param>
        /// <param name="e">The click event</param>
        protected void BtnAddToOrder_Click(object sender, EventArgs e)
        {
            //Get product and quantity
            int productID = Convert.ToInt32(DDLProduct.SelectedValue);
            int quantity = Convert.ToInt32(TxtQuantity.Text);

            //Get the product object
            var product = context.Products
                    .Where(p => p.ProductID == productID)
                    .FirstOrDefault();

            //If that product has been selected, modify the row, otherwise, add it to the order
            if (orderContents.Select("ProductID Like '" + productID + "'").FirstOrDefault() == null)
            {
                //Add the new product to the order
                DataRow row = orderContents.NewRow();
                row["OrderLineID"] = 0;
                row["ProductID"] = productID;
                row["ProductName"] = product.ProductName;
                row["Quantity"] = quantity;
                row["LineTotal"] = product.Price * quantity;
                orderContents.Rows.Add(row);
            }
            else
            {
                //Edit the product
                DataRow row = orderContents.Select("ProductID Like '" + productID + "'").FirstOrDefault();
                row["ProductID"] = productID;
                row["ProductName"] = product.ProductName;
                row["Quantity"] = quantity;
                row["LineTotal"] = product.Price * quantity;
            }

            //Bind the order contents gridview
            GrOrderContents.DataSource = orderContents;
            GrOrderContents.DataBind();
            
            //Reset the inputs
            DDLProduct.SelectedIndex = 0;
            TxtQuantity.Text = "";
        }

        /// <summary>
        /// This method handles the click event for the BtnSubmit button
        /// </summary>
        /// <param name="sender">The BtnSubmit button</param>
        /// <param name="e">The click event</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            Models.Order order = new Models.Order();
            //Get the user
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByName(User.Identity.Name);

            if (isEdit)
            {
                //Get the order object
                int orderID = Convert.ToInt32(Request.QueryString["orderid"]);
                order = context.Orders.Where(o => o.OrderID == orderID).FirstOrDefault();

                //Edit the information
                order.OrderDate = Convert.ToDateTime(TxtOrderDate.Text);
                order.CustomerID = Convert.ToInt32(DDLCustomer.SelectedValue);
                order.LocationID = Convert.ToInt32(DDLLocation.SelectedValue);
                order.SalesRepID = user.Id;
                order.OrderTotal = 0.00m;

                context.SaveChanges();
            }
            else
            {
                //Create the order object
                order.OrderDate = Convert.ToDateTime(TxtOrderDate.Text);
                order.CustomerID = Convert.ToInt32(DDLCustomer.SelectedValue);
                order.LocationID = Convert.ToInt32(DDLLocation.SelectedValue);
                order.SalesRepID = user.Id;
                order.OrderTotal = 0.00m;

                //Save the order object
                context.Orders.Add(order);
                context.SaveChanges();
            }

            decimal total = 0.00m;

            //Loop through and add all order lines
            foreach(DataRow row in orderContents.Rows)
            {
                Models.OrderLine orderLine;
                int orderLineID = Convert.ToInt32(row["OrderLineID"]);

                //Add to the total
                total += Convert.ToDecimal(row["LineTotal"]);

                //If the id is over 0, this is an edit
                if (orderLineID != 0)
                {
                    //Edit the order line
                    orderLine = context.OrderLines.Where(l => l.OrderLineID == orderLineID).FirstOrDefault();
                    orderLine.OrderID = order.OrderID;
                    orderLine.ProductID = Convert.ToInt32(row["ProductID"]);
                    orderLine.Quantity = Convert.ToInt32(row["Quantity"]);
                    context.SaveChanges();
                }
                else
                {
                    //Create the order line
                    orderLine = new Models.OrderLine();
                    orderLine.OrderID = order.OrderID;
                    orderLine.ProductID = Convert.ToInt32(row["ProductID"]);
                    orderLine.Quantity = Convert.ToInt32(row["Quantity"]);

                    //Save the OrderLine object
                    context.OrderLines.Add(orderLine);
                    context.SaveChanges();
                }

                //Edit the Product object
                Models.Product product = context.Products.Where(p => p.ProductID == orderLine.ProductID).FirstOrDefault();
                product.QuantityInStock -= orderLine.Quantity;
                context.SaveChanges();
            }

            //Save the order total
            order.OrderTotal = total;
            context.SaveChanges();

            //Redirect the user
            if(isEdit)
                //Edit success message
                Response.Redirect("/Orders/Orders.aspx?successmessage=2");
            else
                //Add success message
                Response.Redirect("/Orders/Orders.aspx?successmessage=1");
        }

        /// <summary>
        /// This method handles the click event for the BtnRemove LinkButton
        /// </summary>
        /// <param name="sender">The BtnRemove LinkButton</param>
        /// <param name="e">The click event</param>
        protected void BtnRemove_Click(object sender, EventArgs e)
        {
            //Get the product ID
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
            int productID = Convert.ToInt32(GrOrderContents.DataKeys[row.RowIndex].Values["ProductID"]);
            int orderLineID = Convert.ToInt32(GrOrderContents.DataKeys[row.RowIndex].Values["OrderLineID"]);

            //Get the row from the order contents table and remove it
            DataRow orderRow = orderContents.Select("ProductID Like '" + productID + "'").FirstOrDefault();
            orderContents.Rows.Remove(orderRow);

            //If the row exists in the database, delete it
            if(orderLineID != 0)
            {
                Models.OrderLine orderLine = context.OrderLines.Where(l => l.OrderLineID == orderLineID).FirstOrDefault();

                //Edit the Product object
                Models.Product product = context.Products.Where(p => p.ProductID == orderLine.ProductID).FirstOrDefault();
                product.QuantityInStock += orderLine.Quantity;
                context.SaveChanges();

                //Remove the order line object
                context.OrderLines.Remove(orderLine);
                context.SaveChanges();
            }

            //Bind the order contents gridview
            GrOrderContents.DataSource = orderContents;
            GrOrderContents.DataBind();
        }

        /// <summary>
        /// This method handles the SelectedIndexChanged event for the DDLCustomer drop-down
        /// </summary>
        /// <param name="sender">The DDLCustomer drop-down</param>
        /// <param name="e">The SelectedIndexChanged event</param>
        protected void DDLCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLCustomer.SelectedIndex != 0)
            {
                //Get the customer ID
                int customerID = Convert.ToInt32(DDLCustomer.SelectedValue);

                //Get all locations for the customer
                var locations = from l in context.Locations
                                 where l.CustomerID == customerID
                                 select l;

                //Bind the locations drop-down
                DDLLocation.DataSource = locations.ToList();
                DDLLocation.DataBind();
                DDLLocation.Items.Insert(0, new ListItem("--Select--", "-1"));

                //Enable the location drop-down
                DDLLocation.Enabled = true;
            }
            else
            {
                //Reset the location drop-down
                DDLLocation.SelectedIndex = 0;
                DDLLocation.Enabled = true;
            }
        }

        /// <summary>
        /// This methods locks all the controls on the page
        /// </summary>
        private void LockControls()
        {
            TxtOrderDate.Enabled = false;
            TxtQuantity.Enabled = false;
            DDLCustomer.Enabled = false;
            DDLLocation.Enabled = false;
            DDLProduct.Enabled = false;
            BtnSubmit.Visible = false;
            BtnAddToOrder.Visible = false;
            LnkCancel.Text = "Close";
        }
    }
}