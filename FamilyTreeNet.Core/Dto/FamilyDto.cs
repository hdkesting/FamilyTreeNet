using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeNet.Core.Dto
{
    public class FamilyDto
    {
        public long Id { get; set; }

        public DateTime? MarriageDate { get; set; }
        public string MarriagePlace { get; set; }

        public DateTime? DivorceDate { get; set; }
        public string DivorcePlace { get; set; }

        public IList<IndividualDto> Spouses { get; } = new List<IndividualDto>();
        public IList<IndividualDto> Children { get; } = new List<IndividualDto>();

        public override string ToString() => $"Family {Id}";
    }
}
