using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;

namespace FormWizard.Pages.QuestionConditions
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;


        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public QuestionCondition questionCondition { get; set; }

        [BindProperty(SupportsGet = true)]
        public int questionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int myFormId { get; set; }
        public void OnGet(int questionconditionid, int questionid, int myformid)
        {
            questionCondition = _db.QuestionConditions.Find(questionconditionid);
            questionId = questionid;
            myFormId = myformid;
        }

        public async Task<IActionResult> OnPost()
        {
            var questionConditionOptionsFromDb = _db.QuestionConditionsOptions.Where(u => u.QuestionConditionId == questionCondition.Id);
 
            if (questionConditionOptionsFromDb == null)
            {
                NotFound();
            }
            if (questionConditionOptionsFromDb != null)
            {

                _db.QuestionConditionsOptions.RemoveRange(questionConditionOptionsFromDb);
                await _db.SaveChangesAsync();
            }

         
                _db.QuestionConditions.Remove(questionCondition);
                await _db.SaveChangesAsync();
                TempData["success"] = "Question Condition has been deleted.";
                return RedirectToPage("Index", new { questionid = questionId, myformid = myFormId });
            
        }
    }
}
