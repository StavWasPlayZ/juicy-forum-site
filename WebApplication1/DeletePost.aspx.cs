using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;
using System.Web.UI;

namespace WebApplication1
{
    public partial class DeletePost : Page
    {
        public static bool Deletable(HttpSessionState session, int beholderId) =>
            // User logged in
            (session["uId"] != null) &&
            // Sessioned user is beholder
            (((int)session["uId"] == beholderId) ||
            // Sessioned user is admin
            ((string)session["UserType"] == "adm") || ((string)session["UserType"] == "cre"));

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            if ((Session["uId"] == null) || (id == null))
            {
                Response.Redirect("Home.aspx");
                return;
            }


            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            DataTable data = DatabaseAccessor.GetTable($"select UserId from {DatabaseAccessor.POSTS_DB_NAME} where Id=" + id, connection);

            if ((data.Rows.Count == 0) || !Deletable(Session, (int)data.Rows[0]["UserId"]))
            {
                Response.Redirect("Home.aspx");
                return;
            }


            new SqlCommand($"delete from {DatabaseAccessor.POSTS_DB_NAME} where Id={id}", connection).ExecuteNonQuery();
            connection.Close();
            Response.Redirect("Forums.aspx");
        }
    }
}