namespace R3M.Finantial.Backend.Dtos.Dashboard;

public record GroupByCategoryResponse
{
    public Guid CategoryId { get; set; }
    public string Category { get; set; }
    public decimal Sum { get; set; }
    public decimal Qty { get; set; }
    public decimal Avg { get; set; }
}
