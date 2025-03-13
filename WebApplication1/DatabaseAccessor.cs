using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace WebApplication1
{
    public static class DatabaseAccessor
    {
        public const string USERS_DB_NAME = "UsersTable", PROFILE_DB_NAME = "ProfileInformation", POSTS_DB_NAME = "Posts";

        static readonly string databasePath = HttpContext.Current.Server.MapPath("\\App_Data\\UsersDB.mdf");
        public static SqlConnection ConnectToDatabase()
        {
            SqlConnection connection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databasePath};" +
                $@"Integrated Security=True;Connect Timeout=30");
            connection.Open();
            return connection;
        }


        public static SqlDataReader GetReader(string sql, SqlConnection connection) =>
            new SqlCommand(sql, connection).ExecuteReader();
        public static DataTable GetTable(string sql, SqlConnection connection)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter data = new SqlDataAdapter(sql, connection);
            data.Fill(dataTable);
            return dataTable;
        }
        public static bool Exists(string sql, SqlConnection connection) =>
            new SqlCommand(sql, connection).ExecuteScalar() != null;

        /// <summary>
        /// Inserts a new row to <paramref name="tableName"/> with given <paramref name="fields"/>.
        /// </summary>
        /// <param name="connection">A connection to communicate with the database</param>
        /// <param name="tableName">The table to insert to</param>
        /// <param name="fields">The fields to pass to the table</param>
        /// <returns>The first row of the first colomn of the execution</returns>
        public static object InsertToTable(SqlConnection connection, string tableName, params TableField[] fields)
        {
            string fieldsQry = "(", valuesQry = "(";
            for (int i = 0; i < fields.Length; i++)
            {
                TableField field = fields[i];
                if (field.IsEmpty)
                    continue;

                string addition = (i == (fields.Length - 1)) ? ")" : ",";

                fieldsQry += field.Name + addition;
                valuesQry += $"'{field.Value.Replace("'", "''")}'{addition}";
            }

            return new SqlCommand($"insert into {tableName} {fieldsQry} values {valuesQry}; select scope_identity();", connection).ExecuteScalar();
        }


        public static string GetUserQuery(string fields, bool topSearch = false, int max = 15) =>
            $"select {(topSearch ? "top "+max+" " : "")}{fields} from {USERS_DB_NAME} left join {PROFILE_DB_NAME} on UserId=Id";
        public static string GetUserQuery(string email, string fields, string password = null) =>
            $"select {fields} from {USERS_DB_NAME} left join {PROFILE_DB_NAME} on UserId=Id " +
                $"where Email='{email}'{((password != null) ? $" and Password='{password}'" : "")}";
        public static DataTable GetUserFromDatabase(string email, string fields, SqlConnection connection, string password = null) =>
            GetTable(GetUserQuery(email, fields, password), connection);
        public static DataTable GetUserFromDatabase(int id, string fields, SqlConnection connection) =>
            GetTable($"select {fields} from {USERS_DB_NAME} left join {PROFILE_DB_NAME} on UserId=Id where Id='{id}'", connection);
    }

    public struct TableField
    {
        public static TableField Empty =>
            new TableField(null, null);

        public string Name { get; private set; }
        public string Value { get; private set; }
        public TableField(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public bool IsEmpty =>
            (Name == null) && (Value == null);
    }
}