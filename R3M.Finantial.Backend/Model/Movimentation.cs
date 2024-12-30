namespace R3M.Finantial.Backend.Model;

public class Movimentation : Register
{
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; }

    public Guid PeriodId { get; set; }
    public Period Period { get; set; }
}
