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

        public string SexChar
        {
            get
            {
                return Sex == Sex.Male ? "M"
                    : Sex == Sex.Female ? "F"
                    : "?";
            }

            set
            {
                Sex = value == "M" ? Sex.Male
                    : value == "F" ? Sex.Female
                    : Sex.Unknown;
            }
        }

        public GeneaDate BirthDate { get; set; }
        public string BirthDateFmt => this.BirthDate?.ToString();

        public DateTime? BirthDateDate
        {
            get
            {
                return this.BirthDate?.ToDate();
            }

            set
            {
                if (value == null)
                {
                    this.BirthDate = null;
                }
                else
                {
                    this.BirthDate = new GeneaDate(value.Value);
                }
            }
        }

        public string BirthPlace { get; set; }

        public GeneaDate DeathDate { get; set; }
        public string DeathDateFmt => this.DeathDate?.ToString();

        public DateTime? DeathDateDate
        {
            get
            {
                return this.DeathDate?.ToDate();
            }

            set
            {
                if (value == null)
                {
                    this.DeathDate = null;
                }
                else
                {
                    this.DeathDate = new GeneaDate(value.Value);
                }
            }
        }

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

        public bool DiedAtBirth => BirthDate != null && DeathDate != null && BirthDate == DeathDate;

        public bool BirthDataKnown => BirthDate != null || !String.IsNullOrWhiteSpace(BirthPlace);
        public bool DeathDataKnown => DeathDate != null || !String.IsNullOrWhiteSpace(DeathPlace);


    }
}
