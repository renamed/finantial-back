using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3M.Finantial.Backend.Context;
using R3M.Finantial.Backend.Dtos;
using R3M.Finantial.Backend.Model;
using System.Data;

namespace R3M.Finantial.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovimentationsController : ControllerBase
{
    private readonly FinantialContext finantialContext;

    public MovimentationsController(FinantialContext finantialContext)
    {
        this.finantialContext = finantialContext;
    }

    [HttpGet("period/{periodId}/institution/{institutionId}")]
    public async Task<IActionResult> ListAsync(Guid periodId, Guid institutionId)
    {
        var movimentations = await finantialContext
            .Movimentations
            .Include(i => i.Category)
            .Include(i => i.Institution)
            .Include(i => i.Period)
            .Where(m => m.PeriodId == periodId && m.InstitutionId == institutionId)
            .OrderBy(o => o.Date)
            .ThenByDescending(o => o.Value)
            .ThenBy(o => o.Description)
            .ToListAsync();

        return Ok(movimentations.Select(s => new MovimentationResponse
        {
            Id = s.Id,
            Description = s.Description,
            Value = s.Value,
            Date = s.Date,
            Category = new CategoryResponse
            {
                Id = s.Category.Id,
                Name = s.Category.Name
            },
            Institution = new InstitutionResponse
            {
                Id = s.Institution.Id,
                Name = s.Institution.Name,
                Balance = s.Institution.Balance,
                InitialBalance = s.Institution.InitialBalance,
                InitialBalanceDate = s.Institution.InitialBalanceDate

            },
            Period = new PeriodResponse
            {
                Id = s.Period.Id,
                Description = s.Period.Description,
                InitialDate = s.Period.InitialDate,
                FinalDate = s.Period.FinalDate
            }
        }));
    }

    [HttpGet("period/{periodId}")]
    public async Task<IActionResult> ListByPeriodAsync(Guid periodId)
    {
        var movimentations = await finantialContext
            .Movimentations
            .Include(i => i.Category)
            .Include(i => i.Institution)
            .Include(i => i.Period)
            .Where(m => m.PeriodId == periodId)
            .OrderBy(o => o.Date)
            .ThenByDescending(o => o.Value)
            .ThenBy(o => o.Description)
            .ToListAsync();

        return Ok(movimentations.Select(s => new MovimentationResponse
        {
            Id = s.Id,
            Description = s.Description,
            Value = s.Value,
            Date = s.Date,
            Category = new CategoryResponse
            {
                Id = s.Category.Id,
                Name = s.Category.Name
            },
            Institution = new InstitutionResponse
            {
                Id = s.Institution.Id,
                Name = s.Institution.Name,
                Balance = s.Institution.Balance,
                InitialBalance = s.Institution.InitialBalance,
                InitialBalanceDate = s.Institution.InitialBalanceDate

            },
            Period = new PeriodResponse
            {
                Id = s.Period.Id,
                Description = s.Period.Description,
                InitialDate = s.Period.InitialDate,
                FinalDate = s.Period.FinalDate
            }
        }));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(MovimentationRequest movimentationRequest)
    {
        var movimentation = new Movimentation
        {
            Date = movimentationRequest.Date,
            Description = movimentationRequest.Description,
            Value = movimentationRequest.Value,
            CategoryId = movimentationRequest.CategoryId,
            InstitutionId = movimentationRequest.InstitutionId,
            PeriodId = movimentationRequest.PeriodId
        };

        var period = await finantialContext.Periods.FindAsync(movimentationRequest.PeriodId);
        if (period == null)
        {
            return NotFound("Period not found");
        }

        if (movimentation.Date < period.InitialDate ||
            movimentation.Date > period.FinalDate)
        {
            return BadRequest("Wrong period");
        }

        using var transaction = await finantialContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var institution = await finantialContext.Institutions.FindAsync(movimentationRequest.InstitutionId);
        if (institution == null)
        {
            return NotFound("Institution not found");
        }

        if (movimentationRequest.Date < institution.InitialBalanceDate)
        {
            return BadRequest("Movimentation date is before the institution initial balance date");
        }

        var category = await finantialContext.Categories.FindAsync(movimentationRequest.CategoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        finantialContext.Movimentations.Add(movimentation);
        institution.Balance += movimentation.Value;
        
        await finantialContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return Created(Request.Path, new MovimentationResponse
        {
            Id = movimentation.Id,
            Description = movimentation.Description,
            Value = movimentation.Value,
            Date = movimentation.Date,
            Category = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            },
            Institution = new InstitutionResponse
            {
                Id = institution.Id,
                Name = institution.Name,
                Balance = institution.Balance,
                InitialBalance = institution.InitialBalance,
                InitialBalanceDate = institution.InitialBalanceDate

            },
            Period = new PeriodResponse
            {
                Id = period.Id,
                Description = period.Description,
                InitialDate = period.InitialDate,
                FinalDate = period.FinalDate
            }
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync([FromBody] MovimentationRequest movimentationRequest, Guid id)
    {
        var movimentation = await finantialContext.Movimentations.FindAsync(id);

        if (movimentation == null)
        {
            return NotFound("Movimentation not found");
        }

        var period = await finantialContext.Periods.FindAsync(movimentationRequest.PeriodId);
        if (period == null)
        {
            return NotFound("Period not found");
        }

        if (movimentation.Date < period.InitialDate || 
            movimentation.Date > period.FinalDate)
        {
            return BadRequest("Wrong period");
        }

        using var transaction = await finantialContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var institution = await finantialContext.Institutions.FindAsync(movimentationRequest.InstitutionId);
        if (institution == null)
        {
            return NotFound("Institution not found");
        }

        var category = await finantialContext.Categories.FindAsync(movimentationRequest.CategoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        var oldBalance = movimentation.Value;
        movimentation.Date = movimentationRequest.Date;
        movimentation.Description = movimentationRequest.Description;
        movimentation.Value = movimentationRequest.Value;
        movimentation.CategoryId = movimentationRequest.CategoryId;
        movimentation.InstitutionId = movimentationRequest.InstitutionId;
        movimentation.PeriodId = movimentationRequest.PeriodId;

        institution.Balance -= oldBalance;
        institution.Balance += movimentation.Value;

        await finantialContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(new MovimentationResponse
        {
            Id = movimentation.Id,
            Description = movimentation.Description,
            Value = movimentation.Value,
            Date = movimentation.Date,
            Category = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            },
            Institution = new InstitutionResponse
            {
                Id = institution.Id,
                Name = institution.Name,
                Balance = institution.Balance,
                InitialBalance = institution.InitialBalance,
                InitialBalanceDate = institution.InitialBalanceDate

            },
            Period = new PeriodResponse
            {
                Id = period.Id,
                Description = period.Description,
                InitialDate = period.InitialDate,
                FinalDate = period.FinalDate
            }
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var movimentation = await finantialContext.Movimentations.FindAsync(id);
        if (movimentation == null)
        {
            return NotFound("Movimentation not found");
        }

        using var transaction = await finantialContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var institution = await finantialContext.Institutions.FindAsync(movimentation.InstitutionId);
        if (institution == null)
        {
            return NotFound("Institution not found");
        }
             
        institution.Balance -= movimentation.Value;
        finantialContext.Movimentations.Remove(movimentation);

        await finantialContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return NoContent();
    }
}
