using CapstoneProject.Models;
using System;
using System.Linq;

namespace CapstoneProject.Customers
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Customer.aspx.cs
     */
    public partial class Customer : System.Web.UI.Page
    {
        //Variables for the Entity Framework context and the edit boolean
        private CapstoneEntities context = new CapstoneEntities();
        private bool isEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if edit
            if (!String.IsNullOrWhiteSpace(Request.QueryString["customerid"]))
            {
                isEdit = true;
                LblPageTitle.Text = "Edit Customer";
            }
            else
            {
                LblPageTitle.Text = "New Customer";
            }
            

            if (!IsPostBack)
            {
                if (isEdit)
                {
                    //Get the customer ID
                    int customerID = Convert.ToInt32(Request.QueryString["customerID"]);

                    //Get the customer object
                    Models.Customer customer = context.Customers.Where(o => o.CustomerID == customerID).FirstOrDefault();

                    //Set the input field values
                    TxtBusinessName.Text = customer.BusinessName;
                    TxtBillingAddress.Text = customer.BillingAddress;
                    TxtPhone.Text = customer.PhoneNumber;
                    TxtEmailAddress.Text = customer.EmailAddress;
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
            Models.Customer customer;
           
            if (isEdit)
            {
                //Get the customer object
                int customerID = Convert.ToInt32(Request.QueryString["customerid"]);
                customer = context.Customers.Where(o => o.CustomerID == customerID).FirstOrDefault();

                //Edit the information
                customer.BusinessName = TxtBusinessName.Text;
                customer.BillingAddress = TxtBillingAddress.Text;
                customer.PhoneNumber = TxtPhone.Text;
                customer.EmailAddress = TxtEmailAddress.Text;

                context.SaveChanges();
            }
            else
            {
                //Create the customer object
                customer = new Models.Customer();
                customer.BusinessName = TxtBusinessName.Text;
                customer.BillingAddress = TxtBillingAddress.Text;
                customer.PhoneNumber = TxtPhone.Text;
                customer.EmailAddress = TxtEmailAddress.Text;

                //Save the customer object
                context.Customers.Add(customer);
                context.SaveChanges();
            }

            //Redirect the user
            if (isEdit)
                //Edit success message
                Response.Redirect("/Customers/Customers.aspx?successmessage=2");
            else
                //Add success message
                Response.Redirect("/Customers/Customers.aspx?successmessage=1");
        }

        /// <summary>
        /// This methods locks all the controls on the page
        /// </summary>
        private void LockControls()
        {
            TxtBusinessName.Enabled = false;
            TxtPhone.Enabled = false;
            TxtBillingAddress.Enabled = false;
            TxtEmailAddress.Enabled = false;
            BtnSubmit.Visible = false;
            LnkCancel.Text = "Close";
        }
    }
}