using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FormWizard.Model
{
    public class MyForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsInUse { get; set; } = true;
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        [Display(Name="Field")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Country Country { get; set; }
        
        [Required]
        [Display(Name = "Country Name")]

        public int CountryId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
