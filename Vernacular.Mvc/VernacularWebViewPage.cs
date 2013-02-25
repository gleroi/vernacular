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

        public string __p(string singularMsg, string pluralMsg, int count, 
            string developerComments = null)
        {
            return Catalog.GetPluralString(singularMsg, pluralMsg, count, 
                developerComments);
        }

        #endregion

        #region Localization of MvcHtmlString

        public IHtmlString _(string msgid, string developerComments = null)
        {
            return new MvcHtmlString(__(msgid, developerComments));
        }

        public IHtmlString _p(string singularMsg, string pluralMsg, int count,
            string developerComments = null)
        {
            return new MvcHtmlString(__p(singularMsg, pluralMsg, count,
                developerComments));
        }

        #endregion
    }
}
