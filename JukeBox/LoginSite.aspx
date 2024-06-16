<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginSite.aspx.cs" Inherits="JukeBox.LoginSite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="stylesheet" href="LoginPageStyle.css" />
    <title>Login</title>
   
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2>Login</h2>
            <asp:TextBox ID="TxtUsername" runat="server" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
            <asp:Button ID="BtnLogin" runat="server" Text="Login" OnClick="BtnLogin_Click" CssClass="login-button" />
            <asp:Label ID="LblMessage" runat="server" CssClass="message"></asp:Label>
            <a href="RegisterSite.aspx" class="register-link">Don't have an account? Register</a>
        </div>
    </form>
</body>
</html>
