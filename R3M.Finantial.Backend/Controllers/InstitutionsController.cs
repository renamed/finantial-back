using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3M.Finantial.Backend.Context;
using R3M.Finantial.Backend.Dtos;
using R3M.Finantial.Backend.Model;
using System.Data;

namespace R3M.Finantial.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstitutionsController : ControllerBase
{
    private readonly FinantialContext finantialContext;

    public InstitutionsController(FinantialContext finantialContext)
    {
        this.finantialContext = finantialContext;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var institutions = await finantialContext.Institutions.ToListAsync();

        return Ok(institutions.Select(s => new InstitutionResponse
        {
            Id = s.Id,
            Balance = s.Balance,
            InitialBalance = s.InitialBalance,
            InitialBalanceDate = s.InitialBalanceDate,
            Name = s.Name
        }));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(InstitutionRequest institutionRequest)
    {
        var institution = new Institution
        {
            Name = institutionRequest.Name,
            InitialBalance = institutionRequest.InitialBalance,
            InitialBalanceDate = institutionRequest.InitialBalanceDate,
            Balance = institutionRequest.InitialBalance
        };

        finantialContext.Institutions.Add(institution);
        await finantialContext.SaveChangesAsync();

        return Created(Request.Path, new InstitutionResponse
        {
            Id = institution.Id,
            Balance = institution.Balance,
            InitialBalance = institution.InitialBalance,
            InitialBalanceDate = institution.InitialBalanceDate,
            Name = institution.Name
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromBody] InstitutionRequest institutionRequest, Guid id)
    {
        using var transaction = await finantialContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var institution = await finantialContext.Institutions.FindAsync(id);

        if (institution == null)
        {
            return NotFound("Institution not found");
        }

        if (institution.InitialBalance != institutionRequest.InitialBalance)
        {
            institution.Balance = institutionRequest.InitialBalance + 
                await finantialContext.Movimentations
                    .Where(m => m.InstitutionId == id)
                    .SumAsync(m => m.Value);
        }

        institution.Name = institutionRequest.Name;
        institution.InitialBalance = institutionRequest.InitialBalance;
        institution.InitialBalanceDate = institutionRequest.InitialBalanceDate;


        await finantialContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(new InstitutionResponse
        {
            Id = institution.Id,
            Balance = institution.Balance,
            InitialBalance = institution.InitialBalance,
            InitialBalanceDate = institution.InitialBalanceDate,
            Name = institution.Name
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var institution = await finantialContext.Institutions.FindAsync(id);

        if (institution == null)
        {
            return NotFound("Institution not found");
        }

        var existsMovimentations = await finantialContext.Movimentations
            .AnyAsync(m => m.InstitutionId == id);
        if (existsMovimentations)
        {
            return BadRequest("Institution has movimentations");
        }

        finantialContext.Institutions.Remove(institution);
        await finantialContext.SaveChangesAsync();
        return NoContent();
    }
}
