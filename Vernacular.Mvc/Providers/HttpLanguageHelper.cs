using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Vernacular.Mvc.Providers
{
    internal enum LanguageMatchType
    {
        NoMatch,
        Exact,
        Partial
    }

    internal static class HttpLanguageHelper
    {
        public static IEnumerable<CultureInfo> ToCultureInfos(string acceptLanguages)
        {
            List<CultureInfo> cultures = new List<CultureInfo>();
            string[] rangesAndQualities = acceptLanguages.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < rangesAndQualities.Length; i += 2)
            {
                string[] languages = rangesAndQualities[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string language in languages)
                {
                    try
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo(language.Trim());
                        cultures.Add(culture);
                    }
                    catch (CultureNotFoundException)
                    { /* ignore and continue */ }
                    catch (ArgumentException)
                    { /* ignore and continue */ }
                }
            }
            return cultures;
        }

        public static LanguageMatchType MatchingLocales(string locale1, string locale2) 
        {
            if (locale1.Equals(locale2, StringComparison.InvariantCultureIgnoreCase))
            {
                // full name match
                return LanguageMatchType.Exact;
            }
            else if (partialMatch == LanguageMatchType.NoMatch)
            {
                // partial match, fr-FR => fr
                string shortLocale = locale2.Substring(0, 2);
                string shortName = locale1.Substring(0, 2);
                if (shortName.Equals(shortLocale, StringComparison.InvariantCultureIgnoreCase))
                    return LanguageMatchType.Partial;
            }
            return LanguageMatchType.NoMatch;
        }

    }
}
