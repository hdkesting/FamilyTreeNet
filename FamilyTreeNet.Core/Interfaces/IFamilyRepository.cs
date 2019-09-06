using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;

namespace FamilyTreeNet.Core.Interfaces
{
    public interface IFamilyRepository
    {
        /// <summary>
        /// Counts all families, possibly including the deleted ones.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        Task<int> Count(bool includeDeleted);

        /// <summary>
        /// Deletes all families.
        /// </summary>
        /// <returns></returns>
        Task DeleteAll();

        /// <summary>
        /// Adds or updates the family.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <returns></returns>
        Task AddOrUpdate(FamilyDto family);

        /// <summary>
        /// Updates the relations between the family and spouses and children.
        /// </summary>
        /// <param name="familyId">The family identifier.</param>
        /// <param name="spouses">The spouses.</param>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        Task UpdateRelations(long familyId, List<long> spouses, List<long> children);

        Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted);

        Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted);
        Task AddSpouse(long sfam, long id);
        Task AddChild(long cfam, long id);
    }
}
