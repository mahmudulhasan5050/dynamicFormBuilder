using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FormWizard.Pages.QuestionView
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Question> questions { get; set; }

        [BindProperty]
        public IEnumerable<QuestionOption> questionOptions { get; set; }
        [BindProperty]
        public IEnumerable<QuestionCondition> questionCondition { get; set; }
        [BindProperty]
        public IEnumerable<QuestionConditionOption> questionConditionOption { get; set; }

        public IActionResult OnGet(int myformid)
        {
            questions = _db.Questions.Where(u => u.MyFormId == myformid);
            questionOptions = _db.QuestionOptions.Where(r => questions.Any(u => u.Id == r.QuestionId)).ToList();
            questionCondition = _db.QuestionConditions.Where(r=> questions.Any(u => u.Id == r.QuestionId)).ToList();
            questionConditionOption = _db.QuestionConditionsOptions.Where(r => questionCondition.Any(u=> u.Id == r.QuestionConditionId));
 
            return Page();
        }
    }
}
