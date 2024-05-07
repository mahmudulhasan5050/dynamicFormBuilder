using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.QuestionConditionOptions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<QuestionConditionOption> questionConditionOptions { get; set; }
        public void OnGet(int questionconditionid, int questionid, int myformid)
        {
            questionConditionOptions = _db.QuestionConditionsOptions.Where(u => u.QuestionConditionId == questionconditionid).Include(p => p.QuestionCondition);
            ViewData["QuestionId"] = questionid;
            ViewData["MyFormId"] = myformid;
            ViewData["QuestionConditionId"] = questionconditionid;
        }
    }
}
