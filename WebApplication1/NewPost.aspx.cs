using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication1
{
    public partial class NewPost : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if ((string)Session["UserType"] == "blk")
            {
                Response.Redirect("Home.aspx");
                return;
            }

            NameValueCollection form = Request.Form;
            if (form["title"] == null)
                return;

            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            object postId = DatabaseAccessor.InsertToTable(connection, DatabaseAccessor.POSTS_DB_NAME,
                new TableField("UserId", (int)Session["uId"]+""),
                new TableField("Title", form["title"]),
                new TableField("Content", form["content"].Replace("\r\n", "\\n"))
            );
            connection.Close();

            Response.Redirect("Post.aspx?id=" + postId);
        }
    }
}