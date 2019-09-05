using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    public class EditPersonModel : PageModel
    {
        private readonly TreeService treeService;

        public EditPersonModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        public IndividualVm Person { get; set; }
        public long Id { get; private set; }
        public long Primary { get; private set; }

        public async Task<IActionResult> OnGet(long id, long primary)
        {
            var indi = await this.treeService.GetIndividualById(id).ConfigureAwait(false);
            if (indi == null)
            {
                return RedirectToPage("Index");
            }

            this.Id = id;
            this.Primary = primary;
            this.Person = new IndividualVm(indi);
            return Page();
        }

        public async Task<IActionResult> OnPost(long id, IndividualVm person, long primary)
        {
            if (person == null || person.Id != id)
            {
                return RedirectToPage("Index");
            }

            var indi = await this.treeService.GetIndividualById(id).ConfigureAwait(false);
            indi.BirthDate = person.BirthDate;
            indi.BirthPlace = person.BirthPlace;
            indi.DeathDate = person.DeathDate;
            indi.DeathPlace = person.DeathPlace;
            indi.Firstnames = person.Firstnames;
            indi.Lastname = person.Lastname;
            indi.Sex = person.Sex;

            await this.treeService.Update(indi).ConfigureAwait(false);

            if (primary == 0)
            {
                return RedirectToPage("Search", new { indi.Lastname });
            }
            else
            {
                return RedirectToPage("ShowPerson", new { Id = primary });
            }
        }
    }
}