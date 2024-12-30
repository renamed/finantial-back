using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using R3M.Finantial.Backend.Context;
using R3M.Finantial.Backend.Dtos;
using R3M.Finantial.Backend.Model;
using System.Collections.Generic;

namespace R3M.Finantial.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly FinantialContext finantialContext;

    public CategoriesController(FinantialContext finantialContext)
    {
        this.finantialContext = finantialContext;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var categories = await finantialContext.Categories
            .Include(i => i.Children)
            .ToListAsync();

        return Ok(Convert(categories.Where(x => !x.ParentId.HasValue)));
    }

    private static List<CategoryWithChildrenResponse> Convert(IEnumerable<Category> categories)
    {
        List<CategoryWithChildrenResponse> res = [];
        foreach (var cat in categories)
        {
            res.Add(new CategoryWithChildrenResponse
            {
                Id = cat.Id,
                Name = cat.Name,
                Children = Convert(cat.Children.Where(x => x.ParentId.HasValue && x.ParentId.Value == cat.Id))
            });
        }

        return res;
    }

    [HttpGet("children")]
    public async Task<IActionResult> ListChildrenAsync([FromQuery]Guid? parentId = null)
    {
        var categories = await finantialContext.Categories
            .Where(c => c.ParentId == parentId).ToListAsync();

        return Ok(categories.Select(s => new CategoryResponse
        {
            Id = s.Id,
            Name = s.Name
        }));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(CategoryRequest categoryRequest)
    {
        var category = new Category
        {
            Name = categoryRequest.Name,
            ParentId = categoryRequest.ParentId.HasValue && categoryRequest.ParentId.Value != default ? categoryRequest.ParentId : null
        };

        finantialContext.Categories.Add(category);
        await finantialContext.SaveChangesAsync();

        return Created(Request.Path, new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync([FromBody] CategoryRequest categoryRequest, Guid id)
    {
        var category = await finantialContext.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        category.Name = categoryRequest.Name;

        await finantialContext.SaveChangesAsync();

        return Ok(new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var category = await finantialContext.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        var existsMovimentation = await finantialContext.Movimentations
            .AnyAsync(m => m.CategoryId == id);
        if (existsMovimentation)
        {
            return BadRequest("Period has movimentations");
        }

        var existsChildren = await finantialContext.Categories
            .AnyAsync(c => c.ParentId == id);
        if (existsChildren)
        {
            return BadRequest("Period has children");
        }

        finantialContext.Categories.Remove(category);
        await finantialContext.SaveChangesAsync();
        return NoContent();
    }
}
