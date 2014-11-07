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
        public string FallbackLocale { get; set; }
        
        IDictionary<string, IEnumerable<LocalizedString>> LocaleUnits { get; set; }
        IEnumerable<string> LoadedLocales
        {
            get { return this.LocaleUnits.Keys; }
        }

        
        public const string DefaultFallBackLocale = "en";

        public DirectoryCatalog(string path)
        {
            FallbackLocale = DefaultFallBackLocale;
            DirectoryPath = path;
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

        private bool IsLocaleLoaded(string locale)
        {
            return this.LocaleUnits.ContainsKey(locale) ||
                this.LocaleUnits.Keys.Any(loadedLocale => HttpLanguageHelper.MatchingLocales(locale, loadedLocale) == LanguageMatchType.Exact);
        }

        void LoadPoFile(string locale)
        {
            if (IsLocaleLoaded(locale))
                return;

            string filepath = GetPoFile(locale);
            if (String.IsNullOrEmpty(filepath)) 
            {
                filepath = GetPoFile(FallbackLocale);
                locale = FallbackLocale;
                if (String.IsNullOrEmpty(filepath))
                    throw new InvalidOperationException("The fallback locale has no PO file. Please use a correct default locale");
            }
            
            PoParser parser = new PoParser();
            parser.Add(filepath);
            var units = parser.Parse();
            List<LocalizedString> messages = new List<LocalizedString>();
            foreach (ILocalizationUnit u in units)
            {
                LocalizedString msg = u as LocalizedString;
                if (msg != null)
                    messages.Add(msg);
            }
            LocaleUnits[locale] = messages;
        }

        #region Catalog interface

        public override string CoreGetString(string message)
        {
            LoadPoFile(CurrentIsoLanguageCode);

            var msg = LocaleUnits[CurrentIsoLanguageCode].FirstOrDefault(unit => unit.UntranslatedSingularValue == message);
            if (msg != null && msg.TranslatedValues != null)
                return msg.TranslatedValues.First();
            else 
                return message + @" /!\ Untranslated message";
        }

        public override string CoreGetPluralString(string singularMessage, string pluralMessage, int n)
        {
            LoadPoFile(CurrentIsoLanguageCode);

            int pluralOrder = PluralRules.GetOrder(CurrentIsoLanguageCode, n);
            var msg = LocaleUnits[CurrentIsoLanguageCode].FirstOrDefault(unit => 
                unit.UntranslatedSingularValue == singularMessage ||
                unit.UntranslatedPluralValue == pluralMessage);
            if (msg != null && msg.TranslatedValues != null && msg.TranslatedValues.Length > pluralOrder)
                return msg.TranslatedValues[pluralOrder];
            else
                return (pluralOrder == 0 ? singularMessage : pluralMessage) + @" /!\ Untranslated message";
        }

        public override string CoreGetGenderString(LanguageGender gender, string masculineMessage, string feminineMessage)
        {
            LoadPoFile(CurrentIsoLanguageCode);

            var msg = LocaleUnits[CurrentIsoLanguageCode].FirstOrDefault(unit => unit.Gender == gender &&
                (unit.UntranslatedSingularValue == masculineMessage ||
                 unit.UntranslatedSingularValue == feminineMessage));
            if (msg != null && msg.TranslatedValues != null)
                return msg.TranslatedValues.First();
            else
                return (gender == LanguageGender.Masculine ? masculineMessage : feminineMessage) + @" /!\ Untranslated message";
        }

        public override string CoreGetPluralGenderString(LanguageGender gender, string singularMasculineMessage, string pluralMasculineMessage, string singularFeminineMessage, string pluralFeminineMessage, int n)
        {
            LoadPoFile(CurrentIsoLanguageCode);

            int pluralOrder = PluralRules.GetOrder(CurrentIsoLanguageCode, n);
            var msg = LocaleUnits[CurrentIsoLanguageCode].FirstOrDefault(unit => unit.Gender == gender &&
                (unit.UntranslatedSingularValue == singularMasculineMessage ||
                 unit.UntranslatedPluralValue == pluralMasculineMessage ||
                 unit.UntranslatedSingularValue == singularFeminineMessage ||
                 unit.UntranslatedPluralValue == pluralFeminineMessage));
            if (msg != null && msg.TranslatedValues != null && msg.TranslatedValues.Length > pluralOrder)
                return msg.TranslatedValues[pluralOrder];
            else
                return (pluralOrder == 1 ? singularMasculineMessage : pluralMasculineMessage) + @" /!\ Untranslated message";
        }

        #endregion

    }
}
