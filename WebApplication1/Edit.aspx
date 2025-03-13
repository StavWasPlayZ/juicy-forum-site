<%@ Page Title="JFS | Edit Profile" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="WebApplication1.Edit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        table {
            position: relative;
            top: 100px;

            width: 80%;
            margin: auto;
            font-size: 1.5em;
        }


        #menu {
            display: flex;
            -ms-flex-direction: column;
            -webkit-flex-direction: column;
            flex-direction: column;
            -webkit-align-items: end;
            align-items: end;

            width: 80%;
            height: calc(80vh - 60px)
        }
            #menu > div {
                width: max-content;
                padding: 10px;
                cursor: pointer;
                margin-bottom: 5px;
                margin-right: 10px;

                -moz-transition: background-color ease-out .1s, border-radius ease-out .1s;
                -o-transition: background-color ease-out .1s, border-radius ease-out .1s;
                -webkit-transition: background-color ease-out .1s, border-radius ease-out .1s;
                transition: background-color ease-out .1s, border-radius ease-out .1s;
            }
            #menu > div:hover {
                background-color: rgb(237, 237, 237);

                -moz-border-radius: 5px;
                -webkit-border-radius: 5px;
                border-radius: 5px;
            }
            #menu > div.selected {
                background-color: rgb(206, 206, 206);

                -moz-border-radius: 5px;
                -webkit-border-radius: 5px;
                border-radius: 5px;
            }


        #fields {
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
        }
            #fields > span {
                display: block;
                position: absolute;

                -moz-transform: scale(0);
                -ms-transform: scale(0);
                -o-transform: scale(0);
                -webkit-transform: scale(0);
                transform: scale(0);
                -moz-transition: transform, ease-out, .2s;
                -o-transition: transform, ease-out, .2s;
                -webkit-transition: transform, ease-out, .2s;
                transition: transform, ease-out, .2s;
            }
            #fields > span.selected {
                -moz-transform: scale(1);
                -ms-transform: scale(1);
                -o-transform: scale(1);
                -webkit-transform: scale(1);
                transform: scale(1);
            }
                #fields > span label {
                    margin-right: 7px !important;
                }
        #pfp {
            display: block;
            margin-top: 10px;

            border-radius: 45%;
            background-image: url('<%= WebApplication1.Utils.GetPfp(ProfileInformation["PfpURL"]) %>');
            width: 150px;
            padding-top: 150px
        }

        /* Taken from IdentificationForm. Chose to not make a fuss over a single style */
        #message {
            font-size: 1.2em;
            color: red;

            position: absolute;
            bottom: calc(-15px + 1.2em);
        }

        textarea {
            font-family: Arial;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <table>
        <tbody>
            <tr>
                <td id="menu">
                    <div>Profile</div>
                    <div>Personal Information</div>
                    <div>Data & Security</div>
                </td>
                <td style="width: 50%">
                    <form id="fields" method="post" runat="server" onsubmit="return validate('edit')" autocomplete="off">

                        <div style="position: absolute; bottom: -20px">
                            <input type="password" name="oPassword" id="oPassword" maxlength="64" label="Current Password:"/>
                            <input type="submit" name="sumbit" id="submit" value="Edit" style="margin-top: 5px;" />
                        </div>
                        <!-- Disables email autocomplete, generally -->
                        <input type="text" style="display: none">


                        <!-- Profile -->
                        <span>
                            <input type="text" name="uname" id="uname" maxlength="20" label="User Name:"/>
                            <br /><br />
                            <textarea name="description" id="description" maxlength="345" br label="Description:" style="max-height: 100px; max-width: 500px"
                            ><%= ProfileInformation["Description"] %></textarea>
                            <br /><br />

                            <input type="text" name="link1N" id="link1N" maxlength="20" label="Link 1:" placeholder="Preview Name" style="width: 100px;"
                                value="<%=GetLinkProperty(true, 0) %>"/>
                            <input type="text" name="link1" id="link1" maxlength="100" placeholder="Address"
                                value="<%=GetLinkProperty(false, 0) %>"/>
                            <br />
                            <input type="text" name="link2N" id="link2N" maxlength="20" label="Link 2:" placeholder="Preview Name" style="width: 100px;"
                                value="<%=GetLinkProperty(true, 1) %>"/>
                            <input type="text" name="link2" id="link2" maxlength="100" placeholder="Address"
                                value="<%=GetLinkProperty(false, 1) %>"/>
                            <br />
                            <input type="text" name="link3N" id="link3N" maxlength="20" label="Link 3:" placeholder="Preview Name" style="width: 100px;"
                                value="<%=GetLinkProperty(true, 2) %>"/>
                            <input type="text" name="link3" id="link3" maxlength="100" placeholder="Address"
                                value="<%=GetLinkProperty(false, 2) %>"/>
                            <br /><br />

                            <input type="text" name="pfpURL" id="pfpURL" maxlength="4000" label="Profile Picture URL:" value="<%=ProfileInformation["PfpURL"] %>"/>
                            <span class="pfp" id="pfp"></span>
                        </span>
                        <!-- Personal Information -->
                        <span>
                            <input type="text" name="fname" id="fname" maxlength="18" placeholder="First Name" label="Name:"/>
                            <input type="text" name="lname" id="lname" maxlength="18" placeholder="Last Name"/>
                            <br /><br />

                            <input type="email" name="email" id="email" maxlength="345"  label="Email:"/>
                            <br /><br />

                            <input type="date" name="birthdate" id="birthdate"  label="Birthdate:"/>
                            <br />
                            <span id="gender" label="Gender:" style="font-size: .8em">
                                <input type="radio" name="gender" id="male" value="male" label="Male" />
                                <input type="radio" name="gender" id="female" value="female" label="Female"/>
                                <input type="radio" name="gender" id="other" value="other" label="Other"/>
                            </span>
                        </span>
                        <!-- Data & Security -->
                        <span>
                            <input type="password" name="password" id="password" maxlength="64" label="New Password:"/>
                            <br />
                            <input type="password" name="cPassword" id="cPassword" maxlength="64" label="Confirm Password:"/>
                            <br /><br />

                            <%= IsAdmin() ?

                                "<select name='userType' id='userType' label='User Type:'>" +
                                    "<option value='blk'" +
                                        $"{(((string)ProfileInformation["UserType"] == "blk") ? "selected" : "")}>Blocked</option>" +
                                    "<option value='usr'" +
                                        $"{(((string)ProfileInformation["UserType"] == "usr") ? "selected" : "")}>User</option>" +
                                    "<option value='adm'" +
                                        $"{(IsAdmin((string)ProfileInformation["UserType"]) ? "selected" : "")}>Admin</option>" +
                                "</select>" +
                                "<br /><br />"

                            : ""%>

                            <input type="button" id="delete" value="Delete Account"/>
                            <br />
                            <span id="delConfirmation"></span>
                        </span>


                        <div id="message"><%=message %></div>
                    </form>
                </td>
        </tbody>
    </table>

    <script src="Validation.js"></script>
    <script src="Label.js"></script>

    <script>
        const delBtn = form.querySelector("#delete"), confirmMsg = form.querySelector("#delConfirmation");
        delBtn.addEventListener("click", () => {
            if (delBtn.hasAttribute("counter")) {
                const count = delBtn.getAttribute("counter");

                confirmMsg.innerHTML =
                    (count == '1') ? "Are you 100% sure you have intended to click twice on the above button?" :
                    (count == '2') ? "You do remember that there is no backing off, right? One more click and <i><%=name%></i> is fully gone... ;-;" <%= IsLoggedUser ? ":"+
                    "(count == '3') ? \"Will you not miss this... juicy website?\" :" +
                    "(count == '4') ? \"You dissapoint me, my child.\" :" +
                    "(count == '5') ? \"Alright. You're just a click away. I have given up on you long ago. Just do it. Click it once more.\" :" +
                    "\"Goodbye.\""
                : ": \"aight goodbye\""%>;

                if (count == "<%= IsLoggedUser ? "6" : "3"%>")
                    window.location.replace("Delete.aspx?beholder=<%=Beholder%>");
                else
                    delBtn.setAttribute("counter", parseInt(count) + 1);
            } else {
                delBtn.setAttribute("counter", 1);
                confirmMsg.innerHTML = "Are you sure you want to delete <%=IsLoggedUser ? "your" : $"<i>{name}</i>'s" %> utmost beloved account?"
            }
        });
    </script>

    <script>
        const menu = document.getElementById("menu"), fields = document.getElementById("fields"),
            aFields = Array.from(document.querySelectorAll("#fields > span")), aMenu = Array.from(menu.children);

        // Menu bar functionality
        function select(categoryNumber) {
            // Remove old selection (if made)
            const oldSelect = menu.querySelector(".selected");
            if (oldSelect != null) {
                oldSelect.classList.remove("selected");
                aFields[aMenu.indexOf(oldSelect)].classList.remove("selected");
            }

            // Apply new selection
            aMenu[categoryNumber].classList.add("selected");
            aFields[categoryNumber].classList.add("selected");

            // Reset deletion button
            delBtn.removeAttribute("counter");
            confirmMsg.innerHTML = "";
        }
        // Apply to all menu buttons
        aMenu.forEach((child) =>
            child.addEventListener("click", () => select(aMenu.indexOf(child)))
        );

        // Disable autocomplete
        inputs.forEach((input) => input.setAttribute("autocomplete", "off"));

    </script>
</asp:Content>
