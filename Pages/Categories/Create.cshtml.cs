using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormWizard.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Category category { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(Category category)
        {
            category.CreatedAt = category.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (!_db.Category.Any(o => o.Name == category.Name))
                {
                    _db.Category.Add(category);
                    _db.SaveChanges();
                    TempData["success"] = "Category created successfully!";
                    return RedirectToPage("Index");
                }
                TempData["error"] = "Category name already exists!";
            }
            return Page();
        }
    }
}
