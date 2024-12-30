using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3M.Finantial.Backend.Context;
using R3M.Finantial.Backend.Dtos;
using R3M.Finantial.Backend.Model;

namespace R3M.Finantial.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PeriodsController : ControllerBase
{
    private readonly FinantialContext finantialContext;

    public PeriodsController(FinantialContext finantialContext)
    {
        this.finantialContext = finantialContext;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var periods = await finantialContext
            .Periods
            .OrderByDescending(o => o.FinalDate)
            .ToListAsync();
        return Ok(periods.Select(s => new PeriodResponse
        {
            Id = s.Id,
            Description = s.Description,
            InitialDate = s.InitialDate,
            FinalDate = s.FinalDate
        }));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(PeriodRequest periodRequest)
    {
        var period = new Period
        {
            Description = periodRequest.Description,
            InitialDate = periodRequest.InitialDate,
            FinalDate = periodRequest.FinalDate
        };

        var hasOverlapping = await
            finantialContext.Periods.AnyAsync(p =>
                p.InitialDate < periodRequest.FinalDate && p.FinalDate > periodRequest.InitialDate);

        if (hasOverlapping)
        {
            return BadRequest("Overlapping period");
        }

        finantialContext.Periods.Add(period);
        await finantialContext.SaveChangesAsync();

        return Created(Request.Path, new PeriodResponse
        {
            Id = period.Id,
            Description = period.Description,
            InitialDate = period.InitialDate,
            FinalDate = period.FinalDate
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync([FromBody] PeriodRequest periodRequest, Guid id)
    {
        var period = await finantialContext.Periods.FindAsync(id);

        if (period == null)
        {
            return NotFound("Period not found");
        }

        period.Description = periodRequest.Description;
        period.InitialDate = periodRequest.InitialDate;
        period.FinalDate = periodRequest.FinalDate;

        var hasOverlapping = await
            finantialContext.Periods.AnyAsync(p =>
                           p.Id != id && p.InitialDate < periodRequest.FinalDate && p.FinalDate > periodRequest.InitialDate);
        if (hasOverlapping)
        {
            return BadRequest("Overlapping period");
        }

        var hasMovimentationsOutsidePeriod
            = await finantialContext.Movimentations.AnyAsync(m => m.PeriodId == id &&
                           (m.Date < periodRequest.InitialDate || m.Date > periodRequest.FinalDate));
        if (hasMovimentationsOutsidePeriod)
        {
            return BadRequest("Movimentations outside period");
        }

        await finantialContext.SaveChangesAsync();

        return Ok(new PeriodResponse
        {
            Id = period.Id,
            Description = period.Description,
            InitialDate = period.InitialDate,
            FinalDate = period.FinalDate
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var period = await finantialContext.Periods.FindAsync(id);

        if (period == null)
        {
            return NotFound("Period not found");
        }

        var hasMovimentations = await finantialContext.Movimentations
            .AnyAsync(m => m.PeriodId == id);
        if (hasMovimentations)
        {
            return BadRequest("Period has movimentations");
        }

        finantialContext.Periods.Remove(period);
        await finantialContext.SaveChangesAsync();

        return NoContent();
    }
}
