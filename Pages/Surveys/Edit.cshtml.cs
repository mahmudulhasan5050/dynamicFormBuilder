using FormWizard.Data;
using FormWizard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormWizard.Pages.Surveys
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditModel(ApplicationDbContext db)
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
        //[BindProperty]
        //public int questionId { get; set; }
        [BindProperty]
        public int currentQuestionIndex { get; set; }
        [BindProperty]
        public int currentQuestionId { get; set; }
        [BindProperty]
        public List<string> SelectedCheckboxAnswers { get; set; }
        [BindProperty]
        public string SelectedRadioButtonAnswer { get; set; } = "";
        //[BindProperty]
        //public string AnswerConditionForRadioButtonOption { get; set; }
      
        public int counted { get; set; }


        public async Task OnGet(int myformid, int currentquestionindex, int participantId)
        {
            myFormId = myformid;
            if (currentquestionindex <= 1)
            {
                currentQuestionIndex = 1;
            }
            else
            {
                currentQuestionIndex = currentquestionindex;
            }

            currentQuestion = _db.Questions.FirstOrDefault(u => u.MyFormId == myformid && u.OrderOfDisplay == currentQuestionIndex && u.IsInUse == true);
            answer = _db.Answers.FirstOrDefault(u => u.MyFormId == myFormId && u.QuestionId == currentQuestion.Id) ?? answer;
            questionOptions = _db.QuestionOptions.Where(u => u.QuestionId == currentQuestion.Id).ToList();
            currentQuestionId = currentQuestion.Id;
            TempData["CurrentQuestionId"] = currentQuestionId;

            if (currentQuestion.Type == Question.QuestionType.list)
            {
                IEnumerable<SelectListItem> listItems = questionOptions.Select(u => new SelectListItem
                {
                    Text = u.OptionText,
                    Value = u.OptionText
                }).ToList();
                ViewData["ListItems"] = listItems;
            }
            if (currentQuestion.Type == Question.QuestionType.radio)
            {
                SelectedRadioButtonAnswer = answer.QuestionAnswer;
            }

            if (currentQuestion.Type == Question.QuestionType.checkbox)
            {
                SelectedCheckboxAnswers = answer.QuestionAnswer.Split(',').ToList();

            }

            if (currentQuestion.Extension == true)
            {
                questionConditions = await _db.QuestionConditions.Where(u => u.QuestionId == currentQuestion.Id).ToListAsync();
                questionConditionOptions = _db.QuestionConditionsOptions.AsEnumerable().Where(r => questionConditions.Any(u => u.Id == r.QuestionConditionId)).ToList();


                answerConditions = _db.AnswerConditions.Include(u => u.QuestionCondition).Where(p => p.MyFormId == myFormId).AsEnumerable().Where(ac => questionConditions.Any(qc => qc.Id == ac.QuestionConditionId)).ToList();
                foreach (var answerCondition in answerConditions)
                {
                    if (answerCondition.ConditionAnswer != "")
                    {
                        TempData["AnswerConditionForRadioButtonOption"] = answerCondition.QuestionCondition.Value as string;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection? form)
        {
            currentQuestionId = (int)TempData["CurrentQuestionId"]!;

            currentQuestion = _db.Questions.Find(currentQuestionId) ?? currentQuestion;
            if (currentQuestion == null)
            {
                return NotFound();
            }

            if (Request.Form["Next"].Count > 0)
            {
                int nextAnswerId = answer.Id + 1;
                Answer nextAnswerFound = _db.Answers.Find(nextAnswerId); // to find the next answer if it is already

                await UpdateDataAsync(answer, SelectedCheckboxAnswers, SelectedRadioButtonAnswer);

                if (currentQuestion.Extension == true)
                {
                    await UpdateConditionAnswersAsync(answerConditions, form);
                }

                TempData["success"] = "Updated";

                if (nextAnswerFound != null)
                {
                    currentQuestionIndex = currentQuestionIndex + 1;
                    return RedirectToPage("Edit", new { myformid = myFormId, currentquestionindex = currentQuestionIndex });
                }
                currentQuestionIndex = currentQuestionIndex + 1;
                return RedirectToPage("Create", new { myformid = myFormId, currentquestionindex = currentQuestionIndex });
            }
            else if (Request.Form["Previous"].Count > 0)
            {
                await UpdateDataAsync(answer, SelectedCheckboxAnswers, SelectedRadioButtonAnswer);

                TempData["success"] = "Updated";
                currentQuestionIndex = currentQuestionIndex - 1;
                return RedirectToPage("Edit", new { myformid = myFormId, currentquestionindex = currentQuestionIndex });

            }

            return Page();
        }


        private async Task UpdateDataAsync(Answer answer, List<string> SelectedCheckboxAnswers, string SelectedRadioButtonAnswer)
        {

            answer.UpdatedAt = DateTime.Now;
            if (SelectedCheckboxAnswers.Count != 0)
            {
                string selectedItems = string.Join(",", SelectedCheckboxAnswers);
                answer.QuestionAnswer = selectedItems;
                _db.Answers.Update(answer);

            }
            else if (SelectedRadioButtonAnswer != "")
            {
                answer.QuestionAnswer = SelectedRadioButtonAnswer;
                _db.Answers.Update(answer);
            }
            else
            {
                _db.Answers.Update(answer);
            }


            await _db.SaveChangesAsync();


        }

        private async Task UpdateConditionAnswersAsync(List<AnswerCondition> answerConditions, IFormCollection form)
        {
            foreach (AnswerCondition answerCondition in answerConditions)
            {
                answerCondition.UpdatedAt = DateTime.Now;

                foreach (var key in form.Keys)
                {
                   
                    if ((string)TempData["AnswerConditionForRadioButtonOption"]! != SelectedRadioButtonAnswer)
                    {
                        if (answerCondition.ConditionAnswer == "")
                        {

                            if (key.StartsWith("SelectedRadioButtonAnswerCondition_"))
                            {

                                int conditionQuestionId = int.Parse(key.Replace("SelectedRadioButtonAnswerCondition_", ""));
                                var value = form[key];
                                if (answerCondition.QuestionConditionId == conditionQuestionId && value.Count() == 1)
                                {
                                    answerCondition.ConditionAnswer = value[0];
                                    _db.AnswerConditions.Update(answerCondition);
                                    await _db.SaveChangesAsync();
                                }
                                else
                                {
                                    answerCondition.ConditionAnswer = "";
                                    _db.AnswerConditions.Update(answerCondition);
                                    await _db.SaveChangesAsync();
                                }

                            }

                            if (key.StartsWith("SelectedCheckboxButtonAnswerCondition_"))
                            {
                                int conditionQuestionId = int.Parse(key.Replace("SelectedCheckboxButtonAnswerCondition_", ""));
                                string[] values = form[key].ToArray();
                                string value = string.Join(",", values);
                                if (answerCondition.QuestionConditionId == conditionQuestionId)
                                {
                                    answerCondition.ConditionAnswer = value;
                                    _db.AnswerConditions.Update(answerCondition);
                                    await _db.SaveChangesAsync();
                                }
                                else
                                {
                                    answerCondition.ConditionAnswer = "";
                                    _db.AnswerConditions.Update(answerCondition);
                                    await _db.SaveChangesAsync();
                                }

                            }
                        }
                        else
                        {
                            answerCondition.ConditionAnswer = "";
                        }

                        _db.AnswerConditions.Update(answerCondition);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        _db.AnswerConditions.Update(answerCondition);
                        await _db.SaveChangesAsync();
                    }


                }

            }

        }
    }
}
