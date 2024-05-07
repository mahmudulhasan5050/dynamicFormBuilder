using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FormWizard.Pages.Categories
{
    //[DbContext(typeof(Data.ApplicationDbContext))]
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Category> Categories { get; set; }
        public void OnGet()
        {
            Categories = _db.Category;
        }
    }
}
