using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3M.Finantial.Backend.Context;
using R3M.Finantial.Backend.Dtos;
using R3M.Finantial.Backend.Dtos.Dashboard;
using R3M.Finantial.Backend.Model;
using System.Linq;
using System.Linq.Expressions;

namespace R3M.Finantial.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly FinantialContext finantialContext;

    public DashboardController(FinantialContext finantialContext)
    {
        this.finantialContext = finantialContext;
    }

    [HttpGet("categories/{periodId}")]
    public async Task<IActionResult> GroupByCategoryAsync(Guid periodId, [FromQuery] Guid? parentId = null)
    {
        var statistics = await finantialContext.Movimentations
            .Include(i => i.Category)
            .ThenInclude(i => i.Children)
            .Where(x => x.PeriodId == periodId)
            .GroupBy(x => x.Category.Name)
            .Select(x => new GroupByCategoryResponse
            {
                Category = x.First().Category.Name,
                Sum = x.Sum(s => s.Value),
                Qty = x.Count(),
                Avg = x.Average(s => s.Value)
            })
            .ToListAsync();

        return Ok(statistics);
    }

}