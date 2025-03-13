<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="WebApplication1.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        a {
            color: black;
            -webkit-text-decoration: none;
            text-decoration: none;
        }

        #resultMsg, #subResMsg {
            width: fit-content;
        }
        #resultMsg {
            margin-top: 50px;
            font-weight: 700;
            font-size: 50px;
        }
        #subResMsg {
            margin-top: 10px;
            font-size: 1.8em;
        }


        #results {
            width: 40%;
            margin-top: 20px;
        }


        #searchType {
            margin-top: 80px;
            margin-bottom: -15px;

            width: 40%;
            height: 60px;

            display: flex;
            -ms-flex-direction: row;
            -webkit-flex-direction: row;
            flex-direction: row;
        }
            #searchType > span {
                display: inline-block;
                text-align: center;
                vertical-align: central;
                line-height: 60px;

                -moz-box-shadow: 0 -2px 5px rgb(223, 223, 223);
                -webkit-box-shadow: 0 -2px 5px rgb(223, 223, 223);
                box-shadow: 0 -2px 5px rgb(223, 223, 223);

                cursor: pointer;

                font-weight: 600;
                font-size: 1.5em;
            }
                #searchType > span:not(.selected) {
                    position: relative;
                    bottom: 0;

                    height: 100%;
                    width: 50%;

                    -moz-border-radius: 20px 20px 5px 5px;
                    -webkit-border-radius: 20px 20px 5px 5px;
                    border-radius: 20px 20px 0 0;

                    -moz-transition: all ease-out .15s;
                    -o-transition: all ease-out .15s;
                    -webkit-transition: all ease-out .15s;
                    transition: all ease-out .15s;
                }
                #searchType > span:not(.selected):hover {
                    height: 105%;
                    bottom: 5%;
                }
            #searchType > span.selected {
                position: relative;
                bottom: 20%;
                height: 120%;
                width: 60%;
                background-color: rgb(186, 186, 186);

                -moz-border-radius: 30px 30px 5px 5px;
                -webkit-border-radius: 30px 30px 5px 5px;
                border-radius: 30px 30px 5px 5px;
            }
    </style>
    <%= stylesheets %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <p class="center" id="resultMsg"><%=resultMsg %></p>
    <p class="center" id="subResMsg"><%=subResMsg%></p>

    <div class="center" id="searchType">
        <span <%=((Request.QueryString["type"] == "profiles") || (Request.QueryString["type"] == null)) ? "class='selected'" : "" %>>Profiles</span>
        <span <%=(Request.QueryString["type"] == "posts") ? "class='selected'" : "" %>>Posts</span>
    </div>
    <script>
        const unselected = document.querySelector("#searchType > span:not(.selected)");
        unselected.addEventListener("click", () =>
            window.location.replace(`Search.aspx?type=${unselected.innerHTML.toLowerCase()}&term=<%=Request.QueryString["term"]%>`))
    </script>


    <div id="results" class="center">
        <%=results %>
    </div>
</asp:Content>
