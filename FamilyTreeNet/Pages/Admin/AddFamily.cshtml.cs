using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FamilyTreeNet.Support;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    [Authorize(Roles = RoleNames.Editors)]
    public class AddFamilyModel : PageModel
    {
        public IActionResult OnGet()
        {
            PageStack.PushPage(HttpContext.Session, "/Admin/AddFamily", "");

            // TODO
            return RedirectToPage("Back");
        }
    }
}