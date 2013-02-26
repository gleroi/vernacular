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
    /// <summary>
    /// Configures the Vernacular localization service.
    /// If you use a culture provider, don't forget to call <c>Vernacular.SetCultures</c> in
    /// the <c>HttpApplication.PreRequestHandlerExecute </c> event.
    /// </summary>
    public static class VernacularConfig
    {
        static IRequestCultureProvider CultureProvider { get; set; }

        /// <summary>
        /// Configure Vernacular localization service
        /// </summary>
        /// <param name="instance">Catalog instance to use to obtain translations</param>
        /// <param name="cultureProvider">a culture provider, pass null if you want to use IIS behavior</param>
        public static void Config(Catalog instance, IRequestCultureProvider cultureProvider)
        {
            Catalog.Implementation = instance;
            CultureProvider = cultureProvider;
        }

        public static void SetCultures()
        {
            if (CultureProvider != null)
            {
                IEnumerable<CultureInfo> cultureInfo = CultureProvider.GetCulture(new HttpContextWrapper(HttpContext.Current));
                Thread.CurrentThread.CurrentCulture = cultureInfo.First();
                Thread.CurrentThread.CurrentUICulture = cultureInfo.First();
            }
        }
    }
}
