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
    public class RIResponse
    {
        public string[] values { get; set; }
        public SerializedError error { get; set; }
    }
    public class RemedyInfoGeneralController : ApiController
    {
        public RIResponse Get(string token, string remedy)
        {
            RIResponse result = new RIResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {
                        using (session.BeginTransaction())
                        {
                            result.values = session.QueryOver<RemedyGenericInfo>().Where(ri => ri.RAbbrev == remedy).Select(ri=>ri.RTextInfo).List<string>().ToArray<string>();
                            session.Transaction.Rollback();
                        }
                    }
                    else
                        LoginManager.NoValidToken(token);
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