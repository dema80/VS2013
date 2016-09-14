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
    public class CRIResponse
    {
        public string[] values { get; set; }
        public SerializedError error { get; set; }
    }
    public class ComplaintRemedyInfoController : ApiController
    {
        public CRIResponse Get(string token, string remedy, string complaint)
        {
            CRIResponse result = new CRIResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {
                        using (session.BeginTransaction())
                        {
                            result.values = session.QueryOver<ComplaintRemedyInfo>().Where(cri => cri.RAbbrev == remedy && cri.PIcd10Code==complaint).Select(ri=>ri.TextInfo).List<string>().ToArray<string>();
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