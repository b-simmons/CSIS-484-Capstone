using CapstoneProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Customers
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Customers.aspx.cs
     */
    public partial class Customers : System.Web.UI.Page
    {
        ApplicationUserManager manager;
        ApplicationUser user;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the user
            manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            user = manager.FindByName(User.Identity.Name);

            //Get all customers
            CapstoneEntities context = new CapstoneEntities();
            List<Models.Customer> allCustomers = context.Customers.ToList();

            //Populate the gridview
            GrCustomers.DataSource = allCustomers;
            GrCustomers.DataBind();

            //Show a success message if the query string indicates it
            if (!String.IsNullOrWhiteSpace(Request.QueryString["successmessage"]) && !IsPostBack)
            {
                int messageType = Convert.ToInt32(Request.QueryString["successmessage"]);
                DivSuccessMessage.Visible = true;
                switch (messageType)
                {
                    case 1:
                        LblSuccessMessage.Text = "Customer successfully added!";
                        break;
                    case 2:
                        LblSuccessMessage.Text = "Customer successfully edited!";
                        break;
                    case 3:
                        LblSuccessMessage.Text = "Customer successfully deleted!";
                        break;
                    default:
                        LblSuccessMessage.Text = "Error";
                        break;
                }
            }
        }

        /// <summary>
        /// This method handles the click event for the BtnEditCustomer button
        /// that is inside the GrCustomers gridview.
        /// </summary>
        /// <param name="sender">The BtnEditCustomer button</param>
        /// <param name="e">The click event</param>
        protected void BtnEditCustomer_Click(object sender, EventArgs e)
        {
            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the customer ID
            string customerID = GrCustomers.DataKeys[row.RowIndex].Values["CustomerID"].ToString();

            //Redirect the user
            Response.Redirect("/Customers/Customer.aspx?customerid=" + customerID);
        }

        /// <summary>
        /// This method handles the click event for the BtnDeleteCustomer button
        /// that is inside the GrCustomers gridview.
        /// </summary>
        /// <param name="sender">The BtnDeleteCustomer button</param>
        /// <param name="e">The click event</param>
        protected void BtnDeleteCustomer_Click(object sender, EventArgs e)
        {
            CapstoneEntities context = new CapstoneEntities();

            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the customer ID
            int customerID = Convert.ToInt32(GrCustomers.DataKeys[row.RowIndex].Values["CustomerID"]);

            //Get the customer to delete
            Models.Customer customerToDelete = context.Customers.Where(o => o.CustomerID == customerID).FirstOrDefault();

            //Delete the customer
            context.Customers.Remove(customerToDelete);
            context.SaveChanges();

            //Refresh the gridview
            List<Models.Customer> allCustomers = context.Customers.ToList();
            GrCustomers.DataSource = allCustomers;
            GrCustomers.DataBind();

            //Show success message
            DivSuccessMessage.Visible = true;
            LblSuccessMessage.Text = "Customer successfully deleted!";
        }

        /// <summary>
        /// This method handles the RowDataBound event for the GrCustomers gridview and ensures that 
        /// the DataTables JQuery plugin works properly.
        /// </summary>
        /// <param name="sender">The GrCustomers gridview</param>
        /// <param name="e">The RowDataBound event</param>
        protected void GrCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CapstoneEntities context = new CapstoneEntities();

            //Set the header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Hide the delete button if not admin
                LinkButton deleteButton = (LinkButton)e.Row.FindControl("BtnDeleteCustomer");
                LinkButton editButton = (LinkButton)e.Row.FindControl("BtnEditCustomer");

                string role = manager.GetRoles(user.Id).FirstOrDefault();

                if (role != "Admin")
                {
                    deleteButton.Visible = false;
                }

                //Get the customer for this row
                int customerID = Convert.ToInt32(GrCustomers.DataKeys[e.Row.RowIndex].Values["CustomerID"]);
                Models.Customer customer = context.Customers.Where(o => o.CustomerID == customerID).FirstOrDefault();

                //Don't allow deletion if the customer is already on an order
                if (customer.Orders.Count > 0)
                {
                    //Hide the delete button
                    deleteButton.Visible = false;
                }
            }
        }
    }
}