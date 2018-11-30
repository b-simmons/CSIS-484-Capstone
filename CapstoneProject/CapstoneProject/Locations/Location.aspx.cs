using CapstoneProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneProject.Locations
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Location.aspx.cs
     */
    public partial class Location : System.Web.UI.Page
    {
        //Variables for the Entity Framework context and the edit boolean
        private CapstoneEntities context = new CapstoneEntities();
        private bool isEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if edit
            if (!String.IsNullOrWhiteSpace(Request.QueryString["locationid"]))
            {
                isEdit = true;
                LblPageTitle.Text = "Edit Location";
            }
            else
            {
                LblPageTitle.Text = "New Location";
            }
            

            if (!IsPostBack)
            {
                //Fill the customer drop down
                List<Models.Customer> allCustomers = context.Customers.ToList();
                DDLCustomer.DataSource = allCustomers;
                DDLCustomer.DataBind();

                if (isEdit)
                {
                    //Get the location ID
                    int locationID = Convert.ToInt32(Request.QueryString["locationID"]);

                    //Get the location object
                    Models.Location location = context.Locations.Where(o => o.LocationID == locationID).FirstOrDefault();

                    //Set the input field values
                    DDLCustomer.SelectedIndex = location.CustomerID;
                    TxtAddress.Text = location.Address;
                    TxtPhone.Text = location.PhoneNumber;
                }
            }
        }

        /// <summary>
        /// This method handles the click event for the BtnSubmit button
        /// </summary>
        /// <param name="sender">The BtnSubmit button</param>
        /// <param name="e">The click event</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            Models.Location location;
           
            if (isEdit)
            {
                //Get the location object
                int locationID = Convert.ToInt32(Request.QueryString["locationid"]);
                location = context.Locations.Where(o => o.LocationID == locationID).FirstOrDefault();

                //Edit the information
                location.CustomerID = DDLCustomer.SelectedIndex;
                location.Address = TxtAddress.Text;
                location.PhoneNumber = TxtPhone.Text;

                context.SaveChanges();
            }
            else
            {
                //Create the location object
                location = new Models.Location();
                location.CustomerID = DDLCustomer.SelectedIndex;
                location.Address = TxtAddress.Text;
                location.PhoneNumber = TxtPhone.Text;

                //Save the location object
                context.Locations.Add(location);
                context.SaveChanges();
            }

            //Redirect the user
            if (isEdit)
                //Edit success message
                Response.Redirect("/Locations/Locations.aspx?successmessage=2");
            else
                //Add success message
                Response.Redirect("/Locations/Locations.aspx?successmessage=1");
        }

        /// <summary>
        /// This methods locks all the controls on the page
        /// </summary>
        private void LockControls()
        {
            TxtAddress.Enabled = false;
            TxtPhone.Enabled = false;
            DDLCustomer.Enabled = false;
            BtnSubmit.Visible = false;
            LnkCancel.Text = "Close";
        }
    }
}