using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R3M.Finantial.Backend.Model;

public abstract class Register
{
    public Guid Id { get; set; }

    public DateTime InsertedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
