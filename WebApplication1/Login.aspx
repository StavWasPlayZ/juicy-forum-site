<%@ Page Title="JFS | Login" Language="C#" MasterPageFile="~/IdentificationForm.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="form" runat="server">
    <form id="identificationForm" formName="Log In" method="post" runat="server" class="center" onsubmit="return validate('login')">

        <input type="email" name="email" id="email" maxlength="345" label="Email:" required/>
        <br />
        <input type="password" name="password" id="password" maxlength="64" label="Password:" required/>
        <br />

        <input type="submit" name="check" id="submit" value="Log in" style="margin-top: 3px;" />

        <div id="message"><%= message %></div>


        <div style="width: 100%; direction:rtl; margin-top: 5px;">
            <input type="button" onclick="window.location.href = 'Signup.aspx'" value="Sign Up" />
        </div>

    </form>
</asp:Content>