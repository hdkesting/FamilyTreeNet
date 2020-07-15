using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    /// <summary>
    /// The Model class for the Person page, showing details about a person and his/her lineage.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [AllowAnonymous]
    public class PersonModel : PageModel
    {
        private readonly TreeService treeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonModel"/> class.
        /// </summary>
        /// <param name="treeService">The tree service.</param>
        public PersonModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        /// <summary>
        /// Gets the primary person.
        /// </summary>
        /// <value>
        /// The primary.
        /// </value>
        public IndividualVm Primary { get; private set; }

        /// <summary>
        /// Gets the family of the parents of the primary.
        /// </summary>
        /// <value>
        /// The parent family.
        /// </value>
        public FamilyVm ParentFamily { get; private set; } = new FamilyVm();

        /// <summary>
        /// Gets the siblings of the primary.
        /// </summary>
        /// <value>
        /// The siblings.
        /// </value>
        public List<IndividualVm> Siblings { get; } = new List<IndividualVm>();

        /// <summary>
        /// Gets or sets the family of the maternal grandparents.
        /// </summary>
        /// <value>
        /// The maternal grandparents.
        /// </value>
        public FamilyVm MaternalGrandparents { get; set; } = new FamilyVm();

        /// <summary>
        /// Gets or sets the family of the paternal grandparents.
        /// </summary>
        /// <value>
        /// The paternal grandparents.
        /// </value>
        public FamilyVm PaternalGrandparents { get; set; } = new FamilyVm();

        /// <summary>
        /// Gets any marriages of the primary.
        /// </summary>
        /// <value>
        /// The marriages.
        /// </value>
        public List<FamilyVm> Marriages { get; } = new List<FamilyVm>();

        /// <summary>
        /// Called when a GET request occurs.
        /// </summary>
        /// <param name="id">The identifier of the primary.</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGet(long id)
        {
            var prim = await this.treeService.GetIndividualById(id).ConfigureAwait(false);

            if (prim == null)
            {
                // couldn't find this person so back (?) to the FamilyNames page.
                return RedirectToPage("FamilyNames");
            }

            Primary = new IndividualVm(prim);

            foreach(var spouseFam in await this.treeService.GetSpouseFamiliesByIndividualId(id, false).ConfigureAwait(false))
            {
                Marriages.Add(new FamilyVm(spouseFam));
            }

            var childFams = await this.treeService.GetChildFamiliesByIndividualId(id, false).ConfigureAwait(false);
            if (childFams.Any())
            {
                ParentFamily = new FamilyVm(childFams.First());
                SetSiblings();

                if (ParentFamily.Husband != null) // of id != 0 ?
                {
                    var fams = await this.treeService.GetChildFamiliesByIndividualId(ParentFamily.Husband.Id, false).ConfigureAwait(false);
                    if (fams.Any())
                    {
                        PaternalGrandparents = new FamilyVm(fams.First());
                    }
                }

                if (ParentFamily.Wife != null) // of id != 0 ?
                {
                    var fams = await this.treeService.GetChildFamiliesByIndividualId(ParentFamily.Wife.Id, false).ConfigureAwait(false);
                    if (fams.Any())
                    {
                        MaternalGrandparents = new FamilyVm(fams.First());
                    }
                }

                SortData();
            }

            return Page();
        }

        private void SetSiblings()
        {
            this.Siblings.AddRange(ParentFamily.Children.Where(i => i.Id != Primary.Id));
        }

        private void SortData()
        {
            Siblings.Sort(DateComparer);

            foreach (var fam in Marriages)
            {
                fam.Children.Sort(DateComparer);
            }
        }

        private static int DateComparer(IndividualVm a, IndividualVm b)
        {
            if (a.BirthDate == null)
            {
                return b.BirthDate == null ? 0 : 1;
            }

            if (b.BirthDate == null)
            {
                return -1;
            }

            return a.BirthDate.CompareTo(b.BirthDate);
        }
    }
}