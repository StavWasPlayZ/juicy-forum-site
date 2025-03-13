using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Forums : Page
    {
        protected string posts = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();


            // Limit to first 15 results so that it won't load 50,000 posts if exists
            DataTable postsData = DatabaseAccessor.GetTable($"select top 15 * from {DatabaseAccessor.POSTS_DB_NAME} order by PostDate desc",
                connection);

            for (int i = 0; i < postsData.Rows.Count; i++)
                posts += GetForumNotation(postsData.Rows[i], 50, connection);


            connection.Close();
        }

        /// <summary>
        /// Visualizes an <paramref name="article"/> as a preview box
        /// </summary>
        /// <param name="article">The row representing a post. Must include all fields from <see cref="DatabaseAccessor.POSTS_DB_NAME"/></param>
        /// <param name="connection">The connection made between this server and <see cref="DatabaseAccessor.databasePath"/></param>
        /// <param name="maxChars">The maximum amount of characters to be returned in the content preview</param>
        /// <returns>The HTML notation for the visualization</returns>
        public static string GetForumNotation(DataRow article, int maxChars, SqlConnection connection)
        {
            DataRow author = DatabaseAccessor.GetTable(
                DatabaseAccessor.GetUserQuery("PfpURL,Name,UserName,UserType") + " where Id=" + article["UserId"], connection).Rows[0];

            string content = article["Content"].ToString(),
                shortenedContent = ((content.Length > maxChars) ? content.Substring(0, maxChars) : content).Replace("\\n", " ");

            return
                "<div class='forum'>" +
                    $"<a href='Profile.aspx?beholder={article["UserId"]}'>" +
                        "<div class='profile'>" +
                            $"<div class='pfp' style='background-image: url(\"{Utils.GetPfp(author["PfpURL"])}\");'></div>" +
                            $"<p class='mainName'>{Utils.GetName(true, author["Name"], author["UserName"])}</p>" +
                            Utils.GetRole((string)author["UserType"]) +
                        "</div>" +
                    "</a>" +

                    $"<a href='Post.aspx?id={article["Id"]}'>" +
                        "<article>" +
                            "<div class='vr'></div>" +
                            $"<h2>{article["Title"]}</h2>" +
                            $"<p>{shortenedContent + ((content.Length > maxChars) ? "..." : "")}</p>" +
                            $"<p class='postTime'>Posted on <time>{(DateTime)article["PostDate"]:dd/MM/yyyy}</time></p>" +
                        "</article>" +
                    "</a>" +
                "</div>";
        }
    }
}