using R3M.Finantial.Backend.Model;
using System.ComponentModel.DataAnnotations;

namespace R3M.Finantial.Backend.Dtos;

public class PeriodResponse
{
    public Guid Id { get; set; }
    public DateOnly InitialDate { get; set; }
    public DateOnly FinalDate { get; set; }
    public string Description { get; set; }
}
