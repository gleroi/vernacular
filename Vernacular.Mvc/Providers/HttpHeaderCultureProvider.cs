using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Vernacular.Mvc.Providers
{
    public class HttpHeaderCultureProvider : IRequestCultureProvider
    {
        #region IRequestCultureProvider Members

        public IEnumerable<CultureInfo> GetCulture(System.Web.HttpContextBase context)
        {
            List<CultureInfo> cultures = new List<CultureInfo>();
            string acceptLanguages = context.Request.Headers["Accept-Language"];

            if (!String.IsNullOrWhiteSpace(acceptLanguages))
            {
                return HttpLanguageHelper.ToCultureInfos(acceptLanguages);
            }

            return new List<CultureInfo>();
        }

        #endregion

    }
}
