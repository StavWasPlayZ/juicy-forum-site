using System;
using System.Collections.Generic;
using System.Web.SessionState;

namespace WebApplication1
{
    public static class Utils
    {
        // Session methods
        public static void SetBasicUserSession(HttpSessionState session, object id, object pfp, object userType, object userName)
        {
            session["uId"] = id;
            session["uPfp"] = pfp;
            session["UserType"] = userType;
            session["UserName"] = userName;
        }
        public static void ResetUserSession(HttpSessionState session) =>
            SetBasicUserSession(session, null, null, null, "Guest");


        // Profile methods
        public static string GetPfp(object sessionPfp) =>
            ((sessionPfp is DBNull) || (sessionPfp == null) || ((string)sessionPfp == "")) ? "blankpfp.svg" : (string)sessionPfp;

        /// <summary>
        /// Gets either the username or name of the specified user;
        /// depending on whether one has it.
        /// </summary>
        /// <param name="primary">Whether the atucal name of the user should be returned</param>
        /// <param name="name">The name of the user</param>
        /// <param name="userName">The username of the user</param>
        /// <returns></returns>
        public static string GetName(bool primary, object name, object userName)
        {
            bool isUNameEmpty = (userName is DBNull) || ((string)userName == "");

            return (string)(
                primary
                    ? (isUNameEmpty ? name : userName)
                    : (isUNameEmpty ? "" : name)
            );
        }
        public static string GetName(bool primary, Dictionary<string, object> profileInformation) =>
            GetName(primary, profileInformation["Name"], profileInformation["UserName"]);

        public static string GetRole(string role)
        {
            string backgroundColor = null, text = null;
            switch (role)
            {
                case "adm":
                    backgroundColor = "#caca00"; text = "Administrator";
                    break;
                case "cre":
                    backgroundColor = "#ac3535"; text = "Almighty Creator";
                    break;
            }

            return (text != null) ?
                $"<div class='role' style='background-color: {backgroundColor}; padding: 3px; border-radius: 3px;'>{text}</div>" : "";
        }

        public static Link[] GetLinks(Dictionary<string, object> profileInformation)
        {
            object rawLinks = profileInformation["Links"];
            if ((rawLinks == null) || (rawLinks is DBNull) || ((string)rawLinks == ""))
                return null;

            string[] links = ((string)rawLinks).Split(' ');
            // The database saves links as follows:
            // link1 link1-name link2 link2-name link3 link3-name
            // Thus, the number of links present is 2 times less the amount of spaces
            Link[] result = new Link[links.Length / 2];
            for (int i = 0; i < links.Length; i += 2)
                result[i / 2] = new Link(links[i + 1], links[i]);

            return result;
        }
    }

    public struct Link
    {
        readonly string name;
        public string Name { get => name; }
        public string Url { get; private set; }
        public Link(string name, string url)
        {
            this.name = name.Replace("%20", " ");
            Url = url;
        }
    }
}