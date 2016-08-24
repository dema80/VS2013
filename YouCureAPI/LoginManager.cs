using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YouCureAPI.Models;

namespace YouCureAPI
{
    public class LoginManager
    {
        public static bool Check(ISession session, string token)
        {
            return session.QueryOver<SessionDuration>().Where(sd=>sd.Token==token && sd.Deadline>=DateTime.Now).RowCount()>0;
        }
        public static void NoValidToken(string token)
        {
            throw new Exception(string.Format("token {0} not valid.", token));
        }
    }
}