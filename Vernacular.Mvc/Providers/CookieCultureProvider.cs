using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Vernacular.Mvc.Providers
{
    public class CookieCultureProvider : IRequestCultureProvider
    {
        const string DefaultCookieName = "Vernacular_Language";
        string CookieName { get; set; }

        public CookieCultureProvider(string cookieName)
        {
            CookieName = cookieName;
        }

        public CookieCultureProvider()
            : this(DefaultCookieName)
        {
        }

        #region IRequestCultureProvider Members

        public IEnumerable<CultureInfo> GetCulture(System.Web.HttpContextBase context)
        {
            HttpCookie cookie = context.Request.Cookies[CookieName];

            if (cookie == null)
            {
                cookie = new HttpCookie(CookieName);
                context.Response.Cookies.Add(cookie);
            }

            if (cookie != null && String.IsNullOrEmpty(cookie.Value))
                cookie.Value = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            
            return HttpLanguageHelper.ToCultureInfos(cookie.Value);
        }

        #endregion
    }
}
