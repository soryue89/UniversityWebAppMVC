using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Park_University_MVC.Utils
{
    public class SessionFacade
    {
        static readonly string loggedin = "LOGGGEDIN";
        public static string LOGGEDIN
        {
            get
            {
                if (HttpContext.Current.Session[loggedin] != null)
                    return (string)HttpContext.Current.Session[loggedin];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session[loggedin] = value;
            }
        }
    }
}