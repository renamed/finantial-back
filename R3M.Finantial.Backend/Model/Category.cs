using Microsoft.EntityFrameworkCore;

namespace R3M.Finantial.Backend.Model;

public class Category : Register
{
    public string Name { get; set; }

    public Guid? ParentId { get; set; }
    public HierarchyId HierarchyId { get; set; }

    public int ChildrenCount { get; set; }
}
