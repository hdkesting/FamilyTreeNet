using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeNet.Core.Interfaces
{
    public interface IIndividualRepository
    {
        Task<int> Count(bool includeDeleted);

        Task<int> GetTotalSpouseCount();

        Task<int> GetTotalChildrenCount();

        Task DeleteAll();

    }
}
