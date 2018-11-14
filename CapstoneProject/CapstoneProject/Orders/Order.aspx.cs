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
        public DataTable orderContents
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Get all customers
                CapstoneEntities context = new CapstoneEntities();
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
                table.Columns.Add("ProductID");
                table.Columns.Add("ProductName");
                table.Columns.Add("Quantity");
                table.Columns.Add("LineTotal");
                orderContents = table;
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
            CapstoneEntities context = new CapstoneEntities();
            var product = context.Products
                    .Where(p => p.ProductID == productID)
                    .FirstOrDefault();

            //If that product has been selected, modify the row, otherwise, add it to the order
            if (orderContents.Select("ProductID Like '" + productID + "'").FirstOrDefault() == null)
            {
                //Add the new product to the order
                DataRow row = orderContents.NewRow();
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

            //Redirect the user
            Response.Redirect("/Orders.aspx?sucessmessage=1");
        }

        /// <summary>
        /// This method handles the click event for the BtnSubmit button
        /// </summary>
        /// <param name="sender">The BtnSubmit button</param>
        /// <param name="e">The click event</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            CapstoneEntities entities = new CapstoneEntities();
            //Get the user
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByName(User.Identity.Name);

            //Create the order object
            Models.Order order = new Models.Order();
            order.OrderDate = Convert.ToDateTime(TxtOrderDate.Text);
            order.CustomerID = Convert.ToInt32(DDLCustomer.SelectedValue);
            order.LocationID = Convert.ToInt32(DDLLocation.SelectedValue);
            order.SalesRepID = user.Id;
            order.OrderTotal = 0.00m;

            //Save the order object
            entities.Orders.Add(order);
            entities.SaveChanges();

            decimal total = 0.00m;

            //Loop through and add all order lines
            foreach(DataRow row in orderContents.Rows)
            {
                //Add to the total
                total += Convert.ToDecimal(row["LineTotal"]);

                //Create the OrderLine object
                Models.OrderLine orderLine = new Models.OrderLine();
                orderLine.OrderID = order.OrderID;
                orderLine.ProductID = Convert.ToInt32(row["ProductID"]);
                orderLine.Quantity = Convert.ToInt32(row["Quantity"]);

                //Save the OrderLine object
                entities.OrderLines.Add(orderLine);
                entities.SaveChanges();
            }

            //Save the order total
            order.OrderTotal = total;
            entities.SaveChanges();
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

            //Get the row from the order contents table and remove it
            DataRow orderRow = orderContents.Select("ProductID Like '" + productID + "'").FirstOrDefault();
            orderContents.Rows.Remove(orderRow);

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
                CapstoneEntities context = new CapstoneEntities();
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
    }
}