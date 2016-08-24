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
    public class QResponse
    {
        public Question value { get; set; }
        public SerializedError error { get; set; }
    }
    public class QuestionController : ApiController
    {

        //GET /api/question?q_id=<int>&lang=<int>
        public QResponse Get(int q_id, int lang)
        {
            QResponse result = new QResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    using (session.BeginTransaction())
                    {
                        result.value = session.Get<Question>(q_id);
                        result.value = TranslateManager.Translate<Question>(session, lang, result.value);
                        for (int i = 0; i < result.value.Answers.Count; i++)
                        {
                            result.value.Answers[i] = TranslateManager.Translate<Answer>(session, lang, result.value.Answers[i]);
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
        //GET /api/question/First?p_id=<int>&lang=<int>
        [HttpGet]
        public QResponse First(string token, int p_id, int lang)
        {
            QResponse result = new QResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {
                        using (session.BeginTransaction())
                        {
                            ICriteria criteria = session.CreateCriteria<Pathology>();
                            Pathology pat = session.Get<Pathology>(p_id);
                            //session.Update(pat);
                            result.value = session.Get<Question>(pat.QIdFirstQuestion);
                            result.value = TranslateManager.Translate<Question>(session, lang, result.value);
                            for (int i = 0; i < result.value.Answers.Count; i++)
                            {
                                result.value.Answers[i] = TranslateManager.Translate<Answer>(session, lang, result.value.Answers[i]);
                            }
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
        //GET /api/question/Next?token=<string>&a_id=<int>&lang=<int>
        //public QResponse Next(string token, int a_id, int lang)
        //{
        //    QResponse result = new QResponse();
        //    try
        //    {
        //        using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
        //        {
        //            if (LoginManager.Check(session, token))
        //            {
        //                using (session.BeginTransaction())
        //                {
        //                    //carico la risposta
        //                    Answer answer = session.Get<Answer>(a_id);
        //                    if (answer != null)
        //                    {
        //                        var max = session.QueryOver<Session>().Select(Projections.ProjectionList().Add(Projections.Max<Session>(x => x.SId))).List<int>().First();
        //                        //Salvo la risposta nella sessione
        //                        Session newSessionItem = new Session()
        //                        {
        //                            SToken = token,
        //                            SAnswerId = a_id,
        //                            SId = max + 1
        //                        };
        //                        session.Save(newSessionItem);
        //                        //Recupero la prossima domanda da ritornare
        //                        result.value = session.Get<Question>(answer.NextQuestionId);
        //                        result.value = TranslateManager.Translate<Question>(session, lang, result.value);
        //                        for (int i = 0; i < result.value.Answers.Count; i++)
        //                        {
        //                            result.value.Answers[i] = TranslateManager.Translate<Answer>(session, lang, result.value.Answers[i]);
        //                        }
        //                    }
        //                    session.Transaction.Commit();
        //                }
        //            }
        //            else
        //                LoginManager.NoValidToken(token);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.error = new SerializedError(ex);
        //    }
        //    return result;
        //}
        [HttpGet]
        public QResponse Next(string token, int q_id, [FromUri]int[] a_id, int lang)
        {
            QResponse result = new QResponse();
            try
            {
                using (ISession session = ApplicationCore.Instance.SessionFactory.OpenSession())
                {
                    if (LoginManager.Check(session, token))
                    {
                        using (session.BeginTransaction())
                        {
                            //carico la risposta
                            Answer[] answers = session.QueryOver<Answer>().AndRestrictionOn(a => a.AId).IsIn(a_id).List().ToArray();
                            if (answers != null && answers.Length > 0)
                            {
                                var max = session.QueryOver<Session>().Select(Projections.ProjectionList().Add(Projections.Max<Session>(x => x.SId))).List<int>().First()+1;
                                foreach (Answer answer in answers)
                                {
                                    //Salvo la risposta nella sessione
                                    Session newSessionItem = new Session()
                                    {
                                        SToken = token,
                                        SAnswerId = answer.AId,
                                        SId = max++
                                    };
                                    session.Save(newSessionItem);
                                }                                
                            }
                            else
                            {
                                //Se l'utente non ha dato risposte recupero tutte le risposte legate alla vecchia domanda per calcolare
                                answers = session.QueryOver<Answer>().Where(a => a.QuestionId == q_id).List().ToArray();
                            }
                            //Recupero la prossima domanda da restituire
                            result.value = session.Get<Question>(answers[0].NextQuestionId);
                            result.value = TranslateManager.Translate<Question>(session, lang, result.value);
                            for (int i = 0; i < result.value.Answers.Count; i++)
                            {
                                result.value.Answers[i] = TranslateManager.Translate<Answer>(session, lang, result.value.Answers[i]);
                            }

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

    }
}