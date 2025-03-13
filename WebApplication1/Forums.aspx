<%@ Page Title="JFS | Recent Posts" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Forums.aspx.cs" Inherits="WebApplication1.Forums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        a {
            color: black;
            -webkit-text-decoration: none;
            text-decoration: none;
        }
        h1 {
            width: max-content;
            font-size: 55px;
            margin-top: 55px !important;
            margin-bottom: 55px !important;

            -moz-user-select: none;
            -ms-user-select: none;
            -webkit-user-select: none;
            user-select: none;
        }

        #wrapper {
            width: 50%;
        }
    </style>
    <link href="Profile.css" rel="stylesheet" />
    <link href="Post.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div class="center" id="wrapper">
        <h1 class="center">Recent Posts</h1>

        <%=posts %>
    </div>
</asp:Content>
