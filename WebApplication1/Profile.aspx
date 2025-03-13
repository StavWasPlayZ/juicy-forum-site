<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication1.Profile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        a {
            color: black;
            -webkit-text-decoration: none;
            text-decoration: none;
        }

        #twrapper {
            width: 100vw;
            text-align: center;
        }
        table {
            margin: auto;
            width: 80%;
            text-align: center;

            font-size: 1.2rem;
        }

        table > tbody > tr > td {
            vertical-align: top;
        }
        table > tbody > tr > td:first-child {
            width: 40%;
            text-align: left;
        }
        table > tbody > tr > td:last-child {
            width: 60%;
            text-align: start;
        }


        #profileholder {
            display: flex;
            flex-direction: column;
            align-items: center;
            width: 80%;

            margin-top: 50px;
            padding: 10px 0;

            background-color: gray;

            -moz-border-radius: 10px 10px 0 0;
            -webkit-border-radius: 10px 10px 0 0;
            border-radius: 10px 10px 0 0;

        }

        #mainName {
            font-size: 1.6em;
            margin-bottom: 0;
        }
        #secondName {
            margin-top: 0;
            font-size: 1em;
        }
        #mainName, #secondName {
            text-align: center;
        }
        .beginner {
            text-align: left;
            width: 85%;
        }
        .linkIco {
            top: 3px;
            position: relative;
            margin-right: 5px;
        }

        .controlBtn {
            width: 80%;
            margin-top: 5px;
            height: 50px;
            font-size: 1.5rem;
            cursor: pointer;

            transition: border,background-color ease-out, .2s;
            -moz-transition: border,background-color ease-out, .2s;
            -o-transition: border,background-color ease-out, .2s;
            -webkit-transition: border,background-color ease-out, .2s;
        }
        #editBtn {
            border: 6px solid #404040;
            background-color: white;
        }
        #editBtn:hover {
            border: 3px dashed white;
            background-color: #404040;
            font-size: 1.8rem;
            color: white;
        }
        #logoutBtn {
            border: 0 solid white;
            background-color: #c21919;
        }
        #logoutBtn:hover {
            background-color: #b42424;
            border: 3px solid white;
            font-size: 1.4rem;
        }

        #lPosts {
            margin-top: 75px;
        }
        #lPosts > div {
            width: 100%;
        }

        .forum {
            text-align: left;
        }
    </style>

    <link href="Profile.css" rel="stylesheet" />
    <link href="Post.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div id="twrapper">
        <table>
            <tbody>
                <tr>
                    <td>
                        <div id="profileholder">
                            <div class="pfp" style="background-image: url('<%= WebApplication1.Utils.GetPfp(ProfileInformation["PfpURL"]) %>'); width: 80%; padding-top: 80%"></div>
                            <p id="mainName"><%= GetName(true) %></p>
                            <p id="secondName"><%= (GetName(false) == "") ? "" : $"AKA <i>{GetName(false)}</i>" %></p>
                            <%= WebApplication1.Utils.GetRole((string)ProfileInformation["UserType"]) %>
                            <div class="beginner">
                                <hr/>
                                <p>Member since <%= ((DateTime)ProfileInformation["JoinDate"]).ToString("dd/MM/yyyy") %></p>
                                <div id="links" style="margin-bottom: 10px">
                                    <h2 style="margin-bottom: 5px;">Links</h2>
                                    <%= GetLinksNotation() %>
                                </div>
                            </div>
                        </div>

                        <!-- TODO integer ranks -->
                        <%= (IsLoggedUser || ((string)Session["UserType"] == "adm") || ((string)Session["UserType"] == "cre")) ?
                                "<button id='editBtn' class='controlBtn'>Edit</button>" : "" %>
                        <script>
                            const editBtn = document.getElementById("editBtn");
                            if (editBtn != undefined)
                                editBtn.addEventListener("click", () => window.location.href = "Edit.aspx<%= IsLoggedUser ? "" : "?Beholder="+Beholder %>");
                        </script>
                        <%= IsLoggedUser ? "<button onclick=\"window.location.href = 'Logout.aspx'\" id='logoutBtn' class='controlBtn'>Log Out</button>" : "" %>
                    </td>
                    <td style="text-align: center">
                        <h1>About Me</h1>
                        <p><%=
                            ((ProfileInformation["Description"] is DBNull) || ((string)ProfileInformation["Description"] == "")) ?
                                "<i>Not a very descriptive person of sorts.</i>"
                                : ProfileInformation["Description"]
                        %></p>

                        <h1 id="lPosts">Latest Posts</h1>
                        <div>
                            <%=posts %>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div style="height: 50px;"></div>
</asp:Content>
