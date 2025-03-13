using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace WebApplication1
{
    /// <summary>
    /// A page-class used to get information about the current logged in or queried user,
    /// passed via the <c>beholder: int</c> GET query.
    /// </summary>
    /// <remarks>
    /// <i>Note: "Beholder" referes to the user being referred to in this page.</i>
    /// </remarks>
    public abstract class UserPage : Page
    {
        /// <summary>
        /// Defines wether the client should be redirected if the Beholder query is not self. Ignores 'cre' and 'adm' roles
        /// </summary>
        protected abstract bool BeholderBeLogged { get; }
        /// <summary>
        /// An array to define the values that <see cref="ProfileInformation"/> should collect from the
        /// <see cref="DatabaseAccessor.USERS_DB_NAME"/> and <see cref="DatabaseAccessor.PROFILE_DB_NAME"/> tables.
        /// </summary>
        protected virtual string[] Fields => null;
        /// <summary>
        /// Makes it so that <see cref="ProfileInformation"/> won't be initiated.
        /// </summary>
        protected virtual bool CollectData => true;
        /// <summary>
        /// If no beholder (query) is passed, this sessioned user will be.
        /// </summary>
        protected virtual bool AutoSessionedUser => true;

        /// <summary>
        /// The Beholder's ID
        /// </summary>
        protected int Beholder { get; private set; }
        /// <summary>
        /// Whether the Beholder is the logged in user
        /// </summary>
        protected bool IsLoggedUser { get; private set; }
        protected Dictionary<string, object> ProfileInformation { get; private set; }


        // Sets a dictionary containing given fields from the user and profiles databases.
        // User in context may be the sessioned or site queried user associated with this page.
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            // Get the ID of the logged in user
            object rawId = Session["uId"];

            // Deny access to non-registered users
            if ((rawId == null) || (rawId is DBNull))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            ParseBeholder();

            // Redirect if page restricts to logged in user, and user is not the queried one (ingore admins)
            string uType = (string)Session["UserType"];
            if (BeholderBeLogged && !IsLoggedUser && !((uType == "cre") || (uType == "adm")))
            {
                Response.Redirect("Home.aspx");
                return;
            }

            LoadUserData();
        }

        /// <summary>
        /// Parses the requested beholder from the GET parameter <c>beholder</c>,
        /// and updates <see cref="UserPage.Beholder">Beholder</see> and <see cref="UserPage.IsLoggedUser">IsLoggedUser</see> appropriately.
        /// </summary>
        private void ParseBeholder()
        {
            object rawId = Session["uId"];

            // Get self or requested profile to view
            object rawQuery = Request.QueryString["Beholder"];

            // If no query provided, we should parse the request as being the current user's page.
            if (rawQuery == null)
            {
                if (AutoSessionedUser)
                {
                    IsLoggedUser = true;
                    Beholder = (int)rawId;
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }

                return;
            }

            // Check if the given ID is valid
            try
            {
                Beholder = int.Parse((string)rawQuery);
            }
            catch (FormatException)
            {
                Response.Redirect("error.aspx?code=502");
                return;
            }
            catch (OverflowException)
            {
                Response.Redirect("error.aspx?code=400");
                return;
            }

            IsLoggedUser = Beholder == (int)rawId;
        }

        /// <summary>
        /// Loads the user data from the database into
        /// <see cref="UserPage.ProfileInformation">ProfileInformation</see>.
        /// </summary>
        private void LoadUserData()
        {
            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
            if (!CollectData)
            {
                // Check if the given ID is even valid
                if (!DatabaseAccessor.Exists("select Id from UsersTable where Id=" + Beholder, connection))
                    Response.Redirect("error.aspx?code=404");
                connection.Close();
                return;
            }

            // Build the row list to be ready for a select query
            string rowQuery;
            if (Fields != null)
            {
                StringBuilder queryBuilder = new StringBuilder();

                for (int i = 0; i < Fields.Length; i++)
                {
                    queryBuilder.Append(Fields[i]).Append((i == (Fields.Length - 1)) ? "" : ",");
                }
                rowQuery = queryBuilder.ToString();
            }
            else
            {
                rowQuery = "*";
            }

            // Query them
            DataTable data = DatabaseAccessor.GetTable(
                $"select {rowQuery} from {DatabaseAccessor.USERS_DB_NAME} left join {DatabaseAccessor.PROFILE_DB_NAME} on UserId=Id " +
                $"where Id={Beholder} ",
            connection);
            connection.Close();


            ProfileInformation = new Dictionary<string, object>();
            // Return appropriate results
            if (data.Rows.Count == 0)
            {
                Response.Redirect("error.aspx?code=404");
                return;
            }

            for (int i = 0; i < data.Columns.Count; i++)
            {
                ProfileInformation[data.Columns[i].ColumnName] = data.Rows[0][i];
            }
        }
    }
}