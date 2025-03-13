<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Post.aspx.cs" Inherits="WebApplication1.Post" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        a.noA {
            color: black;
            -webkit-text-decoration: none;
            text-decoration: none;
        }

        #wrapper {
            width: 65%;
            margin-top: 100px;
            position: relative;
        }
        h1 {
            font-size: 40px;
            width: fit-content;
        }

        #post {
            position: relative;
            min-height: 250px;
        }
            #post > footer {
                position: absolute;
                bottom: 2em;
            }
            .profile {
                top: -15px;
            }

        p {
            font-size: 1.5em;
        }
        #content {
            max-width: 100%;
            -ms-word-wrap: break-word;
            word-wrap: break-word;
        }


        #delBtn {
            width: 250px;
            height: 50px;
            background-color: rgb(200, 0, 0);
            color: white;
            font-size: 1.2em;
            font-weight: 700;
            border: none;
            -moz-border-radius: 7px;
            -webkit-border-radius: 7px;
            border-radius: 7px;
            cursor: pointer;
        }
    </style>
    <link href="Profile.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

    <div class="center" id="wrapper">
        <article id="post">
            <a class="noA" href="Profile.aspx?beholder=<%=beholderId %>">
                <div class="profile">
                    <div class='pfp' style='background-image: url("<%= beholderPfp %>");'></div>
                    <p class='mainName'><%=beholderName %></p>
                    <%=beholderTag %>
                </div>
            </a>

            <h1 class="center"><%=articleTitle %></h1>
            <hr />

            <p id="content"><%=articleContent %></p>

            <footer>Posted on <time><%=articlePostTime %></time></footer>

            <div style="height: 100px;"></div>
        </article>

        <%=WebApplication1.DeletePost.Deletable(Session, beholderId) ?
                "<button onclick='deletePost()' id='delBtn'>Delete Post</button>"
        : ""%>
        <p id="warningMsg"></p>
    </div>
    <script>
        const button = document.getElementById("delBtn"), warningMsg = document.getElementById("warningMsg");
        function deletePost() {
            if (button.hasAttribute("clicked"))
                window.location.replace("DeletePost.aspx?id=<%=Request.QueryString["id"]%>");
            else {
                warningMsg.innerHTML = "Are you <b>sure</b> you want to <b>delete</b> this post?";
                button.setAttribute("clicked", "");
            }
        }
    </script>

</asp:Content>
