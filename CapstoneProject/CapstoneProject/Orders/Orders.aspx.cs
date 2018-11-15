﻿using CapstoneProject.Models;
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

            //Show a success message if the query string indicates it
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

        /// <summary>
        /// This method handles the click event for the BtnDeleteOrder button
        /// that is inside the GrOrders gridview.
        /// </summary>
        /// <param name="sender">The BtnDeleteOrder button</param>
        /// <param name="e">The click event</param>
        protected void BtnDeleteOrder_Click(object sender, EventArgs e)
        {
            CapstoneEntities context = new CapstoneEntities();

            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the order ID
            int orderID = Convert.ToInt32(GrOrders.DataKeys[row.RowIndex].Values["OrderID"]);

            //Get the order to delete
            Models.Order orderToDelete = context.Orders.Where(o => o.OrderID == orderID).FirstOrDefault();

            //Delete any order lines and shipments
            foreach(Models.OrderLine line in orderToDelete.OrderLines)
            {
                context.OrderLines.Remove(line);
            }
            foreach(Models.Shipment shipment in orderToDelete.Shipments)
            {
                context.Shipments.Remove(shipment);
            }
            context.SaveChanges();

            //Remove the order
            context.Orders.Remove(orderToDelete);
            context.SaveChanges();

            //Refresh the gridview
            List<Models.Order> allOrders = context.Orders.ToList();
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

            //Show success message
            DivSuccessMessage.Visible = true;
            LblSuccessMessage.Text = "Order successfully deleted!";
        }

        /// <summary>
        /// This method handles the RowDataBound event for the GrOrders gridview and ensures that 
        /// the DataTables JQuery plugin works properly.
        /// </summary>
        /// <param name="sender">The GrOrders gridview</param>
        /// <param name="e">The RowDataBound event</param>
        protected void GrOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Set the header
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}