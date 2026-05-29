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

        // =========================================
        // GET
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LecturaSensor>>> GetLecturas()
        {
            try
            {
                return await _context.Lecturas.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // GET ID
        // =========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<LecturaSensor>> GetLectura(int id)
        {
            try
            {
                var lectura =
                    await _context.Lecturas.FindAsync(id);

                if (lectura == null)
                    return NotFound(
                        "Lectura no encontrada.");

                return lectura;
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno: {ex.Message}");
            }
        }

        // =========================================
        // POST
        // =========================================
        [HttpPost]
        public async Task<ActionResult<LecturaSensor>>
            PostLectura(LecturaSensor lectura)
        {
            try
            {
                if (lectura.Humedad < 0 ||
                    lectura.Humedad > 100)
                {
                    return BadRequest(
                        "La humedad debe estar entre 0 y 100.");
                }

                _context.Lecturas.Add(lectura);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetLectura),
                    new { id = lectura.Id },
                    lectura);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al guardar lectura: {ex.Message}");
            }
        }
    }
}