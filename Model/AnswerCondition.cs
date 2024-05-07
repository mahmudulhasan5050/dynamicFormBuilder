using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FormWizard.Model
{
    public class AnswerCondition
    {
        [Key]
        public int Id { get; set; }
        public string ConditionAnswer { get; set; }
        [Required]
        public int MyFormId { get; set; }
        [Required]
        public int QuestionConditionId { get; set; }
        [ValidateNever]
        public QuestionCondition QuestionCondition { get; set; }
        [Required]
        public int ParticipantId { get; set; }
        [ValidateNever]
        public Participant Participant { get; set; }
        public string? Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
