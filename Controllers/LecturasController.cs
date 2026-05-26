using AgroTechAPI.Data;
using AgroTechAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturasController : ControllerBase
    {
        private readonly AgroTechContext _context;

        public LecturasController(AgroTechContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LecturaSensor>>> GetLecturas()
        {
            return await _context.Lecturas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LecturaSensor>> GetLectura(int id)
        {
            var lectura = await _context.Lecturas.FindAsync(id);
            if (lectura == null) return NotFound();
            return lectura;
        }

        [HttpPost]
        public async Task<ActionResult<LecturaSensor>> PostLectura(LecturaSensor lectura)
        {
            _context.Lecturas.Add(lectura);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLectura), new { id = lectura.Id }, lectura);
        }
    }
}