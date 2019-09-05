using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FamilyTreeNet.Core.Gedcom
{
    /// <summary>
    /// Represents a single line from a GEDCOM file.
    /// </summary>
    internal class PropertyLine
    {
        public int Level { get; }

        public string Keyword { get; }

        public string Value { get; }

        public PropertyLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                throw new ArgumentException("Line cannot be empty", nameof(line));
            }

            string[] parts = line.Split(' ');
            this.Level = int.Parse(parts[0], CultureInfo.InvariantCulture); // assuming this doesn't fail
            if (parts.Length >= 2)
            {
                this.Keyword = parts[1];
            }

            if (parts.Length >= 3)
            {
                // rest of the line, even when multiple words
                this.Value = line.Substring(parts[0].Length + parts[1].Length + 2); // also skip the 2 separating spaces
            }
        }
    }
}
