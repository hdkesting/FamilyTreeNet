using FamilyTreeNet.Core.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeNet.Core.Dto
{
    public class IndividualDto
    {
        public long Id { get; set; }

        public string Firstnames { get; set; }

        public string Lastname { get; set; }

        public Sex Sex { get; set; }

        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }

        public DateTime? DeathDate { get; set; }
        public string DeathPlace { get; set; }

        /// <summary>
        /// Gets the families where this individual is a spouse.
        /// </summary>
        /// <value>
        /// The spouse families.
        /// </value>
        public IList<FamilyDto> SpouseFamilies { get; } = new List<FamilyDto>();

        /// <summary>
        /// Gets the families where this individual is a child.
        /// </summary>
        /// <value>
        /// The child families.
        /// </value>
        public IList<FamilyDto> ChildFamilies { get; } = new List<FamilyDto>();
    }
}
