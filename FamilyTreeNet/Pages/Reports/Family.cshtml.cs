using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    /// <summary>
    /// The Model class for the Family page, listing all people with the same last name.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [AllowAnonymous]
    public class FamilyModel : PageModel
    {
        private readonly TreeService treeService;

        /// <summary>Initializes a new instance of the <see cref="FamilyModel" /> class.</summary>
        /// <param name="treeService">The tree service.</param>
        public FamilyModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        /// <summary>
        /// Gets the lastname of this family.
        /// </summary>
        /// <value>
        /// The lastname.
        /// </value>
        public string Lastname { get; private set; }

        /// <summary>
        /// Gets the individuals in this family.
        /// </summary>
        /// <value>
        /// The individuals.
        /// </value>
        public List<IndividualVm> Individuals { get; } = new List<IndividualVm>();

        /// <summary>Called when a GET request arrives.</summary>
        /// <param name="name">The (last-) name to search for.</param>
        public async Task OnGet(string name)
        {
            this.Lastname = name;
            var list = await this.treeService.GetIndividualsByLastname(name).ConfigureAwait(false);

            this.Individuals.Clear();
            this.Individuals.AddRange(list.Select(i => new IndividualVm(i))); 
        }
    }
}