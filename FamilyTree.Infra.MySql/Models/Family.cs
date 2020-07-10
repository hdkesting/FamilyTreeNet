using System.Collections.Generic;

namespace FamilyTree.Infra.MySql.Models
{
    public class Family
    {
        public long Id { get; set; }

        public int? MarriageDateInt { get; set; }
        public string MarriagePlace { get; set; }

        public int? DivorceDateInt { get; set; }
        public string DivorcePlace { get; set; }

        public bool IsDeleted { get; set; }

        public List<SpouseRelation> Spouses { get; } = new List<SpouseRelation>();

        public List<ChildRelation> Children { get; } = new List<ChildRelation>();

    }
}
