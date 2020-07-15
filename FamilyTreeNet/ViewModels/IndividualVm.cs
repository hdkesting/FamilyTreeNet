using System;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Support;

namespace FamilyTreeNet.ViewModels
{
    /// <summary>
    /// ViewModel for an individual.
    /// </summary>
    public class IndividualVm
    {
        /// <summary>Initializes a new instance of the <see cref="IndividualVm" /> class.</summary>
        public IndividualVm()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualVm"/> class.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <exception cref="ArgumentNullException">dto</exception>
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

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the full list of firstnames.
        /// </summary>
        /// <value>
        /// The firstnames.
        /// </value>
        public string Firstnames { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>
        /// The lastname.
        /// </value>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets the full name (firstnames + lastname).
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName => $"{Firstnames} {Lastname}";

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        public Sex Sex { get; set; }

        /// <summary>
        /// Gets or sets the character representation of the <see cref="Sex"/>.
        /// </summary>
        /// <value>
        /// The sex character.
        /// </value>
        public string SexChar
        {
            get=> Sex == Sex.Male ? "M"
                    : Sex == Sex.Female ? "F"
                    : "?";

            set => Sex = value == "M" ? Sex.Male
                    : value == "F" ? Sex.Female
                    : Sex.Unknown;
        }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public GeneaDate BirthDate { get; set; }

        /// <summary>
        /// Gets the formatted birth date.
        /// </summary>
        /// <value>
        /// The birth date as string.
        /// </value>
        public string BirthDateFmt => this.BirthDate?.ToString();

        /// <summary>
        /// Gets or sets the birth date as real date.
        /// </summary>
        /// <value>
        /// The birth date date.
        /// </value>
        public DateTime? BirthDateDate
        {
            get=> this.BirthDate?.ToDate();       

            set => this.BirthDate = value == null ? null : new GeneaDate(value.Value);
        }

        /// <summary>
        /// Gets or sets the birth place.
        /// </summary>
        /// <value>
        /// The birth place.
        /// </value>
        public string BirthPlace { get; set; }

        /// <summary>
        /// Gets or sets the death date.
        /// </summary>
        /// <value>
        /// The death date.
        /// </value>
        public GeneaDate DeathDate { get; set; }

        /// <summary>
        /// Gets the formatted death date.
        /// </summary>
        /// <value>
        /// The death date as string.
        /// </value>
        public string DeathDateFmt => this.DeathDate?.ToString();

        /// <summary>
        /// Gets or sets the death date as real date.
        /// </summary>
        /// <value>
        /// The death date date.
        /// </value>
        public DateTime? DeathDateDate
        {
            get => this.DeathDate?.ToDate();

            set => this.DeathDate = (value == null) ? null : new GeneaDate(value.Value);
        }

        /// <summary>
        /// Gets or sets the place of death.
        /// </summary>
        /// <value>
        /// The death place.
        /// </value>
        public string DeathPlace { get; set; }

        /// <summary>
        /// Gets a value indicating whether this individual is known to be male.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this individual is male; otherwise, <c>false</c>.
        /// </value>
        public bool IsMale => this.Sex == Sex.Male;

        /// <summary>
        /// Gets a value indicating whether this individual is known to be female.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this individual is female; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets a value indicating whether this individual died at/near birth.
        /// </summary>
        /// <value>
        ///   <c>true</c> if died at birth; otherwise, <c>false</c>.
        /// </value>
        public bool DiedAtBirth => BirthDate != null && DeathDate != null && BirthDate == DeathDate;

        /// <summary>
        /// Gets a value indicating whether any birth data are known.
        /// </summary>
        /// <value>
        ///   <c>true</c> if birth data known; otherwise, <c>false</c>.
        /// </value>
        public bool BirthDataKnown => BirthDate != null || !String.IsNullOrWhiteSpace(BirthPlace);

        /// <summary>
        /// Gets a value indicating whether any death data are known.
        /// </summary>
        /// <value>
        ///   <c>true</c> if death data known; otherwise, <c>false</c>.
        /// </value>
        public bool DeathDataKnown => DeathDate != null || !String.IsNullOrWhiteSpace(DeathPlace);
    }
}
