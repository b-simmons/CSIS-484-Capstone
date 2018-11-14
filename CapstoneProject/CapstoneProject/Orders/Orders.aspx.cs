using CapstoneProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Orders
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Orders.aspx.cs
     */
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get all orders
            CapstoneEntities context = new CapstoneEntities();
            List<Models.Order> allOrders = context.Orders.ToList();

            //Populate the gridview
            GrOrders.DataSource = from order in allOrders
                                  select new
                                  {
                                      order.OrderID,
                                      order.OrderDate,
                                      order.OrderTotal,
                                      order.Customer.CustomerID,
                                      order.Customer.BusinessName,
                                      order.Location.LocationID,
                                      order.Location.Address,
                                      Shipped = (order.Shipments.Any(s => s.OrderID == order.OrderID) ? "Yes" : "No")
                                  };
            GrOrders.DataBind();

            if(!String.IsNullOrWhiteSpace(Request.QueryString["successmessage"]) && !IsPostBack)
            {
                int messageType = Convert.ToInt32(Request.QueryString["successmessage"]);
                DivSuccessMessage.Visible = true;
                switch(messageType)
                {
                    case 1:
                        LblSuccessMessage.Text = "Order successfully added!";
                        break;
                    case 2:
                        LblSuccessMessage.Text = "Order successfully edited!";
                        break;
                    case 3:
                        LblSuccessMessage.Text = "Order successfully deleted!";
                        break;
                    default:
                        LblSuccessMessage.Text = "Error";
                        break;
                }
            }
        }

        /// <summary>
        /// This method handles the click event for the BtnEditOrder button
        /// that is inside the GrOrders gridview.
        /// </summary>
        /// <param name="sender">The BtnEditOrder button</param>
        /// <param name="e">The click event</param>
        protected void BtnEditOrder_Click(object sender, EventArgs e)
        {
            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the order ID
            string orderID = GrOrders.DataKeys[row.RowIndex].Values["OrderID"].ToString();

            //Redirect the user
            Response.Redirect("/Orders/Order.aspx?orderid=" + orderID);
        }
    }
}