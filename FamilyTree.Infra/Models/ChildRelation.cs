using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FamilyTree.Infra.Models
{
    public class ChildRelation
    {
        public long ChildId { get; set; }

        public Individual Child { get; set; }

        public long ChildFamilyId { get; set; }

        public Family ChildFamily { get; set; }
    }
}
