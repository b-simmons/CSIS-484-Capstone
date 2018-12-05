using CapstoneProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CapstoneProject.Products
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Products.aspx.cs
     */
    public partial class Products : System.Web.UI.Page
    {
        ApplicationUserManager manager;
        ApplicationUser user;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get the user and role
            manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            user = manager.FindByName(User.Identity.Name);
            string role = manager.GetRoles(user.Id).FirstOrDefault();

            //Get all products
            CapstoneEntities context = new CapstoneEntities();
            List<Models.Product> allProducts = context.Products.ToList();

            //Populate the gridview
            GrProducts.DataSource = allProducts;
            GrProducts.DataBind();

            //Hide the new product button if the user is not an admin
            if (role != "Admin")
                BtnNewProduct.Visible = false;

            //Show a success message if the query string indicates it
            if (!String.IsNullOrWhiteSpace(Request.QueryString["successmessage"]) && !IsPostBack)
            {
                int messageType = Convert.ToInt32(Request.QueryString["successmessage"]);
                DivSuccessMessage.Visible = true;
                switch (messageType)
                {
                    case 1:
                        LblSuccessMessage.Text = "Product successfully added!";
                        break;
                    case 2:
                        LblSuccessMessage.Text = "Product successfully edited!";
                        break;
                    case 3:
                        LblSuccessMessage.Text = "Product successfully deleted!";
                        break;
                    default:
                        LblSuccessMessage.Text = "Error";
                        break;
                }
            }
        }

        /// <summary>
        /// This method handles the click event for the BtnEditProduct button
        /// that is inside the GrProducts gridview.
        /// </summary>
        /// <param name="sender">The BtnEditProduct button</param>
        /// <param name="e">The click event</param>
        protected void BtnEditProduct_Click(object sender, EventArgs e)
        {
            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the product ID
            string productID = GrProducts.DataKeys[row.RowIndex].Values["ProductID"].ToString();

            //Redirect the user
            Response.Redirect("/Products/Product.aspx?productid=" + productID);
        }

        /// <summary>
        /// This method handles the click event for the BtnDeleteProduct button
        /// that is inside the GrProducts gridview.
        /// </summary>
        /// <param name="sender">The BtnDeleteProduct button</param>
        /// <param name="e">The click event</param>
        protected void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            CapstoneEntities context = new CapstoneEntities();

            //Get the Gridview row
            GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;

            //Get the product ID
            int productID = Convert.ToInt32(GrProducts.DataKeys[row.RowIndex].Values["ProductID"]);

            //Get the product to delete
            Models.Product productToDelete = context.Products.Where(o => o.ProductID == productID).FirstOrDefault();

            //Delete the product
            context.Products.Remove(productToDelete);
            context.SaveChanges();

            //Refresh the gridview
            List<Models.Product> allProducts = context.Products.ToList();
            GrProducts.DataSource = allProducts;
            GrProducts.DataBind();

            //Show success message
            DivSuccessMessage.Visible = true;
            LblSuccessMessage.Text = "Product successfully deleted!";
        }

        /// <summary>
        /// This method handles the RowDataBound event for the GrProducts gridview and ensures that 
        /// the DataTables JQuery plugin works properly.
        /// </summary>
        /// <param name="sender">The GrProducts gridview</param>
        /// <param name="e">The RowDataBound event</param>
        protected void GrProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CapstoneEntities context = new CapstoneEntities();

            //Set the header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Hide the delete and edit buttons if not admin
                LinkButton deleteButton = (LinkButton)e.Row.FindControl("BtnDeleteProduct");
                LinkButton editButton = (LinkButton)e.Row.FindControl("BtnEditProduct");

                string role = manager.GetRoles(user.Id).FirstOrDefault();

                if (role != "Admin")
                {
                    deleteButton.Visible = false;
                    editButton.Visible = false;
                }

                //Get the product for this row
                int productID = Convert.ToInt32(GrProducts.DataKeys[e.Row.RowIndex].Values["ProductID"]);
                Models.Product product = context.Products.Where(o => o.ProductID == productID).FirstOrDefault();

                //Don't allow deletion or editing if the product is already on an order
                if (product.OrderLines.Count > 0)
                {
                    editButton.Visible = false;
                    deleteButton.Visible = false;
                    Label orderLabel = (Label)e.Row.FindControl("LblOnOrder");
                    orderLabel.Visible = true;
                }
            }
        }
    }
}