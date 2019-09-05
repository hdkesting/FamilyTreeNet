using System;
using System.Globalization;

namespace FamilyTreeNet.ViewModels
{
    internal static class Utils
    {
        public static string FormatDate(DateTime? date)
        {
            if (date == null)
            {
                return string.Empty;
            }

            return date.Value.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
        }
    }
}
