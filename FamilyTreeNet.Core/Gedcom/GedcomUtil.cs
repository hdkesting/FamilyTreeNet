using System;
using System.Globalization;

namespace FamilyTreeNet.Core.Gedcom
{
    internal static class GedcomUtil
    {
        /// <summary>
        /// Takes a reference string like "@F123@" and returns the numerical value (123). The letter is ignored.
        /// </summary>
        /// <param name="reference">The reference string like @F123@.</param>
        /// <returns>The numerical value.</returns>
        public static long GetIdFromReference(String reference)
        {
            reference = reference.Trim();
            reference = reference.Substring(2, reference.Length - 3); // cut off the "@I.." and "..@"
            return long.Parse(reference, CultureInfo.InvariantCulture);
        }
    }
}
