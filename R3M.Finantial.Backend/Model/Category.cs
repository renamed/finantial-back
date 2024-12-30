namespace R3M.Finantial.Backend.Model;

public class Category : Register
{
    public string Name { get; set; }

    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    public List<Category> Children { get; set; }
}
