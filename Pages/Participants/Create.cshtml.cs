using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.Participants
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Participant participant { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            participant.CreatedAt = participant.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (!_db.Participants.Any(o => o.Name == participant.Name))
                {
                    _db.Participants.Add(participant);
                    _db.SaveChanges();
                    TempData["success"] = "Participant created successfully!";
                    return RedirectToPage("Index");
                }
                TempData["error"] = "Category name already exists!";
            }
            return Page();
        }
    }
}
