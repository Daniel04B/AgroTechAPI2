using AgroTechAPI.Data;
using AgroTechAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonasController : ControllerBase
    {
        private readonly AgroTechContext _context;

        public ZonasController(AgroTechContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
        {
            return await _context.Zonas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Zona>> GetZona(int id)
        {
            var zona = await _context.Zonas.FindAsync(id);
            if (zona == null) return NotFound();
            return zona;
        }

        [HttpPost]
        public async Task<ActionResult<Zona>> PostZona(Zona zona)
        {
            _context.Zonas.Add(zona);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetZona), new { id = zona.Id }, zona);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutZona(int id, Zona zona)
        {
            if (id != zona.Id) return BadRequest();

            _context.Entry(zona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Zonas.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZona(int id)
        {
            var zona = await _context.Zonas.FindAsync(id);
            if (zona == null) return NotFound();

            _context.Zonas.Remove(zona);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}