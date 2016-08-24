using NHibernate;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YouCureAPI.Models;

namespace YouCureAPI.Controllers
{
    public class Analysis
    {
        public string rimedio { get; set; }
        public int punteggio { get; set; }
    }
    public class AResponse
    {
        public Analysis[] value { get; set; }
        public int[] idSintomi { get; set; }
        public SerializedError error { get; set; }
    }
    public class AnalysisController : ApiController
    {
        //GET /api/analysis?token=<string>
        public AResponse Get(string token)
        {
            AResponse result = new AResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {
                        using (session.BeginTransaction())
                        {
                            int[] idRisposte = session.QueryOver<Session>().Where(s => s.SToken == token).List().Select(s => s.SAnswerId).ToArray();
                            int[] idSintomi = session.QueryOver<Answer>().List().Where(a => idRisposte.Contains(a.AId)).Select(a => a.SymptomId).ToArray();
                            result.idSintomi = idSintomi;
                            var rimedi = session.QueryOver<HomAddition>().AndRestrictionOn(h => h.RaRubricid).IsIn(idSintomi).List().ToArray();
                            result.value = rimedi.GroupBy(r => r.Remedy.RAbbrev).Select(g => new Analysis() { rimedio = g.Key, punteggio = g.Count() + g.Sum(h => h.RaDegree) }).OrderByDescending(r => r.punteggio).ToArray();
                            session.Transaction.Commit();
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
        [HttpGet]
        public AResponse Test([FromUri]int[] symptoms)
        {
            AResponse result = new AResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                        using (session.BeginTransaction())
                        {
                            result.idSintomi = symptoms;
                            var rimedi = session.QueryOver<HomAddition>().AndRestrictionOn(h => h.RaRubricid).IsIn(symptoms).JoinQueryOver(a => a.Remedy).List().ToArray();
                            result.value = rimedi.GroupBy(r => r.Remedy.RAbbrev).Select(g => new Analysis() { rimedio = g.Key, punteggio = g.Count() + g.Sum(h => h.RaDegree) }).OrderByDescending(r => r.punteggio).ToArray();
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
