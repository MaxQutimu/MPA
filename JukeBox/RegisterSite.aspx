<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterSite.aspx.cs" Inherits="JukeBox.RegisterSite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="stylesheet" href="RegisterPageStyle.css" />
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <h2>Register</h2>
            <asp:TextBox ID="TxtRegisterUsername" runat="server" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="TxtRegisterPassword" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
            <asp:TextBox ID="TxtRegisterEmail" runat="server" placeholder="Email"></asp:TextBox>
            <asp:Button ID="BtnRegisterUser" runat="server" Text="Register" OnClick="BtnRegisterUser_Click" CssClass="register-button" />
            <asp:Label ID="LblError" runat="server" CssClass="message"></asp:Label>
            <asp:Button ID="BtnBackToLogin" runat="server" Text="Back to Login" OnClick="BtnBackToLogin_Click" CssClass="back-button" />
        </div>
    </form>
</body>
</html>
