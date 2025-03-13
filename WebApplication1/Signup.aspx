<%@ Page Title="JFS | Sign Up" Language="C#" MasterPageFile="~/IdentificationForm.master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebApplication1.Signup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="form" runat="server">
    <form id="identificationForm" formName="Sign Up" method="post" runat="server" class="center" onsubmit="return validate('signup')">

        <input type="text" name="fname" id="fname" maxlength="18" placeholder="First Name" required label="Name:"/>
        <input type="text" name="lname" id="lname" maxlength="18" placeholder="Last Name" required/>
        <br />
        <input type="text" name="uname" id="uname" maxlength="20" label="User Name:"/>
        <br /><br />

        <input type="email" name="email" id="email" maxlength="345" required label="Email:"/>
        <br /><br />

        <input type="password" name="password" id="password" maxlength="64" required label="Password:"/>
        <br />
        <input type="password" name="cPassword" id="cPassword" maxlength="64" required  label="Confirm Password:"/>
        <br /><br />

        <input type="date" name="birthdate" id="birthdate" required label="Birthdate:"/>
        <br />
        <span id="gender" label="Gender:" style="font-size: .8em">
            <input type="radio" name="gender" id="male" value="male" label="Male" required/>
            <input type="radio" name="gender" id="female" value="female" label="Female"/>
            <input type="radio" name="gender" id="other" value="other" label="Other"/>
        </span>
        <br />


        <input type="submit" name="sumbit" id="submit" value="Sign Up" style="margin-top: 5px;" />

        <div id="message"><%= message %></div>


        <div style="width: 100%; direction:rtl; margin-top: 5px;">
            <input type="button" onclick="window.location.href = 'Login.aspx'" value="Log In" />
        </div>

    </form>
</asp:Content>
