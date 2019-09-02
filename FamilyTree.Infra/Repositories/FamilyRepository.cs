using FamilyTree.Infra.Models;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FamilyTree.Infra.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly FamilyTreeContext context;

        public FamilyRepository(FamilyTreeContext context)
        {
            this.context = context;
        }

        public Task<int> Count() =>
            this.context.Families.CountAsync(f => !f.IsDeleted);

        public Task<int> DeleteAll() => throw new NotImplementedException();

        public Task<int> GetTotalChildrenCount() =>
            this.context.Individuals
            .Where(i => !i.IsDeleted)
            .CountAsync(i => i.ChildFamilies.Count != 0);

        public Task<int> GetTotalSpouseCount() =>
            this.context.Individuals
            .Where(i => !i.IsDeleted)
            .CountAsync(i => i.SpouseFamilies.Count != 0);
    }
}
