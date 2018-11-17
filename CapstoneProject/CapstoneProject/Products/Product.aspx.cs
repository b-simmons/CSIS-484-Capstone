using CapstoneProject.Models;
using System;
using System.Linq;

namespace CapstoneProject.Products
{
    /* Benjamin Simmons
     * CSIS 484-D01
     * Capstone Project
     * Product.aspx.cs
     */
    public partial class Product : System.Web.UI.Page
    {
        //Variables for the Entity Framework context and the edit boolean
        private CapstoneEntities context = new CapstoneEntities();
        private bool isEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if edit
            if (!String.IsNullOrWhiteSpace(Request.QueryString["productid"]))
            {
                isEdit = true;
                LblPageTitle.Text = "Edit Product";
            }
            else
            {
                LblPageTitle.Text = "New Product";
            }
            

            if (!IsPostBack)
            {
                if (isEdit)
                {
                    //Get the product ID
                    int productID = Convert.ToInt32(Request.QueryString["productID"]);

                    //Get the product object
                    Models.Product product = context.Products.Where(o => o.ProductID == productID).FirstOrDefault();

                    //Set the input field values
                    TxtProductName.Text = product.ProductName;
                    TxtProductDescription.Text = product.Description;
                    TxtQuantity.Text = product.QuantityInStock.ToString();
                    TxtPrice.Text = product.Price.ToString();

                    //If product is on an order, prevent editing
                    if (product.OrderLines.Count > 0)
                    {
                        //Lock the controls
                        LockControls();
                    }
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
            Models.Product product;
           
            if (isEdit)
            {
                //Get the product object
                int productID = Convert.ToInt32(Request.QueryString["productid"]);
                product = context.Products.Where(o => o.ProductID == productID).FirstOrDefault();

                //Edit the information
                product.ProductName = TxtProductName.Text;
                product.Description = TxtProductDescription.Text;
                product.QuantityInStock = Convert.ToInt32(TxtQuantity.Text);
                product.Price = Convert.ToInt32(TxtPrice.Text);

                context.SaveChanges();
            }
            else
            {
                //Create the product object
                product = new Models.Product();
                product.ProductName = TxtProductName.Text;
                product.Description = TxtProductDescription.Text;
                product.QuantityInStock = Convert.ToInt32(TxtQuantity.Text);
                product.Price = Convert.ToInt32(TxtPrice.Text);

                //Save the product object
                context.Products.Add(product);
                context.SaveChanges();
            }

            //Redirect the user
            if (isEdit)
                //Edit success message
                Response.Redirect("/Products/Products.aspx?successmessage=2");
            else
                //Add success message
                Response.Redirect("/Products/Products.aspx?successmessage=1");
        }

        /// <summary>
        /// This methods locks all the controls on the page
        /// </summary>
        private void LockControls()
        {
            TxtProductName.Enabled = false;
            TxtQuantity.Enabled = false;
            TxtProductDescription.Enabled = false;
            TxtPrice.Enabled = false;
            BtnSubmit.Visible = false;
            LnkCancel.Text = "Close";
        }
    }
}