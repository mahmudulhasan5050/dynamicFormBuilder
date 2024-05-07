using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db; 
        }
        [BindProperty]
        public Category category { get; set; }
        public void OnGet(int Id)
        {
            category = _db.Category.FirstOrDefault(u=>u.Id == Id);
        }

        public IActionResult OnPost()
        {
            var categoryFromDb = _db.Category.Find(category.Id);

            if(categoryFromDb != null) {
                _db.Category.Remove(categoryFromDb);
                _db.SaveChanges();
                TempData["success"] = "Category item has been deleted.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
