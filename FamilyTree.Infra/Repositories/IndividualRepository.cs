using FamilyTree.Infra.Models;
using FamilyTreeNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FamilyTree.Infra.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        private readonly FamilyTreeContext context;

        public IndividualRepository(FamilyTreeContext context)
        {
            this.context = context;
        }

        public Task<int> Count(bool includeDeleted) =>
            this.context.Individuals.CountAsync(f => includeDeleted || !f.IsDeleted);

        public async Task DeleteAll()
        {
            this.context.Individuals.RemoveRange(this.context.Individuals);
            await this.context.SaveChangesAsync();
        }

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
