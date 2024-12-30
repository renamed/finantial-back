namespace R3M.Finantial.Backend.Dtos;

public record InstitutionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public decimal InitialBalance { get; set; }
    public DateOnly InitialBalanceDate { get; set; }
    public decimal Balance { get; set; }
}
