using FamilyTreeNet.Support;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyTreeNet.Pages.Admin
{
    /// <summary>
    /// Model class for Index page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
            // no http context yet
        }

        public bool IsAdmin { get; set; }

        public bool IsEditor { get; set; }

        public IActionResult OnGet()
        {
            // now I do have an http context
            var user = HttpContext.User;
            IsAdmin = user?.IsInRole(RoleNames.Administrators) ?? false;
            IsEditor = user?.IsInRole(RoleNames.Editors) ?? false;

            return Page();
        }
    }
}