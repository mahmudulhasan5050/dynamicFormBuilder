using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.Countries
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Country> countries { get; set; }
        public void OnGet()
        {
            countries = _db.Country;
        }
    }
}
