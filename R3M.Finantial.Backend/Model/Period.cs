using System.ComponentModel.DataAnnotations;

namespace R3M.Finantial.Backend.Model;

public class Period : Register
{
    public DateOnly InitialDate { get; set; }
    public DateOnly FinalDate { get; set; }

    public string Description { get; set; }
}
