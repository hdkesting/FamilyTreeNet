using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;

namespace FamilyTreeNet.Core.Interfaces
{
    public interface IIndividualRepository
    {
        /// <summary>
        /// Counts the individuals, possibly including soft-deleted ones.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c>, also include soft-deleted ones.</param>
        /// <returns></returns>
        Task<int> Count(bool includeDeleted);

        /// <summary>
        /// Gets the total spouse count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalSpouseCount();
        Task<List<NameCount>> GetLastNames();

        /// <summary>
        /// Gets the total children count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetTotalChildrenCount();

        /// <summary>
        /// Gets the individual by their identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c>, also include a deleted one.</param>
        /// <returns></returns>
        Task<IndividualDto> GetById(long id, bool includeDeleted);

        /// <summary>
        /// Searches for individuals by firstname and/or lastname.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <returns></returns>
        Task<IEnumerable<IndividualDto>> SearchByName(string firstname, string lastname);

        /// <summary>
        /// Marks the individual as deleted (soft delete).
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task MarkIndividualAsDeleted(long id);

        /// <summary>
        /// Deletes all individuals from the database (hard delete).
        /// </summary>
        /// <returns></returns>
        Task DeleteAll();

        /// <summary>Gets the individuals by their lastname.</summary>
        /// <param name="name">The exact name to search for.</param>
        /// <returns></returns>
        Task<List<IndividualDto>> GetIndividualsByLastname(string name);

        /// <summary>
        /// Adds or updates the individual.
        /// </summary>
        /// <param name="individual">The individual.</param>
        /// <returns></returns>
        Task AddOrUpdate(IndividualDto individual);
    }
}
