using CapstoneProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Locations
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Locations.aspx.cs
     */
    public partial class Locations : System.Web.UI.Page
    {
        ApplicationUserManager manager;
        ApplicationUser user;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the user
            manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            user = manager.FindByName(User.Identity.Name);

            //Get all locations
            CapstoneEntities context = new CapstoneEntities();
            List<Models.Location> allLocations = context.Locations.ToList();

            //Populate the gridview
            GrLocations.DataSource = from location in allLocations
                                     select new
                                     {
                                         location.LocationID,
                                         location.CustomerID,
                                         location.Address,
                                         location.PhoneNumber,
                                         location.Customer.BusinessName
                                     };
            GrLocations.DataBind();

            //Show a success message if the query string indicates it
            if (!String.IsNullOrWhiteSpace(Request.QueryString["successmessage"]) && !IsPostBack)
            {
                int messageType = Convert.ToInt32(Request.QueryString["successmessage"]);
                DivSuccessMessage.Visible = true;
                switch (messageType)
                {
                    case 1:
                        LblSuccessMessage.Text = "Location successfully added!";
                        break;
                    case 2:
                        LblSuccessMessage.Text = "Location successfully edited!";
                        break;
                    case 3:
                        LblSuccessMessage.Text = "Location successfully deleted!";
                        break;
                    default:
                        LblSuccessMessage.Text = "Error";
                        break;
                }
            }
        }

        /// <summary>
        /// This method handles the click event for the BtnEditLocation button
        /// that is inside the GrLocations gridview.
        /// </summary>
        /// <param name="sender">The BtnEditLocation button</param>
        /// <param name="e">The click event</param>
        protected void BtnEditLocation_Click(object sender, EventArgs e)
        {
            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the location ID
            string locationID = GrLocations.DataKeys[row.RowIndex].Values["LocationID"].ToString();

            //Redirect the user
            Response.Redirect("/Locations/Location.aspx?locationid=" + locationID);
        }

        /// <summary>
        /// This method handles the click event for the BtnDeleteLocation button
        /// that is inside the GrLocations gridview.
        /// </summary>
        /// <param name="sender">The BtnDeleteLocation button</param>
        /// <param name="e">The click event</param>
        protected void BtnDeleteLocation_Click(object sender, EventArgs e)
        {
            CapstoneEntities context = new CapstoneEntities();

            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the location ID
            int locationID = Convert.ToInt32(GrLocations.DataKeys[row.RowIndex].Values["LocationID"]);

            //Get the location to delete
            Models.Location locationToDelete = context.Locations.Where(o => o.LocationID == locationID).FirstOrDefault();

            //Delete the location
            context.Locations.Remove(locationToDelete);
            context.SaveChanges();

            //Refresh the gridview
            List<Models.Location> allLocations = context.Locations.ToList();
            GrLocations.DataSource = from location in allLocations
                                     select new
                                     {
                                         location.LocationID,
                                         location.CustomerID,
                                         location.Address,
                                         location.PhoneNumber,
                                         location.Customer.BusinessName
                                     };
            GrLocations.DataBind();

            //Show success message
            DivSuccessMessage.Visible = true;
            LblSuccessMessage.Text = "Location successfully deleted!";
        }

        /// <summary>
        /// This method handles the RowDataBound event for the GrLocations gridview and ensures that 
        /// the DataTables JQuery plugin works properly.
        /// </summary>
        /// <param name="sender">The GrLocations gridview</param>
        /// <param name="e">The RowDataBound event</param>
        protected void GrLocations_RowDataBound(object sender, GridViewRowEventArgs e)
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
                LinkButton deleteButton = (LinkButton)e.Row.FindControl("BtnDeleteLocation");
                LinkButton editButton = (LinkButton)e.Row.FindControl("BtnEditLocation");

                string role = manager.GetRoles(user.Id).FirstOrDefault();

                if (role != "Admin")
                {
                    deleteButton.Visible = false;
                }

                //Get the location for this row
                int locationID = Convert.ToInt32(GrLocations.DataKeys[e.Row.RowIndex].Values["LocationID"]);
                Models.Location location = context.Locations.Where(o => o.LocationID == locationID).FirstOrDefault();

                //Don't allow deletion if the location is already on an order
                if (location.Orders.Count > 0)
                {
                    //Hide the delete button
                    deleteButton.Visible = false;
                }
            }
        }
    }
}