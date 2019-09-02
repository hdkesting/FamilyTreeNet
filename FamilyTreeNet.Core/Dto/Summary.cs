using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeNet.Core.Dto
{
    public class Summary
    {
        public int IndividualCount { get; set; }

        public int FamilyCount { get; set; }

        public int SpouseCount { get; set; }

        public int ChildCount { get; set; }
    }
}
