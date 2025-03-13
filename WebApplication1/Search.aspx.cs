using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace WebApplication1
{
    public partial class Search : Page
    {
        protected string resultMsg, subResMsg, results = "",
            stylesheets;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string term = Request.QueryString["term"];
            if ((term == null) || (term == ""))
            {
                resultMsg = "Did you mean to write something? um";
                Title = "JFS | um";
                ThrowNoResults();
                return;
            }
            term = term.Trim().Replace("+", " ");
            string resStr = "Results for '" + term + "'";

            Title = "JFS | "+resStr;
            resultMsg = resStr;

            term = term.Replace("'", "''");

            // Profiles Search
            if ((Request.QueryString["type"] == null) || (Request.QueryString["type"] == "profiles"))
            {
                PerformProfilesSearch(term);
                stylesheets = "<link href='ProfileItem.css' rel='stylesheet' />";
            } else
            // Posts Search
            {
                PerformPostsSearch(term);
                stylesheets = "<link href='Post.css' rel='stylesheet' /> <link href='Profile.css' rel='stylesheet' />";
            }
        }

        void PerformProfilesSearch(string term)
        {
            string upperTerm = term.ToUpper();

            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            DataTable data = DatabaseAccessor.GetTable(DatabaseAccessor.GetUserQuery("PfpURL,Name,UserName,Id,UserType", true) +
                    $" where (upper(UserName) like '%{upperTerm}%') or (upper(Name) like '%{upperTerm}%') or (Email='{term}')", connection);
            connection.Close();

            if (data.Rows.Count == 0)
            {
                ThrowNoResults();
                return;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                object name = data.Rows[i]["Name"], uName = data.Rows[i]["UserName"];

                builder.Append(
                    GenerateUserResult(
                        (int)data.Rows[i]["Id"], Utils.GetPfp(data.Rows[i]["PfpURL"]), (string)data.Rows[i]["UserType"],
                        Utils.GetName(true, name, uName), Utils.GetName(false, name, uName)
                    )
                );
            }
            results = builder.ToString();
        }

        void PerformPostsSearch(string term)
        {
            string upperTerm = term.ToUpper();

            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            DataTable data = DatabaseAccessor.GetTable("select * from " + DatabaseAccessor.POSTS_DB_NAME +
                $" where (upper(Title) like '%{upperTerm}%') or (upper(Content) like '%{upperTerm}%')", connection);
            connection.Close();

            if (data.Rows.Count == 0)
            {
                ThrowNoResults();
                return;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                builder.Append(Forums.GetForumNotation(data.Rows[i], 50, connection));
            }
            results = builder.ToString();
        }


        void ThrowNoResults() =>
            subResMsg = "No results found";

        string GenerateUserResult(int id, string pfpUrl, string role, string mainName, string secondName) =>
            $"<a href='Profile.aspx?beholder={id}'>" +
                $"<div class='profile'>" +
                    $"<div class='pfp' style='background-image: url(\"{pfpUrl}\");'></div>" +
                    $"<p class='mainName'>{mainName}</p>" +
                    ((secondName != "") ? $"<p class='secondName'>AKA<i> {secondName}</i></p>" : "") +
                    Utils.GetRole(role) +
                "</div>" +
            "</a>";
    }
}