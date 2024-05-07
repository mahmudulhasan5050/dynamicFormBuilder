using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace FormWizard.Pages.Surveys
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Answer answer { get; set; }
        //[BindProperty]
        //public IEnumerable<Question> questions { get; set; }
        [BindProperty]
        public Question currentQuestion { get; set; }
        [BindProperty]
        public IList<QuestionOption> questionOptions { get; set; }
        [BindProperty]
        public List<QuestionCondition> questionConditions { get; set; }
        [BindProperty]
        public List<QuestionConditionOption> questionConditionOptions { get; set; }
        [BindProperty]
        public List<AnswerCondition> answerConditions { get; set; }



        [BindProperty]
        public int myFormId { get; set; }
        [BindProperty]
        public int participantId { get; set; }
        [BindProperty]
        public int currentQuestionIndex { get; set; }
        [BindProperty]
        public int currentQuestionId { get; set; }
        [BindProperty]
        public List<string> SelectedCheckboxAnswers { get; set; }
        [BindProperty]
        public string SelectedRadioButtonAnswer { get; set; } = "";
        public int counted { get; set; }



        public void OnGet(int myformid, int currentquestionindex, int participantid)
        {
            myFormId = myformid;
            currentQuestionIndex = currentquestionindex;
            TempData["CurrentQuestionIndex"] = currentquestionindex;
            participantId = participantid;
            TempData["ParticipantId"] = participantid;


            IEnumerable<Question> questions = _db.Questions.Where(x => x.MyFormId == myFormId && x.IsInUse == true).OrderBy(y => y.OrderOfDisplay).ToList();
            counted = questions.Count();
            currentQuestion = questions?.FirstOrDefault(u => u.OrderOfDisplay == currentQuestionIndex) ?? currentQuestion;

            currentQuestionId = currentQuestion.Id;
            TempData["CurrentQuestionId"] = currentQuestionId;
            questionOptions = _db.QuestionOptions.Where(u => u.QuestionId == currentQuestion.Id).ToList();


            if (currentQuestion.Type == Question.QuestionType.list)
            {
                IEnumerable<SelectListItem> listItems = questionOptions.Select(u => new SelectListItem
                {
                    Text = u.OptionText,
                    Value = u.OptionText
                }).ToList();
                ViewData["ListItems"] = listItems;
            }

            if (currentQuestion.Extension == true)
            {
                questionConditions = _db.QuestionConditions.Where(u => u.QuestionId == currentQuestion.Id).ToList();
                questionConditionOptions = _db.QuestionConditionsOptions.AsEnumerable().Where(r => questionConditions.Any(u => u.Id == r.QuestionConditionId)).ToList();

                answerConditions = new List<AnswerCondition>();
                for (int i = 0; i < questionConditions.Count; i++)
                {
                    answerConditions.Add(new AnswerCondition());

                }

            }


        }


        public async Task<IActionResult> OnPostAsync(IFormCollection? form)
        {
            currentQuestionId = (int)TempData["CurrentQuestionId"]!;
           participantId = (int)TempData["ParticipantId"]!;

            currentQuestion = _db.Questions.Find(currentQuestionId) ?? currentQuestion;
            if (currentQuestion == null)
            {
                return NotFound();
            }

            if (Request.Form["Next"].Count > 0)
            {


                await SaveDataAsync(answer, SelectedCheckboxAnswers, SelectedRadioButtonAnswer);

                if (currentQuestion.Extension == true)
                {
                    await SaveConditionAnswersAsync(answerConditions, form);
                }

                TempData["success"] = "Saved";
                currentQuestionIndex = currentQuestionIndex + 1;
                return RedirectToPage("Create", new { myformid = myFormId, currentquestionindex = currentQuestionIndex, participantid = participantId });

            }
            else if (Request.Form["Previous"].Count > 0)
            {
                currentQuestionIndex = currentQuestionIndex - 1;
                return RedirectToPage("Edit", new { myformid = myFormId, currentquestionindex = currentQuestionIndex });

            }
            else if (Request.Form["Finish"].Count > 0)
            {
                await SaveDataAsync(answer, SelectedCheckboxAnswers, SelectedRadioButtonAnswer);
                if (currentQuestion.Extension == true)
                {
                    await SaveConditionAnswersAsync(answerConditions, form);
                }
                TempData["success"] = "Done";
                return RedirectToPage("ThankYouPage");

            }

            return Page();

        }


        private async Task SaveDataAsync(Answer answer, List<string> SelectedCheckboxAnswers, string SelectedRadioButtonAnswer)
        {

            answer.CreatedAt = answer.UpdatedAt = DateTime.Now;
            if (SelectedCheckboxAnswers.Count != 0)
            {
                string selectedItems = string.Join(",", SelectedCheckboxAnswers);
                answer.QuestionAnswer = selectedItems;
                answer.ParticipantId = participantId;
                _db.Answers.Add(answer);

            }
            else if (SelectedRadioButtonAnswer != "")
            {
                answer.QuestionAnswer = SelectedRadioButtonAnswer;
                answer.ParticipantId = participantId;
                _db.Answers.Add(answer);
            }
            else
            {
                _db.Answers.Add(answer);
            }


            await _db.SaveChangesAsync();


        }


        private async Task SaveConditionAnswersAsync(List<AnswerCondition> answerConditions, IFormCollection form)
        {
            foreach (AnswerCondition answerCondition in answerConditions)
            {
                answerCondition.CreatedAt = answerCondition.UpdatedAt = DateTime.Now;
                if (answerCondition.ConditionAnswer == null)
                {
                    foreach (var key in form.Keys)
                    {
                        if (key.StartsWith("SelectedRadioButtonAnswerCondition_"))
                        {
                            int conditionQuestionId = int.Parse(key.Replace("SelectedRadioButtonAnswerCondition_", ""));
                            var value = form[key];
                            if (answerCondition.QuestionConditionId == conditionQuestionId)
                            {
                                if (value.Count == 1)
                                {
                                    answerCondition.ConditionAnswer = value[0];
                                    _db.AnswerConditions.Add(answerCondition);
                                    await _db.SaveChangesAsync();
                                }
                                //else
                                //{
                                //    answerCondition.ConditionAnswer = "";
                                //    _db.AnswerConditions.Add(answerCondition);
                                //    await _db.SaveChangesAsync();
                                //}
                            }
                         
                      

                        }
                        else if (key.StartsWith("SelectedCheckboxButtonAnswerCondition_"))
                        {
                            int conditionQuestionId = int.Parse(key.Replace("SelectedCheckboxButtonAnswerCondition_", ""));
                            string[] values = form[key].ToArray();
                            string value = string.Join(",", values);
                            if (answerCondition.QuestionConditionId == conditionQuestionId)
                            {
                                if(value != null)
                                {
                                    answerCondition.ConditionAnswer = value;
                                    _db.AnswerConditions.Add(answerCondition);
                                    await _db.SaveChangesAsync();
                                }
                                //else
                                //{
                                //    answerCondition.ConditionAnswer = "";
                                //    _db.AnswerConditions.Add(answerCondition);
                                //    await _db.SaveChangesAsync();
                                //}

                            }

                        }
                        //else
                        //{
                        //    answerCondition.ConditionAnswer = "";
                        //    _db.AnswerConditions.Add(answerCondition);
                        //    await _db.SaveChangesAsync();
                        //}
                    }

                }
                else
                {
                    _db.AnswerConditions.Add(answerCondition);
                    await _db.SaveChangesAsync();
                }
            }

        }

    }
}


