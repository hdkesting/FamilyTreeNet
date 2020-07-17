using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeNet.Core.Dto;
using FamilyTreeNet.Core.Services;
using FamilyTreeNet.Support;
using FamilyTreeNet.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    /// <summary>
    /// Model class for the Edit Family page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [Authorize(Roles = RoleNames.Editors)]
    public class EditFamilyModel : PageModel
    {
        private readonly TreeService treeService;

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public MutableGeneaDate MarriageDate { get; set; }

        [BindProperty]
        public string MarriagePlace { get; set; }

        [BindProperty]
        public MutableGeneaDate DivorceDate { get; set; }

        [BindProperty]
        public string DivorcePlace { get; set; }

        public string FamilyName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditFamilyModel"/> class.
        /// </summary>
        /// <param name="treeService">The tree service.</param>
        public EditFamilyModel(TreeService treeService)
        {
            this.treeService = treeService;
        }

        /// <summary>
        /// Called when a GET request occurs.
        /// </summary>
        public async Task<IActionResult> OnGet()
        {
            PageStack.PushPage(HttpContext.Session, "/Admin/EditFamily", $"id={Id}");
            if (this.Id <= 0)
            {
                // create new family
                // todo: who are the spouses?
                this.MarriageDate = new MutableGeneaDate();
            }
            else
            {
                FamilyDto fam = await this.treeService.GetFamilyById(this.Id).ConfigureAwait(false);
                if (fam is null)
                {
                    // explicit ID not found
                    return RedirectToPage("Index");
                }

                this.MarriageDate = new MutableGeneaDate(fam.MarriageDate);
                this.MarriagePlace = fam.MarriagePlace;
                this.DivorceDate = new MutableGeneaDate(fam.DivorceDate);
                this.DivorcePlace = fam.DivorcePlace;

                this.FamilyName = string.Join(" and ", fam.Spouses.Select(sp => $"{sp.Firstnames}"));
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // this.Family.DivorceDate = value?.ToGeneaDate();
            FamilyDto fam = await this.treeService.GetFamilyById(this.Id).ConfigureAwait(false);

            if (fam != null)
            {
                fam.MarriageDate = this.MarriageDate.ToGeneaDate();
                fam.MarriagePlace = this.MarriagePlace;
                fam.DivorceDate = this.DivorceDate.ToGeneaDate();
                fam.DivorcePlace = this.DivorcePlace;

                await this.treeService.Update(fam).ConfigureAwait(false);
            }

            return RedirectToPage("Back"); 
        }
    }
}