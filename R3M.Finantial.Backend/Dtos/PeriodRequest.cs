using R3M.Finantial.Backend.Model;
using System.ComponentModel.DataAnnotations;

namespace R3M.Finantial.Backend.Dtos;

public record PeriodRequest
{
    public DateOnly InitialDate { get; set; }
    public DateOnly FinalDate { get; set; }

    [Required]
    [StringLength(Constants.PeriodDescriptionLength)]
    public string Description { get; set; }
}
