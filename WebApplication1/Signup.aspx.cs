using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class Signup : IdentificationPage
    {
        protected override bool Restrict => Session["uId"] != null;
        protected override string FormKey => "email";
        protected override string SuccessRedirect => "Profile.aspx";

        protected override bool UpdateTable(SqlConnection connection, NameValueCollection form)
        {
            string email = form["email"],
                getQuery = DatabaseAccessor.GetUserQuery(email, "Id");
            if (DatabaseAccessor.Exists(getQuery, connection))
            {
                message = "Account "+email+" already exists. <a href=\"Login.aspx\">Log in</a> or make a different account using another address.";
                return false;
            }

            DatabaseAccessor.InsertToTable(connection, DatabaseAccessor.USERS_DB_NAME,
                Entry("Email", email),
                Entry("Name", form["fname"] + " " + form["lname"]),
                Entry("Password", form["password"]),
                Entry("Birthdate", form["birthdate"]),
                Entry("Gender", form["gender"])
            );
            DataTable data = DatabaseAccessor.GetTable(getQuery, connection);
            int id = (int)data.Rows[0]["Id"];

            string userName = form["uname"];
            DatabaseAccessor.InsertToTable(connection, DatabaseAccessor.PROFILE_DB_NAME,
                Entry("UserId", id+""),
                (userName == null) ? TableField.Empty : Entry("UserName", userName)
            );

            Session["uId"] = id;
            Session["UserType"] = "usr";

            return true;
        }

        TableField Entry(string name, string value) =>
            new TableField(name, value);
    }

}