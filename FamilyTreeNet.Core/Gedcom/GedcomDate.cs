using FamilyTreeNet.Core.Support;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FamilyTreeNet.Core.Gedcom
{
    internal static class GedcomDate
    {
        private static readonly Regex DmyPattern = new Regex(@"(\d{1,2}) ([A-Za-z]{3}) (\d{4})", RegexOptions.Compiled);
        private static readonly Regex MyPattern = new Regex(@"([A-Za-z]{3}) (\d{4})", RegexOptions.Compiled);
        private static readonly Regex YPattern = new Regex(@"(\d{4})", RegexOptions.Compiled);
        private static readonly string[] monthNamesEn = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
        private static readonly string[] monthNamesNl = {"Jan", "Feb", "Mrt", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"};


        public static GeneaDate Parse(string value)
        {
            // try and find "dd MMM yyyy" -> exact match
            // then try "MMM yyyy" -> use day 1, not exact
            // then try "yyyy" -> use jan 1, not exact
            // else null.
            // ignore markers like "about" or "before"

            var match = DmyPattern.Match(value);
            if (match.Success)
            {
                return new GeneaDate(
                    ParseInt(match.Groups[3].Value), 
                    ParseMonth(match.Groups[2].Value),
                    ParseInt(match.Groups[1].Value), 
                    DateAccuracy.Exact);
            }

            match = MyPattern.Match(value);
            if (match.Success)
            {
                return new GeneaDate(
                    ParseInt(match.Groups[2].Value), 
                    ParseMonth(match.Groups[1].Value),
                    0,
                    DateAccuracy.About);
            }

            match = YPattern.Match(value);
            if (match.Success)
            {
                return new GeneaDate(ParseInt(match.Groups[1].Value), 0, 0, DateAccuracy.About);
            }

            return null;

            int ParseInt(string s)
            {
                if (int.TryParse(s, out int res))
                {
                    return res;
                }

                return 0;
            }

            int ParseMonth(string month)
            {
                int imonthEn = monthNamesEn.Select((m, i) => new { m, i = i + 1 }).Where(x => x.m.Equals(month, StringComparison.OrdinalIgnoreCase)).Select(x => x.i).FirstOrDefault();
                int imonthNl = monthNamesNl.Select((m, i) => new { m, i = i + 1 }).Where(x => x.m.Equals(month, StringComparison.OrdinalIgnoreCase)).Select(x => x.i).FirstOrDefault();
                int imonth = imonthEn > 0 ? imonthEn 
                                : imonthNl > 0 ? imonthNl 
                                : 0;
                return imonth;
            }
        }
    }
}
