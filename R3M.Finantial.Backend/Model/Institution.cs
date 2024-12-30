namespace R3M.Finantial.Backend.Model;

public class Institution : Register
{
    public string Name { get; set; }

    public decimal InitialBalance { get; set; }
    public DateOnly InitialBalanceDate { get; set; }
    public decimal Balance { get; set; }
}
