using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIVProject
{
    public class CustomAuthorize : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            /*
            var authroized = base.AuthorizeCore(httpContext);
            if (!authroized)
            {
                // the user is not authenticated or the forms authentication
                // cookie has expired
                return false;
            }
            */
            // Now check the session:
            
            var myvar = httpContext.Session["username"];
            var role = httpContext.Session["type"];
            if (myvar == null)
            {
                // the session has expired
                return false;
            }
            else
            {
                if (Roles!=null && role != null)
                {
                    string[] CheckRole = Roles.Split(',');
                    for (int i = 0; i < CheckRole.Length; i++)
                    {
                        if (CheckRole[i] == role.ToString())
                        {
                            return true;
                        }

                    }
                    return false;
                }
                return false;
            }

        }

    
       
    }
}