using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Vernacular.Mvc
{
    public abstract class VernacularWebViewPage<T> : WebViewPage<T>
    {
        #region Localization of strings

        public string __(string msgid, string developerComments = null)
        {
            return Catalog.GetString(msgid, developerComments);
        }

        public string __(string singularMsg, string pluralMsg, int count, 
            string developerComments = null)
        {
            return Catalog.GetPluralString(singularMsg, pluralMsg, count, 
                developerComments);
        }

        public string __(LanguageGender gender, string masculineMsg, string feminineMsg, 
            string developerComments = null)
        {
            return Catalog.GetGenderString(gender, masculineMsg, feminineMsg, 
                developerComments);
        }

        public string __(LanguageGender gender, 
            string singularMasculineMsg, string pluralMasculineMsg,
            string singularFeminineMsg, string pluralFeminineMsg,
            int count, string developerComments = null) {
            return Catalog.GetPluralGenderString(gender, singularMasculineMsg, pluralMasculineMsg,
                singularFeminineMsg, pluralFeminineMsg, count, developerComments);
        }

        #endregion

        #region Localization of MvcHtmlString

        public IHtmlString _(string msgid, string developerComments = null)
        {
            return new MvcHtmlString(__(msgid, developerComments));
        }

        public IHtmlString _(string singularMsg, string pluralMsg, int count,
            string developerComments = null)
        {
            return new MvcHtmlString(__(singularMsg, pluralMsg, count,
                developerComments));
        }

        public IHtmlString _(LanguageGender gender, string masculineMsg, string feminineMsg,
            string developerComments = null)
        {
            return new MvcHtmlString(__(gender, masculineMsg, feminineMsg,
                developerComments));
        }

        public IHtmlString _(LanguageGender gender,
            string singularMasculineMsg, string pluralMasculineMsg,
            string singularFeminineMsg, string pluralFeminineMsg,
            int count, string developerComments = null)
        {
            return new MvcHtmlString(__(gender, 
                singularMasculineMsg, pluralMasculineMsg,
                singularFeminineMsg, pluralFeminineMsg, 
                count, developerComments));
        }

        #endregion
    }
}
