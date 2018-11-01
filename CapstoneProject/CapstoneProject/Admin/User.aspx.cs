using CapstoneProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CapstoneProject.Admin
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * User.aspx.cs
     */
    public partial class User : System.Web.UI.Page
    {
        private ApplicationUser user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide any error or success messages
            LtlErrorMessage.Text = "";
            LtlSuccessMessage.Text = "";

            ApplicationUserManager userManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            //Fill the role drop down list
            DDLRole.DataSource = roleManager.Roles.ToList();
            DDLRole.DataBind();

            //If the user id is in the query string, this is an edit
            if (!String.IsNullOrWhiteSpace(Request.QueryString["userid"]))
            {
                //Set the page title
                LblPageTitle.Text = "Edit User";

                //Get the user id and object
                string userID = Request.QueryString["userid"];
                user = userManager.FindById(userID);

                if (!IsPostBack)
                {
                    //Fill the form with the user info
                    FillForm(userID);
                }
            }
            else
            {
                //This is a new user, set the title and enable the username and password textboxes
                LblPageTitle.Text = "New User";
                TxtPassword.Enabled = true;
                TxtConfirmPassword.Enabled = true;
                TxtUsername.Enabled = true;

                //Enable password validators
                RFVPassword.Enabled = true;
                RFVConfirmPassword.Enabled = true;
                CVPassword.Enabled = true;
            }
        }

        /// <summary>
        /// The FillForm method fills all the controls with the appropriate
        /// user information from the database.
        /// </summary>
        /// <param name="id">The user's ID</param>
        private void FillForm(string id)
        {
            //Fill all the inputs
            TxtUsername.Text = user.UserName;
            TxtEmail.Text = user.Email;
            TxtPhone.Text = user.PhoneNumber;
            DDLRole.SelectedValue = user.Roles.First().RoleId.ToString();
            TxtPassword.Text = "NotEditable";
            TxtConfirmPassword.Text = "NotEditable";

            //Do not allow password or username edits
            TxtPassword.Enabled = false;
            TxtConfirmPassword.Enabled = false;
            TxtUsername.Enabled = false;

            //Disable password validators
            RFVPassword.Enabled = false;
            RFVConfirmPassword.Enabled = false;
            CVPassword.Enabled = false;
        }

        /// <summary>
        /// This method prevents changes to the form
        /// </summary>
        private void LockForm()
        {
            //Lock all the inputs
            TxtUsername.Enabled = false;
            TxtEmail.Enabled = false;
            TxtPhone.Enabled = false;
            DDLRole.Enabled = false;
            TxtPassword.Enabled = false;
            TxtConfirmPassword.Enabled = false;
            TxtUsername.Enabled = false;

            //Hide submit button and change cancel link text
            BtnSubmit.Visible = false;
            LnkCancel.Text = "Close Form";
        }

        /// <summary>
        /// Handles the click event for the BtnSubmit button and saves or creates
        /// the user.
        /// </summary>
        /// <param name="sender">The BtnSubmit control</param>
        /// <param name="e">The click event</param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            //Get the user manager and
            ApplicationUserManager manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (user == null || user.Id == null)
            {
                //Create the user object and set properties
                user = new ApplicationUser();
                user.UserName = TxtUsername.Text;
                user.Email = TxtEmail.Text;
                user.PhoneNumber = TxtPhone.Text;
                user.EmailConfirmed = true;

                //Create the user in the system
                IdentityResult addResult = manager.Create(user, TxtPassword.Text);

                //Check to see if the user was created
                if (addResult.Succeeded)
                {
                    //The user was created, add the user to the proper role
                    IdentityResult roleResult = manager.AddToRole(user.Id, DDLRole.SelectedItem.Text);

                    //Check to see if the role was applied and display success or failure message
                    if (roleResult.Succeeded)
                    {
                        LtlSuccessMessage.Text = "User Successfully added!";
                        LockForm();
                    }
                    else
                    {
                        LtlErrorMessage.Text = roleResult.Errors.FirstOrDefault();
                    }
                }
                else
                {
                    //The user could not be created, show error message
                    LtlErrorMessage.Text = addResult.Errors.FirstOrDefault();
                }
            }
            else
            {
                //Set the role for the user
                manager.RemoveFromRole(user.Id, user.Roles.First().ToString());
                manager.AddToRole(user.Id, DDLRole.SelectedItem.Text);

                //Set the phone number and email address
                user.PhoneNumber = TxtPhone.Text;
                user.Email = TxtEmail.Text;
                user.EmailConfirmed = true;

                //Update the user
                IdentityResult result = manager.Update(user);

                //Display a success or failure message
                if (result.Succeeded)
                {
                    LtlSuccessMessage.Text = "User successfully edited!";
                    LockForm();
                }
                else
                {
                    LtlErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }
        }
    }
}