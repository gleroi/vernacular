using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Vernacular.Mvc.Providers
{
    public interface IRequestCultureProvider
    {
        IEnumerable<CultureInfo> GetCulture(HttpContextBase context);
    }
}
