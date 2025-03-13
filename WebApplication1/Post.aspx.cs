using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Post : Page
    {
        protected int beholderId;
        protected string beholderPfp, beholderName, beholderTag,
            articleTitle, articleContent, articlePostTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            string postId = Request.QueryString["id"];
            if (postId == null)
            {
                Response.Redirect("Forums.aspx");
                return;
            }

            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();

            DataRowCollection rawPostData = DatabaseAccessor.GetTable(
                $"select * from {DatabaseAccessor.POSTS_DB_NAME} where Id={postId}", connection).Rows;
            if (rawPostData.Count == 0)
            {
                Response.Redirect("Forums.aspx");
                return;
            }

            DataRow postData = rawPostData[0],
                userData = DatabaseAccessor.GetTable(DatabaseAccessor.GetUserQuery("Id,PfpURL,Name,UserName,UserType") + " where Id=" + postData["UserId"], connection).Rows[0];

            beholderId = (int)userData["Id"];
            beholderName = Utils.GetName(true, userData["Name"], userData["UserName"]);
            beholderPfp = Utils.GetPfp(userData["PfpURL"]);
            beholderTag = Utils.GetRole((string)userData["UserType"]);

            articleTitle = (string)postData["Title"];
            articleContent = ((string)postData["Content"]).Replace("\\n", "<br/>");
            articlePostTime = ((DateTime)postData["PostDate"]).ToString("dd/MM/yyyy");


            Title = $"JFS | {articleTitle} [By {beholderName}]";

            connection.Close();
        }
    }
}