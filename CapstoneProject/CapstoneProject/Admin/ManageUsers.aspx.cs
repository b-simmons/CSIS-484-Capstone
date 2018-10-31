using CapstoneProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get all users
            var context = new ApplicationDbContext();
            List<ApplicationUser> allUsers = context.Users.ToList();

            GrUsers.DataSource = allUsers;
            GrUsers.DataBind();
            

            //Customer customer = new Customer();
            //CapstoneEntities entities = new CapstoneEntities();
            //entities.Locations.Add(new Location());
            //entities.SaveChanges(); // Save to DB
        }

        protected void GrUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}