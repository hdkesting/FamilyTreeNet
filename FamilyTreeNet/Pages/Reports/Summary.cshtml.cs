using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    /// <summary>
    /// The Model class for the Summary page, showing counts of the stored data.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [AllowAnonymous]
    public class SummaryModel : PageModel
    {
        private readonly TreeService treeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryModel"/> class.
        /// </summary>
        /// <param name="treeService">The tree service.</param>
        public SummaryModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        /// <summary>
        /// Gets the number of individuals stored.
        /// </summary>
        /// <value>
        /// The individuals.
        /// </value>
        public int Individuals { get; private set; }

        /// <summary>
        /// Gets the number of families stored.
        /// </summary>
        /// <value>
        /// The families.
        /// </value>
        public int Families { get; private set; }

        /// <summary>
        /// Gets the number of spouses stored.
        /// </summary>
        /// <value>
        /// The spouses.
        /// </value>
        public int Spouses { get; private set; }

        /// <summary>
        /// Gets the number of children stored.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public int Children { get; private set; }

        /// <summary>
        /// Called when a GET request occurs.
        /// </summary>
        public async Task OnGet()
        {
            var summary = await this.treeService.GetCountSummary().ConfigureAwait(false);

            this.Individuals = summary.IndividualCount;
            this.Families = summary.FamilyCount;
            this.Spouses = summary.SpouseCount;
            this.Children = summary.ChildCount;
        }
    }
}