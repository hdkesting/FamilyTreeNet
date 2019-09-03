using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            return date.Value.ToString("dd MMM yyyy");
        }
    }
}
