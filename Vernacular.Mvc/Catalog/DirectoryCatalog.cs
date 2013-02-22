using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vernacular.Mvc.Providers;
using Vernacular.Parsers;
using Vernacular.Tool;

namespace Vernacular
{
    /// <summary>
    /// Catalog for a directory of po files, named by convention [culture].po
    /// </summary>
    public class DirectoryCatalog : Catalog
    {
        string DirectoryPath { get; set; }
        string LoadedLocale { get; set; }
        IEnumerable<ILocalizationUnit> LocaleUnits { get; set; }
        public string FallbackLocale { get; set; }
        
        public const string DefaultFallBackLocale = "en";

        public DirectoryCatalog(string path)
        {
            FallbackLocale = DefaultFallBackLocale;
            DirectoryPath = path;
            LoadedLocale = null;
        }

        /// <summary>
        /// Return a file from <c>DirectoryPath</c> matching <paramref name="locale"/>
        /// </summary>
        /// <param name="locale">a locale to match against a PO file</param>
        /// <returns></returns>
        string GetPoFile(string locale)
        {
            string partialMatch = null;
            var files = Directory.GetFiles(DirectoryPath, "*.po");
            foreach (string path in files)
            {
                string filename = Path.GetFileNameWithoutExtension(path);
                switch (HttpLanguageHelper.MatchingLocales(filename, locale))
                {
                    case LanguageMatchType.Exact:
                        return path;
                    case LanguageMatchType.Partial:
                        if (String.IsNullOrEmpty(partialMatch))
                            partialMatch = path;
                        break;
                    case LanguageMatchType.NoMatch:
                        break;
                    default:
                        throw new InvalidOperationException("Unknown LanguageMatchType enum value");
                }
            }
            return partialMatch;
        }

        void LoadPoFile(string locale)
        {
            if (!String.IsNullOrEmpty(LoadedLocale) && 
                HttpLanguageHelper.MatchingLocales(locale, LoadedLocale) == LanguageMatchType.Exact)
                return;

            string filepath = GetPoFile(locale);
            if (String.IsNullOrEmpty(filepath)) 
            {
                filepath = GetPoFile(FallbackLocale);
                if (String.IsNullOrEmpty(filepath))
                    throw new InvalidOperationException("The fallback locale has no PO file. Please use a correct default locale");
                LoadedLocale = FallbackLocale;
            }
            else 
            {
                LoadedLocale = locale;
            }
            
            PoParser parser = new PoParser();
            parser.Add(filepath);
            LocaleUnits = parser.Parse();
        }

        #region Catalog interface

        public override string CoreGetString(string message)
        {
            LoadPoFile(CurrentIsoLanguageCode);

            //TODO : find and returns the correct translation;
            return LoadedLocale;
        }

        public override string CoreGetPluralString(string singularMessage, string pluralMessage, int n)
        {
            //TODO : find and returns the correct translation;
            return LoadedLocale;
        }

        public override string CoreGetGenderString(LanguageGender gender, string masculineMessage, string feminineMessage)
        {
            //TODO : find and returns the correct translation;
            return LoadedLocale;
        }

        public override string CoreGetPluralGenderString(LanguageGender gender, string singularMasculineMessage, string pluralMasculineMessage, string singularFeminineMessage, string pluralFeminineMessage, int n)
        {
            //TODO : find and returns the correct translation;
            return LoadedLocale;
        }

        #endregion

    }
}
