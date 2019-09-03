using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;

namespace FamilyTreeNet.Core.Interfaces
{
    public interface IIndividualRepository
    {
        /// <summary>
        /// Counts the individuals, possibly including deleted ones.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
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

        Task<IndividualDto> GetById(long id, bool includeDeleted);

        /// <summary>
        /// Deletes all individuals.
        /// </summary>
        /// <returns></returns>
        Task DeleteAll();
        Task<List<IndividualDto>> GetIndividualsByLastname(string name);

        /// <summary>
        /// Adds or updates the individual.
        /// </summary>
        /// <param name="individual">The individual.</param>
        /// <returns></returns>
        Task AddOrUpdate(IndividualDto individual);
    }
}
