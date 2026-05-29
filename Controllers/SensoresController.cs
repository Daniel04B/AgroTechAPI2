using AgroTechAPI.Data;
using AgroTechAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroTechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensoresController : ControllerBase
    {
        private readonly AgroTechContext _context;

        public SensoresController(AgroTechContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensores()
        {
            return await _context.Sensores.ToListAsync();
        }

        [HttpGet("usuario/{agricultorId}")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensoresPorUsuario(int agricultorId)
        {
            return await _context.Sensores
                .Where(x => x.AgricultorId == agricultorId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PostSensor(Sensor sensor)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sensor.Nombre))
                    return BadRequest("Nombre obligatorio.");

                if (string.IsNullOrWhiteSpace(sensor.Tipo))
                    return BadRequest("Tipo obligatorio.");

                if (string.IsNullOrWhiteSpace(sensor.Ubicacion))
                    return BadRequest("Ubicación obligatoria.");

                _context.Sensores.Add(sensor);

                await _context.SaveChangesAsync();

                return Ok(sensor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensor(int id, Sensor sensor)
        {
            try
            {
                if (id != sensor.Id)
                    return BadRequest("ID inválido.");

                var existente =
                    await _context.Sensores.FindAsync(id);

                if (existente == null)
                    return NotFound("Sensor no encontrado.");

                existente.Nombre = sensor.Nombre;
                existente.Tipo = sensor.Tipo;
                existente.Ubicacion = sensor.Ubicacion;
                existente.ValorCalibracion = sensor.ValorCalibracion;
                existente.AgricultorId = sensor.AgricultorId;

                await _context.SaveChangesAsync();

                return Ok("Sensor actualizado.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensor = await _context.Sensores.FindAsync(id);

            if (sensor == null)
                return NotFound();

            _context.Sensores.Remove(sensor);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}