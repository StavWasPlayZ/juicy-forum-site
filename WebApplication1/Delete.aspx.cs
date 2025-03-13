using System;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class Delete : UserPage
    {
        protected override bool BeholderBeLogged => true;
        protected override bool CollectData => false;
        // To prevent troll redirections (although it won't happen since it's a school project but whatever)
        protected override bool AutoSessionedUser => false;

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            new SqlCommand($"delete from {DatabaseAccessor.POSTS_DB_NAME} where UserId={Beholder}", connection).ExecuteNonQuery();
            new SqlCommand($"delete from {DatabaseAccessor.PROFILE_DB_NAME} where UserId={Beholder}", connection).ExecuteNonQuery();
            new SqlCommand($"delete from {DatabaseAccessor.USERS_DB_NAME} where Id={Beholder}", connection).ExecuteNonQuery();
            connection.Close();


            if (Beholder == (int)Session["uId"])
            {
                Session.Abandon();
            }
            Response.Redirect("Home.aspx");
        }
    }
}