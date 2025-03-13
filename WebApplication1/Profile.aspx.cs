using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class Profile : UserPage
    {
        protected override bool BeholderBeLogged => false;
        protected override string[] Fields => new string[]
        {
            "UserName", "Name", "JoinDate", "Description", "Links", "UserType", "PfpURL"
        };

        protected string posts = "";

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            Title = "JFS | " + GetName(true) + "'s Profile";


            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            DataTable data = DatabaseAccessor.GetTable($"select top 15 * from {DatabaseAccessor.POSTS_DB_NAME} where UserId='{Beholder}' order by PostDate desc",
                connection);

            for (int i = 0; i < data.Rows.Count; i++)
                posts += Forums.GetForumNotation(data.Rows[i], 30, connection);

            connection.Close();

            if (posts == "")
                posts = "<i>A quiet kiddo amongst the crowd</i>";
        }

        protected string GetName(bool primary) =>
            Utils.GetName(primary, ProfileInformation);


        /// <returns>
        /// A formatted HTML notation for visualizing <see cref="profileInformation">profileInformation["Links"]</see>
        /// </returns>
        protected string GetLinksNotation()
        {
            Link[] links = Utils.GetLinks(ProfileInformation);
            if (links == null)
                return "<i>None to be found</i>";

            string htmlResult = "<ul>";
            for (int i = 0; i < links.Length; i++)
                htmlResult +=
                    "<li>" +
                        $"<img src=\"https://s2.googleusercontent.com/s2/favicons?domain_url={links[i].Url}\" class=\"linkIco\"/>" +
                        $"<a href={links[i].Url}>{links[i].Name}</a>" +
                    "</li>";
            htmlResult += "</ul>";

            return htmlResult;
        }

    }
}