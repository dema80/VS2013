using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YouCureAPI.Models;

namespace YouCureAPI.Controllers
{
    public class lResponse{
        public string token { get; set; }
        public SerializedError error { get; set; }
    }
    public class LoginController:ApiController
    {
        //GET /api/login?user=<string>&pwd=<string>
        public lResponse Get(string user, string pwd)
        {
            lResponse result = new lResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    using (session.BeginTransaction())
                    {
                        //carico la risposta
                        if (session.QueryOver<Users>().Where(u => u.Login == user && u.Pwd == pwd).List().Count == 1)
                        {
                            result.token = Guid.NewGuid().ToString();                            
                            SessionDuration sd = new SessionDuration()
                            {
                                Token = result.token,
                                Deadline =  DateTime.Now.AddHours(1)
                            };
                            session.Save(sd);
                        }
                        session.Transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                result.error = new SerializedError(ex);
            }
            return result;
        }
    }
}