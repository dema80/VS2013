using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using YouCureAPI.Models;

namespace YouCureAPI
{
    public class TranslateManager
    {
        public static T Translate<T>(ISession session, int lang, T result)
        {
            PropertyInfo[] props= typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if(prop.PropertyType==typeof(System.String)){
                    prop.SetValue(result, TranslateField(session, lang, (string)prop.GetValue(result, null)), null);
                }
            }
            return result;
        }
        private static string TranslateField(ISession session, int lang, string param)
        {
            string app = session.QueryOver<Translation>().Where(t => t.Original == param && t.LanguageId == lang).Select(t => t.Translationval).SingleOrDefault<string>();
            if (app != null)
                return app;
            else
                return param;
        }
    }
}