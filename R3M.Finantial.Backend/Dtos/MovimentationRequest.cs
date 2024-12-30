using R3M.Finantial.Backend.Model;

namespace R3M.Finantial.Backend.Dtos;

public record MovimentationRequest
{
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }

    public Guid CategoryId { get; set; }
    
    public Guid InstitutionId { get; set; }
    
    public Guid PeriodId { get; set; }
}
