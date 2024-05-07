using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormWizard.Model
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string QuestionText { get; set; }
        public string? QuestionDescription { get; set; }
        public enum QuestionType
        {
            [Display(Name ="Plain Text Answer")]
            text,
            [Display(Name = "One Answer from Different Options")]
            radio,
            [Display(Name = "Multiple Answers from Different Options")]
            checkbox,
            [Display(Name = "Number as Answer")]
            number,
            [Display(Name = "Date as Answer")]
            date,
            [Display(Name = "One Answer from List Item")]
            list
        }
        [Required]
        public QuestionType Type { get; set; }
        [Required]
        public bool IsRequired { get; set; } = false;
        [Required]
        public bool IsInUse { get; set; } = true;
        public int? OrderOfDisplay { get; set; }
        [Required]
        [Display(Name = "This Question has Extended questions")]
        public bool Extension { get; set; } = false;
        public string? Value { get; set; }
        [ValidateNever]
        public MyForm MyForm { get; set; }
        [Required]
        public int MyFormId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
