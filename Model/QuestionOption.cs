using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FormWizard.Model
{
    public class QuestionOption
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Option Text")]
        public string OptionText { get; set; }
        [Required]
        [Display(Name = "Option Type")]
        public string OptionType { get; set; }

        [Display(Name = "Set a value for this option")]
        public string OptionValue { get; set; } = string.Empty;
        [Display(Name = "Order of Display as option.")]
        public int? OrderOfDisplay { get; set; }
        [ValidateNever]
        public Question Question { get; set; }
        [Required]
        public int QuestionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isChecked { get; set; } = false;

    }
}
