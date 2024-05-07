using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.QuestionConditions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<QuestionCondition> questionConditions { get; set; }
        public void OnGet(int questionid, int myformid)
        {
            questionConditions = _db.QuestionConditions.Where(x => x.QuestionId == questionid).Include(q => q.Question);
            ViewData["QuestionId"] = questionid;
            ViewData["MyFormId"] = myformid;
        }
    }
}
