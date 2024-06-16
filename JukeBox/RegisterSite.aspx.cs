using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JukeBox
{
    public partial class RegisterSite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void BtnBackToLogin_Click(object sender, EventArgs e) {
            Response.Redirect("LoginSite.aspx");
        }
        protected void BtnRegisterUser_Click(object sender, EventArgs e) {
            UserRegistration Registration = new UserRegistration();
            bool userExists = Registration.UserExistenceCheck(TxtRegisterUsername.Text, TxtRegisterEmail.Text);
            if (userExists) {
                LblError.Text = "User alrady exist";
            }
            else {
                if (TxtRegisterUsername.Text.Length <= 3 )
                {
                    LblError.Text = "Username needs to be atleast 3 chracters";
                    return;
                }
                if (TxtRegisterPassword.Text.Length <= 4)
                {
                    LblError.Text = "Password needs to be atleast 4 characters";
                    return;
                }

                if (string.IsNullOrEmpty(TxtRegisterEmail.Text.Trim()))
                {
                    LblError.Text = "Invalid Email";
                    return;
                }
                Registration.RegisterUser(TxtRegisterUsername.Text, TxtRegisterPassword.Text, TxtRegisterEmail.Text);
                LblError.Text = "User Sucesfully registered";
            }
        }
    }
}