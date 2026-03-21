using ExplorerApp.Infrastructure.Data;
using ExplorerApp.Infrastructure.Models;
using ExplorerApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExplorerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CountriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll()
        {
            var countries = await _context.Countries
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Capital = c.Capital,
                    Population = c.Population,
                    Region = c.Region,
                    FlagUrl = c.FlagUrl
                })
                .ToListAsync();

            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetById(int id)
        {
            var country = await _context.Countries
                .Where(c => c.Id == id)
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Capital = c.Capital,
                    Population = c.Population,
                    Region = c.Region,
                    FlagUrl = c.FlagUrl
                })
                .FirstOrDefaultAsync();

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPost]
        public async Task<ActionResult<CountryDto>> Create(CreateCountryDto dto)
        {
            var country = new Country
            {
                Name = dto.Name,
                Capital = dto.Capital,
                Population = dto.Population,
                Region = dto.Region,
                FlagUrl = dto.FlagUrl
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            var result = new CountryDto
            {
                Id = country.Id,
                Name = country.Name,
                Capital = country.Capital,
                Population = country.Population,
                Region = country.Region,
                FlagUrl = country.FlagUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = country.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCountryDto dto)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            country.Name = dto.Name;
            country.Capital = dto.Capital;
            country.Population = dto.Population;
            country.Region = dto.Region;
            country.FlagUrl = dto.FlagUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}