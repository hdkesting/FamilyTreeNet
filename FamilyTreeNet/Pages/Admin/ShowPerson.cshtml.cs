using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.Support;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = RoleNames.Editors)]
    public class ShowPersonModel : PageModel
    {
        private readonly TreeService treeService;

        public ShowPersonModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public IndividualVm Primary { get; set; }

        public FamilyVm ParentFamily { get; set; } = new FamilyVm();

        public List<IndividualVm> Siblings { get; } = new List<IndividualVm>();

        public List<FamilyVm> Marriages { get; } = new List<FamilyVm>();


        public async Task<IActionResult> OnGet(long id, int primary)
        {
            PageStack.PushPage(HttpContext.Session, $"/Admin/ShowPerson/{id}", "");
            var prim = await this.treeService.GetIndividualById(id).ConfigureAwait(false);

            if (prim == null)
            {
                return RedirectToPage("Back");
            }

            Primary = new IndividualVm(prim);

            foreach (var spouseFam in await this.treeService.GetSpouseFamiliesByIndividualId(id, false).ConfigureAwait(false))
            {
                Marriages.Add(new FamilyVm(spouseFam));
            }

            var childFams = await this.treeService.GetChildFamiliesByIndividualId(id, false).ConfigureAwait(false);
            if (childFams.Any())
            {
                ParentFamily = new FamilyVm(childFams.First());
                SetSiblings();

                // skip grandparents
 
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