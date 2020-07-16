using System.Threading.Tasks;

using FamilyTreeNet.Core.Services;
using FamilyTreeNet.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    /// <summary>
    /// Model class for the "delete person" page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [Authorize(Roles = RoleNames.Editors)]
    public class DeletePersonModel : PageModel
    {
        private readonly TreeService treeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePersonModel"/> class.
        /// </summary>
        /// <param name="treeService">The tree service.</param>
        public DeletePersonModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        /// <summary>
        /// Gets the identifier of the individual to delete.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; private set; }

        /// <summary>
        /// Gets the full name of the individual.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; private set; }

        /// <summary>
        /// Called when a GET request occurs.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGet(long id)
        {
            var indi = await this.treeService.GetIndividualById(id).ConfigureAwait(false);

            if (indi == null)
            {
                return RedirectToPage("Search");
            }

            this.Id = id;
            this.FullName = $"{indi.Firstnames} {indi.Lastname}";

            return Page();
        }

        /// <summary>
        /// Called when a POST request occurs, to confirm the deletion.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="confirmid">The confirmid.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost(long id, long confirmid = -1)
        {
            if (id >= 0 && id == confirmid)
            {
                await this.treeService.DeleteIndividualById(id).ConfigureAwait(false);
            }

            return RedirectToPage("Search");
        }
    }
}