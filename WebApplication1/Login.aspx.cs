using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class Login : IdentificationPage
    {
        protected override bool Restrict => Session["uId"] != null;
        protected override string FormKey => "email";
        protected override string SuccessRedirect => "Profile.aspx";

        protected override bool UpdateTable(SqlConnection connection, NameValueCollection form)
        {
            DataTable data = DatabaseAccessor.GetUserFromDatabase(form["email"], "Id,PfpURL,UserType,UserName", connection, form["password"]);
            if (data.Rows.Count == 0)
            {
                message = "Email or password is incorrect. Please try again.<br/>To sign up, click <a href=\"Signup.aspx\">here</a>";
                return false;
            }

            Utils.SetBasicUserSession(Session, GetRow(data, "Id"), GetRow(data, "PfpURL"), GetRow(data, "UserType"), GetRow(data, "UserName"));
            return true;
        }
        static object GetRow(DataTable data, string columnName) =>
            data.Rows[0][columnName];
    }
}