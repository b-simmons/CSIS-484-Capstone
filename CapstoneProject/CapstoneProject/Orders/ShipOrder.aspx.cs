using CapstoneProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
     * ShipOrder.aspx.cs
     */
    public partial class ShipOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get all orders
            CapstoneEntities context = new CapstoneEntities();
            List<Models.Order> allOrders = context.Orders.ToList();

            //Populate the gridview
            PopulateOrdersGrid();

            //Set validator
            CVShipmentDateFuture.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        }

        /// <summary>
        /// This method handles the click event for the BtnSubmit button
        /// </summary>
        /// <param name="sender">The BtnSubmit button</param>
        /// <param name="e">The click event</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            //Get the user
            ApplicationUserManager manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = manager.FindByName(User.Identity.Name);

            //Get the order object
            CapstoneEntities context = new CapstoneEntities();
            int orderID = Convert.ToInt32(HFOrderID.Value);
            Models.Order shippedOrder = context.Orders.Where(o => o.OrderID == orderID).FirstOrDefault();

            //Determine if add or edit
            if (shippedOrder.Shipments.Count == 0)
            {
                //Add the shipment
                Models.Shipment shipment = new Models.Shipment();
                shipment.ShipmentDate = Convert.ToDateTime(TxtShipmentDate.Text);
                shipment.ShippingServiceName = TxtShippingService.Text;
                shipment.TrackingNumber = TxtTrackingNum.Text;
                shipment.OrderID = shippedOrder.OrderID;
                shipment.ShippingClerkID = user.Id;
                shippedOrder.Shipments.Add(shipment);

                //Set success message
                LblSuccessMessage.Text = "Shipment successfully added!";
            }
            else
            {
                //Edit the shipment
                Models.Shipment shipment = shippedOrder.Shipments.FirstOrDefault();
                shipment.ShipmentDate = Convert.ToDateTime(TxtShipmentDate.Text);
                shipment.ShippingServiceName = TxtShippingService.Text;
                shipment.TrackingNumber = TxtTrackingNum.Text;
                shipment.OrderID = shippedOrder.OrderID;
                shipment.ShippingClerkID = user.Id;

                //Set success message
                LblSuccessMessage.Text = "Shipment successfully edited!";
            }

            //Save to DB
            context.SaveChanges();

            //Show success message
            DivSuccessMessage.Visible = true;

            //Clear the details
            ClearDetails();
            DivDetails.Visible = false;

            //Refresh the orders gridview
            PopulateOrdersGrid();
        }

        /// <summary>
        /// This method handles the click event for the BtnSelectOrder LinkButton
        /// </summary>
        /// <param name="sender">The BtnSelectOrder LinkButton</param>
        /// <param name="e">The click event</param>
        protected void BtnSelectOrder_Click(object sender, EventArgs e)
        {
            //Get the gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the order ID
            int orderID = Convert.ToInt32(GrOrders.DataKeys[row.RowIndex].Values["OrderID"]);

            //Fill the order and shipment details
            PopulateDetails(orderID);

            DivDetails.Visible = true;
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
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        /// <summary>
        /// This method handles the click event for the BtnCancel button
        /// </summary>
        /// <param name="sender">The BtnCancel button</param>
        /// <param name="e">The click event</param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearDetails();
            DivDetails.Visible = false;
        }

        /// <summary>
        /// This method fills the order and shipment details
        /// </summary>
        /// <param name="orderID">The order's ID</param>
        private void PopulateDetails(int orderID)
        {
            //Get the order object
            CapstoneEntities context = new CapstoneEntities();
            Models.Order order = context.Orders.Where(o => o.OrderID == orderID).FirstOrDefault();
            HFOrderID.Value = order.OrderID.ToString();

            //Fill the labels
            LblOrderDate.Text = order.OrderDate.ToString("MM/dd/yy");
            LblCustomer.Text = order.Customer.BusinessName;
            LblLocation.Text = order.Location.Address;
            LblLocationContactNum.Text = order.Location.PhoneNumber;

            //Fill the order contents
            GrOrderContents.DataSource = from line in order.OrderLines
                                  select new
                                  {
                                      line.OrderLineID,
                                      line.ProductID,
                                      line.Product.ProductName,
                                      line.Quantity,
                                      LineTotal = line.Quantity * line.Product.Price
                                  };
            GrOrderContents.DataBind();

            //Fill the shipment details (if any)
            if(order.Shipments.Count > 0)
            {
                Models.Shipment shipment = order.Shipments.FirstOrDefault();
                TxtShipmentDate.Text = shipment.ShipmentDate.ToString("MM/dd/yy");
                TxtShippingService.Text = shipment.ShippingServiceName;
                TxtTrackingNum.Text = shipment.TrackingNumber;

                BtnRemoveShipment.Visible = true;
            }
            else
            {
                BtnRemoveShipment.Visible = false;
            }
        }

        /// <summary>
        /// This method clears the order details from the screen
        /// </summary>
        private void ClearDetails()
        {
            LblOrderDate.Text = "";
            LblCustomer.Text = "";
            LblLocation.Text = "";
            LblLocationContactNum.Text = "";
            GrOrderContents.DataSource = null;
            GrOrderContents.DataBind();
            TxtShipmentDate.Text = "";
            TxtShippingService.Text = "";
            TxtTrackingNum.Text = "";
            HFOrderID.Value = "0";
        }

        /// <summary>
        /// This method handles the click event for the BtnRemoveShipment button
        /// </summary>
        /// <param name="sender">The BtnRemoveShipment button</param>
        /// <param name="e">The click event</param>
        protected void BtnRemoveShipment_Click(object sender, EventArgs e)
        {
            //Get the order object
            CapstoneEntities context = new CapstoneEntities();
            int orderID = Convert.ToInt32(HFOrderID.Value);
            Models.Order shippedOrder = context.Orders.Where(o => o.OrderID == orderID).FirstOrDefault();

            //Remove the shipment
            Models.Shipment shipment = shippedOrder.Shipments.FirstOrDefault();
            context.Shipments.Remove(shipment);
            context.SaveChanges();

            //Reset the inputs
            ClearDetails();
            DivDetails.Visible = false;

            //Show success message
            LblSuccessMessage.Text = "Shipment successfully removed!";
            DivSuccessMessage.Visible = true;

            //Refresh the orders gridview
            PopulateOrdersGrid();
        }

        /// <summary>
        /// This method fills the orders gridview
        /// </summary>
        private void PopulateOrdersGrid()
        {
            //Populate the orders gridview
            CapstoneEntities context = new CapstoneEntities();
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
        }
    }
}