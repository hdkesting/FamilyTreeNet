using System;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Support;

namespace FamilyTreeNet.ViewModels
{
    public class IndividualVm
    {
        public IndividualVm()
        {

        }

        public IndividualVm(IndividualDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            this.Id = dto.Id;
            this.Firstnames = dto.Firstnames;
            this.Lastname = dto.Lastname;
            this.Sex = dto.Sex;
            this.BirthDate = dto.BirthDate;
            this.BirthPlace = dto.BirthPlace;
            this.DeathDate = dto.DeathDate;
            this.DeathPlace = dto.DeathPlace;
        }

        public long Id { get; set; }

        public string Firstnames { get; set; }

        public string Lastname { get; set; }

        public string FullName => $"{Firstnames} {Lastname}";

        public Sex Sex { get; set; }

        public DateTime? BirthDate { get; set; }
        public string BirthDateFmt => Utils.FormatDate(this.BirthDate);

        public string BirthPlace { get; set; }

        public DateTime? DeathDate { get; set; }
        public string DeathDateFmt => Utils.FormatDate(this.DeathDate);

        public string DeathPlace { get; set; }

        public bool IsMale => this.Sex == Sex.Male;

        public bool IsFemale => this.Sex == Sex.Female;

        /// <summary>
        /// Gets or sets the CSS, to be used on the "person block".
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public string Css { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the "primary", for use in the Admin section.
        /// </summary>
        /// <value>
        /// The primary identifier.
        /// </value>
        public long PrimaryId { get; set; }

        public bool DiedAtBirth => BirthDate != null && DeathDate != null && BirthDate.Value == DeathDate.Value;

        public bool BirthDataKnown => BirthDate != null || !String.IsNullOrWhiteSpace(BirthPlace);
        public bool DeathDataKnown => DeathDate != null || !String.IsNullOrWhiteSpace(DeathPlace);


    }
}
