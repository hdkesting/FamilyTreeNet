using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Reports
{
    public class PersonModel : PageModel
    {
        private readonly TreeService treeService;

        public PersonModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public IndividualVm Primary { get; set; }

        public FamilyVm ParentFamily { get; set; } = new FamilyVm();

        public List<IndividualVm> Siblings { get; } = new List<IndividualVm>();

        public FamilyVm MaternalGrandparents { get; set; } = new FamilyVm();

        public FamilyVm PaternalGrandparents { get; set; } = new FamilyVm();

        public List<FamilyVm> Marriages { get; } = new List<FamilyVm>();

        public async Task<IActionResult> OnGet(long id)
        {
            var prim = await this.treeService.GetIndividualById(id);

            if (prim == null)
            {
                return RedirectToPage("FamilyNames");
            }

            Primary = new IndividualVm(prim);

            foreach(var spouseFam in await this.treeService.GetSpouseFamiliesByIndividualId(id, false))
            {
                Marriages.Add(new FamilyVm(spouseFam));
            }

            var childFams = await this.treeService.GetChildFamiliesByIndividualId(id, false);
            if (childFams.Any())
            {
                ParentFamily = new FamilyVm(childFams.First());
                SetSiblings();

                if (ParentFamily.Husband != null) // of id != 0 ?
                {
                    var fams = await this.treeService.GetChildFamiliesByIndividualId(ParentFamily.Husband.Id, false);
                    if (fams.Any())
                    {
                        PaternalGrandparents = new FamilyVm(fams.First());
                    }
                }

                if (ParentFamily.Wife != null) // of id != 0 ?
                {
                    var fams = await this.treeService.GetChildFamiliesByIndividualId(ParentFamily.Wife.Id, false);
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

            return a.BirthDate.Value.CompareTo(b.BirthDate.Value);
        }

    }
}