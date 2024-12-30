namespace R3M.Finantial.Backend.Dtos;

public record CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public record CategoryWithChildrenResponse : CategoryResponse
{
    public List<CategoryWithChildrenResponse> Children { get; set; }
}