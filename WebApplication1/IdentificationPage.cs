using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebApplication1
{
    /// <summary>
    /// A page-extended class dedicated to providing support & feedback for inserting into the database.
    /// </summary>
    public abstract class IdentificationPage : Page
    {
        /// <summary>
        /// The field that'll be checked for a null value. If true, a connection will not exist.
        /// </summary>
        protected abstract string FormKey { get; }

        /// <summary>
        /// A property to decide wether a client should be restricted from this page. <see cref="SuccessRedirect"/> must not be null for this to work.
        /// </summary>
        protected virtual bool Restrict => false;
        /// <summary>
        /// The page to be redirected to after a successful submition. Null for nothing.
        /// </summary>
        protected virtual string SuccessRedirect { get; }

        /// <summary>
        /// Assuming the form has been submitted (field <see cref="FormKey"/> isn't null), this event will be called.
        /// It will try to add the given fields to the database it is in charge of.
        /// </summary>
        /// <param name="connection">The connection made between the database and the server. Used for passing queries</param>
        /// <param name="form">Shorthand for this <see cref="System.Web.HttpRequest.Form"/></param>
        /// <returns></returns>
        protected abstract bool UpdateTable(SqlConnection connection, NameValueCollection form);


        protected string message;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Restrict)
            {
                // SuccessRedirect property must not be null if this page is meant to redirect.
                // If this is thrown, it's my fault :P
                if (SuccessRedirect == null)
                    throw new NotImplementedException("SuccessRedirect must have an assigned value for a restriction to work.");
                Response.Redirect(SuccessRedirect);
                return;
            }
            NameValueCollection form = Request.Form;
            if (form[FormKey] == null)
                return;


            try
            {
                SqlConnection connection = DatabaseAccessor.ConnectToDatabase();
                // If validations and insertions for this page's form went successfully, redirect if needed
                if (UpdateTable(connection, form) && (SuccessRedirect != null))
                    Response.Redirect(SuccessRedirect);
                connection.Close();
            }
            catch (SqlException)
            {
                // This exception is most likely the cause of a client messing with DevTools
                message = "There is a reason in which we put a validation function in place. Editing it is dumb. Thus, you are automatically" +
                    " counted as one would describe as \"dumb\".";
            }
        }
    }
}