using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FamilyTreeNet.Core.Gedcom
{
    internal class GedcomDate
    {
        private static readonly Regex DmyPattern = new Regex(@"(\d{1,2}) ([A-Za-z]{3}) (\d{4})", RegexOptions.Compiled);
        private static readonly Regex MyPattern = new Regex(@"([A-Za-z]{3}) (\d{4})", RegexOptions.Compiled);
        private static readonly Regex YPattern = new Regex(@"(\d{4})", RegexOptions.Compiled);
        private static readonly string[] monthNamesEn = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
        private static readonly string[] monthNamesNl = {"Jan", "Feb", "Mrt", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec"};

        public DateTime Date { get; }

        public bool IsExact { get; }

        private GedcomDate(DateTime date, bool isExact)
        {
            this.Date = date.Date;
            this.IsExact = IsExact;
        }

        public static GedcomDate Parse(string value)
        {
            // try and find "dd MMM yyyy" -> exact match
            // then try "MMM yyyy" -> use day 1, not exact
            // then try "yyyy" -> use jan 1, not exact
            // else null.
            // ignore markers like "about" or "before"

            var m = DmyPattern.Match(value);
            if (m.Success)
            {
                return new GedcomDate(GetDate(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value), true);
            }

            m = MyPattern.Match(value);
            if (m.Success)
            {
                return new GedcomDate(GetDate("1", m.Groups[1].Value, m.Groups[2].Value), isExact: false);
            }

            m = YPattern.Match(value);
            if (m.Success)
            {
                return new GedcomDate(GetDate("1", "1", m.Groups[1].Value), isExact: false);
            }

            return null;
        }

        private static DateTime GetDate(String day, String month, String year)
        {
            int iday = int.Parse(day);
            int imonthEn = monthNamesEn.Select((m, i) => new { m, i = i + 1 }).Where(x => x.m.Equals(month, StringComparison.OrdinalIgnoreCase)).Select(x => x.i).FirstOrDefault();
            int imonthNl = monthNamesNl.Select((m, i) => new { m, i = i + 1 }).Where(x => x.m.Equals(month, StringComparison.OrdinalIgnoreCase)).Select(x => x.i).FirstOrDefault();
            int imonth = imonthEn > 0 ? imonthEn : imonthNl > 0 ? imonthNl : 1;
            int iyear = int.Parse(year);

            return new DateTime(iyear, imonth, iday);
        }

    }
}
