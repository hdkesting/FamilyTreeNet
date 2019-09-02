using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FamilyTree.Infra.Models
{
    public class Individual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public string Firstnames { get; set; }

        public string Lastname { get; set; }

        public Char Sex { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DeathDate { get; set; }
        public string DeathPlace { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets the families where this individual is a spouse.
        /// </summary>
        /// <value>
        /// The spouse families.
        /// </value>
        public List<SpouseRelation> SpouseFamilies { get; set; } 

        /// <summary>
        /// Gets the families where this individual is a child.
        /// </summary>
        /// <value>
        /// The child families.
        /// </value>
        public List<ChildRelation> ChildFamilies { get; set; }

    }
}
