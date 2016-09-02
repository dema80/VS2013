using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using YouCureAPI.Models;

namespace YouCureAPI.Controllers
{
    public class PResponse
    {
        //public TranslatedPathology[] values { get; set; }
        public Pathology[] values { get; set; }
        public SerializedError error { get; set; }
    }
    public class LocalizedPatology
    {
        public int PId { get; set; }
        public string PCommonName { get; set; }
        public string PKeySymptoms { get; set; }
        public string PIcd10Code { get; set; }
        public int QIdFirstQuestion { get; set; }
        public string PWarning { get; set; }
        public string PScientificName { get; set; }
    }
    public class RLResponce
    {
        public LocalizedPatology[] values { get; set; }
        public SerializedError error { get; set; }
    }
    public class PTResponse
    {
        //public TranslatedPathology[] values { get; set; }
        public TranslatedPathology[] values { get; set; }
        public SerializedError error { get; set; }
    }
    public class PathologiesController : ApiController
    {
        //GET /api/pathologies?token=<string>&lang=<int>
        public PResponse getPathologies(string token, int lang)
        {
            PResponse result = new PResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {

                        using (session.BeginTransaction())
                        {
                            ICriteria criteria = session.CreateCriteria<Pathology>();
                            IList<Pathology> pats = criteria.List<Pathology>();
                            result.values = getPathologies(session, pats, lang);
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
        //GET /api/pathologies/Localized?token=<string>&lang=<int>&local=<int>
        [HttpGet]
        public RLResponce Localized(string token, int lang, int local)
        {
            RLResponce result = new RLResponce();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {

                        using (session.BeginTransaction())
                        {
                            LocalizedPatology model = null;
                            IList<LocalizedPatology> pats = session.QueryOver<Pathology>().Right.JoinQueryOver<Localisation>(p => p.Localisations).Where(l => l.LId == local).SelectList(list => list
                                    .Select(p => p.PCommonName).WithAlias(() => model.PCommonName)
                                    .Select(p => p.PId).WithAlias(() => model.PId)
                                    .Select(p => p.PKeySymptoms).WithAlias(() => model.PKeySymptoms)
                                    .Select(p => p.PIcd10Code).WithAlias(() => model.PIcd10Code)
                                    .Select(p => p.QIdFirstQuestion).WithAlias(() => model.QIdFirstQuestion)
                                    .Select(p => p.PWarning).WithAlias(() => model.PWarning)
                                    .Select(p => p.PScientificName).WithAlias(() => model.PScientificName)
                                ).TransformUsing(Transformers.AliasToBean<LocalizedPatology>()).Future<LocalizedPatology>().ToList();
                            for (int i = 0; i < pats.Count; i++)
                            {
                                pats[i] = TranslateManager.Translate<LocalizedPatology>(session, lang, pats[i]);
                            }
                            result.values = pats.ToArray<LocalizedPatology>();
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
        //GET /api/pathologies/Filtered?token=<string>&lang=<int>&filter=<string>
        [HttpGet]
        public PResponse Filtered(string token, int lang, string filter)
        {
            PResponse result = new PResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {

                        using (session.BeginTransaction())
                        {
                            string[] filters = filter.Split(' ');
                            var query = session.QueryOver<Translation>();
                            foreach (string filt in filters)
                            {
                                if (lang == 1)
                                {
                                    query = query.AndRestrictionOn(t => t.Original).IsInsensitiveLike(filt, MatchMode.Anywhere);
                                }
                                else
                                {
                                    query = query.AndRestrictionOn(t => t.Translationval).IsInsensitiveLike(filt, MatchMode.Anywhere);
                                }
                            }
                            var resu = query.List();
                            string[] trans = query.Select(t => t.Original).List<string>().ToArray();
                            IList<Pathology> pats = session.QueryOver<Pathology>().AndRestrictionOn(p => p.PScientificName).IsIn(trans).List();
                            result.values = getPathologies(session, pats, lang);
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
        private Pathology[] getPathologies(ISession session, IList<Pathology> pats, int lang)
        {
            for (int i = 0; i < pats.Count; i++)
            {
                pats[i] = TranslateManager.Translate<Pathology>(session, lang, pats[i]);
                for (int j = 0; j < pats[i].Localisations.Count; j++)
                {
                    pats[i].Localisations[j] = TranslateManager.Translate<Localisation>(session, lang, pats[i].Localisations[j]);
                }
            }
            return pats.ToArray<Pathology>();
        }

        //GET /api/pathologies?token=<string>&lang=<int>
        //        [HttpGet]
        //        public PTResponse getPathologies(string token, int lang)
        //        {
        //            PTResponse result = new PTResponse();
        //            try
        //            {
        //                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
        //                {
        //                    using (session.BeginTransaction())
        //                    {
        //                        ICriteria criteria = session.CreateCriteria<Pathology>();
        //                        string select = string.Format(@"
        //                                Select p.p_id,
        //                                    p.q_id_first_question,
        //                                    p.p_icd10_code,
        //                                    COALESCE((select translation from ""Therapeutic"".translation where language_id={0} and original = p.p_scientific_name), p.p_scientific_name) AS p_scientific_name,
        //                                    COALESCE((select translation from ""Therapeutic"".translation where language_id={0} and original = p.p_common_name), p.p_common_name) AS p_common_name,
        //                                    COALESCE((select translation from ""Therapeutic"".translation where language_id={0} and original = p.p_key_symptoms), p.p_key_symptoms) AS p_key_symptoms,
        //                                    COALESCE((select translation from ""Therapeutic"".translation where language_id={0} and original = p.p_warning), p.p_warning) AS p_warning,
        //                                    COALESCE((select translation from ""Therapeutic"".translation where language_id={0} and original = l.l_text), l.l_text) AS l_text
        //                                from 
        //                                    ""Therapeutic"".pathology_localisation pl
        //                                    join ""Therapeutic"".pathology p ON pl.p_id=p.p_id
        //                                    join ""Therapeutic"".localisation l ON pl.l_id=l.l_id", lang);
        //                        var qry = session.CreateSQLQuery(select);
        //                        IList app = qry.List();
        //                        app = app.OfType<object[]>().Select(tp => (TranslatedPathology)tp).ToList();
        //                        result.values = app.OfType<TranslatedPathology>().ToArray<TranslatedPathology>();
        //                        session.Transaction.Commit();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                result.error = ex;
        //            }
        //            return result;
        //        }
    }
}
