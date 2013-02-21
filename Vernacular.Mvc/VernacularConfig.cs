using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Vernacular.Mvc.Providers;

namespace Vernacular.Mvc
{
    public static class VernacularConfig
    {
        public static void Config(Catalog instance, IRequestCultureProvider cultureProvider)
        {
            Catalog.Implementation = instance;
            CultureInfo cultureInfo = cultureProvider.GetCulture(new HttpContextWrapper(HttpContext.Current));
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
