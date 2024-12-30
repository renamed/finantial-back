using R3M.Finantial.Backend.Model;

namespace R3M.Finantial.Backend.Dtos;

public record MovimentationResponse
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }

    public CategoryResponse Category { get; set; }

    public InstitutionResponse Institution { get; set; }

    public PeriodResponse Period { get; set; }
}
