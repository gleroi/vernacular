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
        static IRequestCultureProvider CultureProvider { get; set; }
        public static void Config(Catalog instance, IRequestCultureProvider cultureProvider)
        {
            Catalog.Implementation = instance;
            CultureProvider = cultureProvider;

            HttpApplication app = HttpContext.Current.ApplicationInstance;
            app.PreRequestHandlerExecute += app_PreRequestHandlerExecute;
            app.Disposed += app_Disposed;
        }

        static void app_Disposed(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null)
            {
                app.PreRequestHandlerExecute -= app_PreRequestHandlerExecute;
                app.Disposed -= app_Disposed;
            }
        }

        static void app_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            SetCultures();
        }

        public static void SetCultures()
        {
            IEnumerable<CultureInfo> cultureInfo = CultureProvider.GetCulture(new HttpContextWrapper(HttpContext.Current));
            Thread.CurrentThread.CurrentCulture = cultureInfo.First();
            Thread.CurrentThread.CurrentUICulture = cultureInfo.First();
        }
    }
}
