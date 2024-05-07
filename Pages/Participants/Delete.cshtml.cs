using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.Participants
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
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
            var participantFromDb = _db.Participants.Find(participant.Id);

            if (participantFromDb != null)
            {
                _db.Participants.Remove(participantFromDb);
                _db.SaveChanges();
                TempData["success"] = "Category item has been deleted.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
