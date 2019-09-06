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

        [BindProperty]
        public IndividualVm Person { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public long Primary { get; private set; }

        public long ChildFam { get; private set; }

        public long SpouseFam { get; private set; }

        public async Task<IActionResult> OnGet(long primary, long cfam, long sfam)
        {
            this.Primary = primary;
            this.ChildFam = cfam;
            this.SpouseFam = sfam;

            if (this.Id > 0)
            {
                var indi = await this.treeService.GetIndividualById(this.Id).ConfigureAwait(false);
                if (indi == null)
                {
                    return RedirectToPage("Index");
                }
                this.Person = new IndividualVm(indi);
            }
            else
            {
                this.Person = new IndividualVm();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(long primary, long cfam, long sfam)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            this.Primary = primary;

            if (this.Person == null || this.Person.Id != this.Id)
            {
                return RedirectToPage("Index");
            }

            var indi = this.Id==0 ? new Core.Dto.IndividualDto() : await this.treeService.GetIndividualById(this.Id).ConfigureAwait(false);
            indi.BirthDate = this.Person.BirthDate;
            indi.BirthPlace = this.Person.BirthPlace;
            indi.DeathDate = this.Person.DeathDate;
            indi.DeathPlace = this.Person.DeathPlace;
            indi.Firstnames = this.Person.Firstnames;
            indi.Lastname = this.Person.Lastname;
            indi.Sex = this.Person.Sex;

            if (this.Id == 0)
            {
                this.Id = await this.treeService.AddPerson(indi).ConfigureAwait(false);

                if (cfam > 0)
                {
                    await this.treeService.AddChild(cfam, this.Id).ConfigureAwait(false);
                }
                if (sfam > 0)
                {
                    await this.treeService.AddSpouse(sfam, Id).ConfigureAwait(false);
                }
            }
            else
            {
                await this.treeService.Update(indi).ConfigureAwait(false);
            }

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