using R3M.Finantial.Backend.Model;
using System.ComponentModel.DataAnnotations;

namespace R3M.Finantial.Backend.Dtos;

public record CategoryRequest
{
    [Required]
    [StringLength(
        Constants.CategoryNameMaxLength, 
        MinimumLength = Constants.CategoryNameMinLength)]
    public string Name { get; set; }

    public Guid? ParentId { get; set; }
}
