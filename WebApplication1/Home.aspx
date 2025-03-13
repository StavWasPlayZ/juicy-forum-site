<%@ Page Title="JFS | Juicy Forum Site" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .photo {
            background-size: cover;
        }
        #startPhoto {
            filter: invert(100%);
            background-image: url("yay.png");
            background-size: 10%;
            width: 100%;
            height: 1080px;
        }
        #sellingPoint {
            background-image: url("woo.png");
            width: 100%;
            height: 40%;
            background-repeat: no-repeat;
            background-position: center;
            background-size: contain;
            position: relative;
            top: calc(var(--bar-height) + 10px);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <section>
        <div id="startPhoto" class="photo">
            <div id="sellingPoint" class="photo"></div>
        </div>
    </section>
</asp:Content>
