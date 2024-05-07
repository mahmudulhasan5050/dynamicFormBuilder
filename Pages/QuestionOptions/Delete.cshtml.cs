using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace FormWizard.Pages.QuestionOptions
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionOption questionOption { get; set; }
        public void OnGet(int questionoptionid, int questionid, int myformid)
        {
            questionOption = _db.QuestionOptions.Find(questionoptionid);
            if (questionOption != null) {
                var dd = _db.Questions.FirstOrDefault(s => s.Id == questionOption.QuestionId);
            }
            ViewData["QuestionId"] = questionid;
            ViewData["MyFormId"] = myformid;
        }

        public async Task<IActionResult> OnPost()
        {
            
            var questionOptionFromDb = _db.QuestionOptions.Find(questionOption.Id);
            var question = _db.Questions.Find(questionOptionFromDb.QuestionId);
            if (questionOptionFromDb != null)
            {
                _db.QuestionOptions.Remove(questionOptionFromDb);
                await _db.SaveChangesAsync();
                TempData["success"] = "Question Option has been removed.";
                return RedirectToPage("Index", new { 
                questionid = question.Id,
                questiontype = question.Type,
                myformid = question.MyFormId
                });
            }
            return Page();
        }

    }
}
