using CapstoneProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Admin
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * ManageUsers.aspx.cs
     */
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get all users
            var context = new ApplicationDbContext();
            List<ApplicationUser> allUsers = context.Users.ToList();

            //Populate the gridview
            GrUsers.DataSource = allUsers;
            GrUsers.DataBind();
        }

        /// <summary>
        /// This method handles the RowDataBound event for the GrUsers gridview and ensures that 
        /// the DataTables JQuery plugin works properly.
        /// </summary>
        /// <param name="sender">The GrUsers gridview</param>
        /// <param name="e">The RowDataBound event</param>
        protected void GrUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Set the header
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }

        /// <summary>
        /// This method handles the click event for the BtnEditUser button
        /// that is inside the GrUsers gridview.
        /// </summary>
        /// <param name="sender">The BtnEditUser button</param>
        /// <param name="e">The click event</param>
        protected void BtnEditUser_Click(object sender, EventArgs e)
        {
            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the user ID
            string id = GrUsers.DataKeys[row.RowIndex].Values["Id"].ToString();

            //Redirect the user
            Response.Redirect("/Admin/User.aspx?userid=" + id);
        }
    }
}