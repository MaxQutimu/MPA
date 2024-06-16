using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace JukeBox
{
    public partial class LoginSite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            UserAuthentication auth = new UserAuthentication();
            int? userId = auth.Login_In(TxtUsername.Text, TxtPassword.Text);

            if (userId != null)
            {
                Session["User_ID"] = userId.Value; // Set the session variable
                Response.Redirect("Homepage.aspx"); // Redirect to the song page
            }
            else
            {
                LblMessage.Text = "Invalid username or password";
            }
        }
        protected void BtnRegister_Click(Object sender, EventArgs e)
        {
            Response.Redirect("RegisterSite.aspx");
        }

    }
}