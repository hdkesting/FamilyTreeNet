using System;
using System.Collections.Generic;

namespace FamilyTree.Infra.MySql.Models
{
    public class Individual
    {
        public long Id { get; set; }

        public string Firstnames { get; set; }

        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the sex: M=male, F=female, ?=unknown.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        public Char Sex { get; set; }

        /// <summary>
        /// Gets or sets the birth date, converted to int (yyyymmdda, supporting unknown parts and "accuracy").
        /// </summary>
        /// <value>
        /// The birth date as int.
        /// </value>
        public int? BirthDateInt { get; set; }

        public string BirthPlace { get; set; }

        public int? DeathDateInt { get; set; }

        public string DeathPlace { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets the families where this individual is a spouse.
        /// </summary>
        /// <value>
        /// The spouse families.
        /// </value>
        public List<SpouseRelation> SpouseFamilies { get; } = new List<SpouseRelation>();

        /// <summary>
        /// Gets the families where this individual is a child.
        /// </summary>
        /// <value>
        /// The child families.
        /// </value>
        public List<ChildRelation> ChildFamilies { get; } = new List<ChildRelation>();
    }
}
