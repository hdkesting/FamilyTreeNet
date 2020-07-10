namespace FamilyTree.Infra.MySql.Models
{
    public class SpouseRelation
    {
        public long SpouseId { get; set; }

        public Individual Spouse { get; set; }

        public long SpouseFamilyId { get; set; }

        public Family SpouseFamily { get; set; }
    }
}
