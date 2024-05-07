using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.Answers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Answer> answers { get; set; }
        public IEnumerable<AnswerCondition> answerConditions { get; set; }
        public List<string> participantIds { get; set; }
        public string myFormName { get; set; } 
        public void OnGet(int myformid)
        {
            answers = _db.Answers.Where(u=>u.MyFormId==myformid).Include(p=> p.Participant).Include(p=>p.MyForm).Include(p=> p.Question).ToList();
            participantIds = answers.Select(u => u.Participant.Name).Distinct().ToList();
            myFormName = answers.FirstOrDefault()?.MyForm.Name ?? myFormName;
            answerConditions = _db.AnswerConditions.Where(u=> u.MyFormId==myformid).Include(p=>p.Participant).Include(p=>p.QuestionCondition).ToList();
        }
    }
}
