﻿using System;
using System.Web.Routing;

namespace WebApplication1
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["message"] = null;
            Utils.ResetUserSession(Session);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session["message"] = null;
            Utils.ResetUserSession(Session);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}