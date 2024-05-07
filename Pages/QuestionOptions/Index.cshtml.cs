using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.QuestionOptions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<QuestionOption> questionOption { get; set; }
        public void OnGet(int questionid, string questiontype, int myformid)
        {

            questionOption = _db.QuestionOptions.Where(x=> x.QuestionId == questionid).Include(x => x.Question);
            ViewData["QuestionId"] = questionid;
            ViewData["QuestionType"] = questiontype;
            ViewData["MyFormId"] = myformid;
        }
    }
}
