using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Infra.MySql.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        public Task AddChild(long cfam, long id)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdate(FamilyDto family)
        {
            throw new NotImplementedException();
        }

        public Task AddSpouse(long sfam, long id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count(bool includeDeleted)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted)
        {
            throw new NotImplementedException();
        }

        public Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRelations(long familyId, List<long> spouses, List<long> children)
        {
            throw new NotImplementedException();
        }
    }
}
