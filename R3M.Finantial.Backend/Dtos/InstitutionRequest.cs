using R3M.Finantial.Backend.Model;
using System.ComponentModel.DataAnnotations;

namespace R3M.Finantial.Backend.Dtos;

public record InstitutionRequest
{
    [Required]
    [StringLength(
        Constants.InstitutionNameMaxLength, 
        MinimumLength = Constants.InstitutionNameMinLength)]
    public string Name { get; set; }

    public decimal InitialBalance { get; set; }
    public DateOnly InitialBalanceDate { get; set; }
}