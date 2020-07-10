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
        /// <param name="includeDeleted">if set to <c>true</c>, also include soft-deleted ones.</param>
        /// <returns></returns>
        Task<int> Count(bool includeDeleted);

        /// <summary>
        /// Deletes all families (hard delete).
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

        /// <summary>
        /// Gets the families (including spouses and children) where the individual (see id) is spouse.
        /// </summary>
        /// <param name="id">The spouse's identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c>, also include deleted.</param>
        /// <returns></returns>
        Task<List<FamilyDto>> GetSpouseFamiliesByIndividualId(long id, bool includeDeleted);

        /// <summary>
        /// Gets the families (including spouses and children) where the individual (see id) is child.
        /// </summary>
        /// <param name="id">The child's identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c>, also include deleted.</param>
        /// <returns></returns>
        Task<List<FamilyDto>> GetChildFamiliesByIndividualId(long id, bool includeDeleted);

        /// <summary>
        /// Adds a spouse to a family.
        /// </summary>
        /// <param name="sfam">The ID of the spouse's family.</param>
        /// <param name="id">The ID of the spouse.</param>
        /// <returns></returns>
        Task AddSpouse(long sfam, long id);

        /// <summary>
        /// Adds a child to a family.
        /// </summary>
        /// <param name="cfam">The ID of the child's family.</param>
        /// <param name="id">The ID of the child.</param>
        /// <returns></returns>
        Task AddChild(long cfam, long id);
    }
}
