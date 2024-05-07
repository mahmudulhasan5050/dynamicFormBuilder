using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.MyForms
{
    
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public MyForm myForm { get; set; }
        public IQueryable<QuestionOption> questionOption { get; set; }
        public void OnGet(int myformid)
        {
            myForm = _db.MyForms.FirstOrDefault(u=>u.Id == myformid);
        }

        public async Task<IActionResult> OnPost()
        {
            var myFormFromDb = _db.MyForms.Find(myForm.Id);
            var questionsFromDb = _db.Questions.Where(u => u.MyFormId == myForm.Id).ToList();
            if (questionsFromDb == null)
            {
                NotFound();
            }
            if (questionsFromDb != null)
            {
                foreach (var ques in questionsFromDb)
                {
                    questionOption = _db.QuestionOptions.Where(u => u.QuestionId == ques.Id);
                    _db.QuestionOptions.RemoveRange(questionOption);
                    await _db.SaveChangesAsync();
                }
                _db.Questions.RemoveRange(questionsFromDb);
                await _db.SaveChangesAsync();
            }

            if (myFormFromDb != null)
            {
                _db.MyForms.Remove(myFormFromDb);
                await _db.SaveChangesAsync();
                TempData["success"] = "Form has been deleted.";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
