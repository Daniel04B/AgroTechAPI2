using AgroTechAPI.Data;
using AgroTechAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        [HttpGet("usuario/{agricultorId}")]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonasPorUsuario(int agricultorId)
        {
            return await _context.Zonas
                .Where(z => z.AgricultorId == agricultorId)
                .ToListAsync();
        }

        // =========================================
        // GET ALL
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
        {
            try
            {
                return await _context.Zonas.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno del servidor: {ex.Message}");
            }
        }

        // =========================================
        // GET BY ID
        // =========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Zona>> GetZona(int id)
        {
            try
            {
                var zona = await _context.Zonas.FindAsync(id);

                if (zona == null)
                {
                    return NotFound(
                        "La zona solicitada no existe.");
                }

                return zona;
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("agricultor/{agricultorId}")]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonasPorAgricultor(int agricultorId)
        {
            var zonas = await _context.Zonas
                .Where(z => z.AgricultorId == agricultorId)
                .ToListAsync();

            return zonas;
        }

        // =========================================
        // CREATE
        // =========================================
        [HttpPost]
        public async Task<ActionResult<Zona>> PostZona(Zona zona)
        {
            try
            {
                // VALIDACIONES
                if (zona == null)
                {
                    return BadRequest(
                        "Los datos de la zona son inválidos.");
                }

                if (string.IsNullOrWhiteSpace(zona.Nombre))
                {
                    return BadRequest(
                        "El nombre de la zona es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(zona.Estado))
                {
                    return BadRequest(
                        "El estado de la zona es obligatorio.");
                }

                // VALIDAR LONGITUD
                if (zona.Nombre.Length > 100)
                {
                    return BadRequest(
                        "El nombre no puede superar los 100 caracteres.");
                }

                _context.Zonas.Add(zona);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetZona),
                    new { id = zona.Id },
                    zona);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al registrar zona: {ex.Message}");
            }
        }

        // =========================================
        // UPDATE
        // =========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZona(
            int id,
            Zona zona)
        {
            try
            {
                if (id != zona.Id)
                {
                    return BadRequest(
                        "El ID no coincide.");
                }

                var zonaExistente =
                    await _context.Zonas.FindAsync(id);

                if (zonaExistente == null)
                {
                    return NotFound(
                        "Zona no encontrada.");
                }

                // VALIDACIONES
                if (string.IsNullOrWhiteSpace(zona.Nombre))
                {
                    return BadRequest(
                        "El nombre es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(zona.Estado))
                {
                    return BadRequest(
                        "El estado es obligatorio.");
                }

                // ACTUALIZAR
                zonaExistente.Nombre = zona.Nombre;
                zonaExistente.SensorId = zona.SensorId;
                zonaExistente.Estado = zona.Estado;
                zonaExistente.Humedad = zona.Humedad;
                zonaExistente.AgricultorId = zona.AgricultorId;

                await _context.SaveChangesAsync();

                return Ok(
                    "Zona actualizada correctamente.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500,
                    "Conflicto de concurrencia detectado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al actualizar zona: {ex.Message}");
            }
        }

        // =========================================
        // DELETE
        // =========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZona(int id)
        {
            try
            {
                var zona =
                    await _context.Zonas.FindAsync(id);

                if (zona == null)
                {
                    return NotFound(
                        "La zona no existe.");
                }

                _context.Zonas.Remove(zona);

                await _context.SaveChangesAsync();

                return Ok(
                    "Zona eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al eliminar zona: {ex.Message}");
            }
           
        }
    }
}