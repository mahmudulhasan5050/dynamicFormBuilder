using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.MyForms
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<MyForm> myForm { get; set; }

        [BindProperty]
        public bool ShowAll { get; set; }
        public async Task OnGetAsync()
        {
            myForm = await GetFormAsync(false);
        }

        public async Task<IActionResult> OnPostAsync()
        {
      
            myForm = await GetFormAsync(ShowAll);
            return Page();
        }

        private async Task<List<MyForm>> GetFormAsync(bool showAll)
        {
            if (!ShowAll)
            {
                var query = _db.MyForms.Where(u=>u.IsInUse == true).Include(u => u.Category).Include(u=>u.Country);
                return await query.ToListAsync();
            }
            else
            {
                var query = _db.MyForms.Include(u => u.Category).Include(u => u.Country);
                return await query.ToListAsync();
            }
        }
    }
}
