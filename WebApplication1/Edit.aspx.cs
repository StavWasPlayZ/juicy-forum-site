using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebApplication1
{
    //TODO: Unite all StringBuilder methods
    public partial class Edit : UserPage
    {
        protected override bool BeholderBeLogged => true;
        protected Link[] links;
        protected string message;
        protected string name;

        protected bool IsAdmin(string userType) =>
            (userType == "adm") || (userType == "cre");
        protected bool IsAdmin() =>
            IsAdmin((string)Session["UserType"]);

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            links = Utils.GetLinks(ProfileInformation);
            this.name = Utils.GetName(true, ProfileInformation);

            NameValueCollection form = Request.Form;
            if (form["cPassword"] == null)
                return;

            SqlConnection connection = DatabaseAccessor.ConnectToDatabase();

            // Verify this user's password
            if (!DatabaseAccessor.Exists($"select Id from UsersTable where Id={Session["uId"]} and Password='{form["oPassword"]}'", connection))
            {
                message = "Incorrect password. Please make sure it's your current, active one.";
                connection.Close();
                return;
            }


            // Initialize profile fields query
            string updateQuery = GetUpdateQuery(new UpdateField[]
            {
                Entry("UserName", "uname"),
                Entry("Description", "description", true),
                Entry("Links", () => GetLinksList(3, form), true),
                Entry("PfpURL", "pfpURL", true)
            });
            if (updateQuery != null)
                new SqlCommand($"update {DatabaseAccessor.PROFILE_DB_NAME} set {updateQuery} where UserId={Beholder}", connection).ExecuteNonQuery();

            // & user fields
            string name = form["fname"] + " " + form["lname"];
            updateQuery = GetUpdateQuery(new UpdateField[]
            {
                Entry("Name", () => (name == " ") ? "" : name),
                Entry("Email"),
                Entry("Birthdate"),
                Entry("Gender"),
                // Data & Security
                Entry("Password"),
                Entry("UserType", () => (IsAdmin() && ((string)ProfileInformation["UserType"] != "cre") && (form["userType"] != "cre")) ?
                    form["userType"] : null)
            });
            if (updateQuery != null)
                new SqlCommand($"update {DatabaseAccessor.USERS_DB_NAME} set {updateQuery} where Id={Beholder}", connection).ExecuteNonQuery();


            DataTable data = DatabaseAccessor.GetUserFromDatabase(Beholder, "UserName,PfpURL", connection);
            connection.Close();

            if (IsLoggedUser)
            {
                Session["UserName"] = data.Rows[0]["UserName"];
                Session["PfpURL"] = data.Rows[0]["PfpURL"];
            }


            Response.Redirect(Request.RawUrl);

        }
        UpdateField Entry(string name, string value = null, bool forceUpdate = false) =>
            new UpdateField(name, Request.Form[value ?? name.ToLower()], forceUpdate);
        static UpdateField Entry(string name, NoArgsStringMethod value, bool forceUpdate = false) =>
            new UpdateField(name, value() ?? "", forceUpdate);

        static AllOrNothing AONEntry(string name, string value) =>
            new AllOrNothing(name, value);



        static string GetUpdateQuery(UpdateField[] fields)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < fields.Length; i++)
            {
                UpdateField field = fields[i];
                if (field.ForceUpdate || ((field.Value != null) && (field.Value != "")))
                    builder.Append($"{field.Name}='{field.Value.Replace("'", "''")}',");
            }

            string result = builder.ToString();
            return (result.Length == 0) ? null : builder.ToString().Substring(0, builder.Length - 1);
        }

        static string GetLinksList(int links, NameValueCollection form)
        {
            AllOrNothing[] pairs = new AllOrNothing[links * 2];
            for (int i = 0; i < pairs.Length; i++)
            {
                // A link does not have a space in it, and will ruin the database structure
                if ((pairs[i].Value != null) && pairs[i].Value.Contains(" "))
                    return "";

                pairs[i] = AONEntry(form[$"link{i+1}N"], form["link"+(i+1)]);
            }
            return AllOrNothing.Joint(pairs);
        }
        protected string GetLinkProperty(bool name, int index) =>
            ((links == null) || (index > (links.Length - 1))) ? "" : (name ? links[index].Name : links[index].Url);
    }

    delegate string NoArgsStringMethod();

    /// <summary>
    /// Represents an editable field in the Edit page.
    /// </summary>
    struct UpdateField
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public bool ForceUpdate { get; private set; }
        public UpdateField(string name, string value, bool forceUpdate)
        {
            Name = name;
            Value = value;
            ForceUpdate = forceUpdate;
        }
    }
    /// <summary>
    /// Represents an editable field in the Edit page.
    /// </summary>
    struct AllOrNothing
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public AllOrNothing(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <returns>
        /// <para><see cref="Name"/> and <see cref="Value"/> seperated by " ".</para>
        /// Name's whitespace is replaced with the unicode character "%20" for the database to recognize as a signle string
        /// (assuming Value is a link).
        /// </returns>
        public override string ToString() =>
            (StringEmpty(Name) || StringEmpty(Value)) ? null : Value + " " + Name.Replace(" ", "%20");

        static bool StringEmpty(string str) =>
            (str == null) || (str == "");

        public static string Joint(params AllOrNothing[] pairs)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < pairs.Length; i++)
            {
                string pair = pairs[i].ToString();
                if (pair != null)
                    builder.Append(pair + " ");
            }

            string result = builder.ToString();
            return (result.Length == 0) ? "" : result.Substring(0, result.Length - 1);
        }
    }
}