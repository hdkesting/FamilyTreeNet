using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilyTree.Infra.Models
{
    public class Family
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MarriageDate { get; set; }
        public string MarriagePlace { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DivorceDate { get; set; }
        public string DivorcePlace { get; set; }

        public bool IsDeleted { get; set; }

        public List<SpouseRelation> Spouses { get; } = new List<SpouseRelation>();

        public List<ChildRelation> Children { get; } = new List<ChildRelation>();

    }
}
