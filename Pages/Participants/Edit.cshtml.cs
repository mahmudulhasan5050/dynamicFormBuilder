using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.Participants
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public Participant participant { get; set; }
        public void OnGet(int Id)
        {
            participant = _db.Participants.FirstOrDefault(u => u.Id == Id);
        }

        public IActionResult OnPost()
        {
            participant.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.Participants.Update(participant);
                _db.SaveChanges();
                TempData["success"] = "Participant has been updated.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
