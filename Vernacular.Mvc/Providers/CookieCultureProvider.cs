using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Vernacular.Mvc.Providers
{
    public class CookieCultureProvider : IRequestCultureProvider
    {
        public const string DefaultCookieName = "Vernacular_Language";

        public string CookieName { get; set; }

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
            return HttpLanguageHelper.ToCultureInfos(cookie.Value);
        }

        #endregion
    }
}
