using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree.Infra.MySql.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        public Task AddOrUpdate(IndividualDto individual)
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

        public Task<IndividualDto> GetById(long id, bool includeDeleted)
        {
            throw new NotImplementedException();
        }

        public Task<List<IndividualDto>> GetIndividualsByLastname(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<NameCount>> GetLastNames()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalChildrenCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalSpouseCount()
        {
            throw new NotImplementedException();
        }

        public Task MarkIndividualAsDeleted(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IndividualDto>> SearchByName(string firstname, string lastname)
        {
            throw new NotImplementedException();
        }
    }
}
