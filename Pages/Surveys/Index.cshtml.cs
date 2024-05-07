using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.Surveys
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Participant> participants { get; set; }
        [BindProperty]
        public int participantId { get; set; }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<SelectListItem> participantList { get; set; }

        public void OnGet(int myformid)
        {
            participants = _db.Participants;
            if (participants != null)
            {
                participantList = participants.Select(u =>
               new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }
               ).ToList();

                ViewData["ParticipantList"] = participantList;

            }
            
            TempData["MyFormId"] = myformid;
            TempData["CurrentQuestionIndex"] = 1;
        }

        public IActionResult OnPost()
        {
            bool participantExists = _db.Answers.Any(u => u.ParticipantId == participantId && u.MyFormId == (int)TempData["MyFormId"]!);
            if (!participantExists) {
            
            return RedirectToPage("Create", new { myformid = TempData["MyFormId"], currentquestionindex = TempData["CurrentQuestionIndex"], participantid = participantId });
            }

            ViewData["ParticipantList"] = participantList;
            TempData["success"] = "Participant already answered.";
            return RedirectToPage("Index", new { myformid = TempData["MyFormId"] });

        }
    }
}
