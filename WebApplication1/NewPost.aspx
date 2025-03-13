<%@ Page Title="JFS | New Post" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="NewPost.aspx.cs" Inherits="WebApplication1.NewPost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #wrapper {
            width: 50%;
        }
        h1 {
            width: max-content;
            margin-top: 55px !important;
            margin-bottom: 60px !important;
            font-size: 50px;

            pointer-events: none;
            -moz-user-select: none;
            -ms-user-select: none;
            -webkit-user-select: none;
            user-select: none;
        }

        form > input, form > textarea {
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            border-radius: 5px;
            border: 1px solid gray;
        }
        label {
            font-size: 2em;
        }
        #title {
            font-size: 1.8em;
            width: 100%;
            text-align: center;
            margin-bottom: 10px;

            padding: 5px 0 5px;
        }
        textarea {
            max-width: 100%;
            min-width: 100%;

            font-family: Arial;
            font-size: 1.4em;
            height: 28em;

            margin-bottom: 10px;
        }

        #submit {
            width: 100%;
            height: 2em;

            border: none;
            background-color: rgb(207, 207, 207);

            font-size: 1.7em;

            -moz-transition: background-color linear .05s;
            -o-transition: background-color linear .05s;
            -webkit-transition: background-color linear .05s;
            transition: background-color linear .05s;
        }
        #submit:hover {
            background-color: rgb(170, 170, 170);
        }
        #submit:active {
            background-color: rgb(147, 147, 147);
        }

        #errMsg {
            width: max-content;
            margin-top: 10px;

            color: red;
            font-size: 1.5em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div class="center" id="wrapper">
        <h1 class="center">Ready To Impact?</h1>

        <form id="postForm" method="post" runat="server" onsubmit="return check()">
            <label for="title">Title:</label>
            <br />
            <input type="text" id="title" name="title" placeholder="Is it illegal to kill an ant????????" maxlength="200"/>

            <label for="content">Content:</label>
            <br />
            <textarea id="content" name="content" maxlength="4000"
                placeholder="Today I was walking around a park and I saw a puppy and it had a ant on it, so i stepped on the ant and killed it. But than a police officer gave me a dirty look and a bunch of other people glared at me so IS IT LEGAL TO KILL AN ANT? im serious guys, could i be arrested?"></textarea>

            <input type="submit" id="submit" name="submit" value="Post"/>
        </form>

        <div class="center" id="errMsg"></div>
    </div>


    <script>
        const form = document.getElementById("postForm"), title = form.querySelector("#title"), content = form.querySelector("#content"),
            errMsg = document.getElementById("errMsg");

        function check() {
            if (title.value.includes("<") || title.value.includes(">")) {
                throwError(title);
                return false;
            }
            if (content.value.includes("<") || content.value.includes(">")) {
                throwError(content);
                return false;
            }
            return true;
        }

        let prevElement;
        function throwError(element) {
            if (prevElement != undefined)
                prevElement.style.border = "";

            prevElement = element;


            element.style.border = "2px solid red";
            element.focus();

            errMsg.innerHTML = "A field cannot include the '<' or '>' symbols."
        }
    </script>
</asp:Content>
