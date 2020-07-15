using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    /// <summary>
    /// The Model class for the Family Names page, listing all known family (last-) names.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [AllowAnonymous]
    public class FamilyNamesModel : PageModel
    {
        private readonly TreeService treeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyNamesModel"/> class.
        /// </summary>
        /// <param name="treeService">The tree service.</param>
        public FamilyNamesModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        /// <summary>
        /// Gets all the know last names.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        public IEnumerable<NameCount> Names { get; private set; }

        /// <summary>
        /// Called when a GET request arrives.
        /// </summary>
        public async Task OnGet()
        {
            this.Names = await this.treeService.GetLastNames().ConfigureAwait(false);
        }
    }
}