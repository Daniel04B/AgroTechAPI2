using AgroTechAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgroTechAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensoresController : ControllerBase
    {
        private static readonly List<Sensor> _dbSensores = new()
        {
            new Sensor
            {
                Id = 1,
                Nombre = "SE-HUM-01",
                Tipo = "Humedad",
                Ubicacion = "Lote Norte",
                ValorCalibracion = 45.2
            },

            new Sensor
            {
                Id = 2,
                Nombre = "SE-TMP-02",
                Tipo = "Temperatura",
                Ubicacion = "Invernadero",
                ValorCalibracion = 24.8
            }
        };

        // =========================================
        // GET
        // =========================================
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_dbSensores);
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
        public IActionResult Post([FromBody] Sensor nuevoSensor)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(
                    nuevoSensor.Nombre))
                {
                    return BadRequest(
                        "El nombre es obligatorio.");
                }

                nuevoSensor.Id =
                    _dbSensores.Any()
                    ? _dbSensores.Max(s => s.Id) + 1
                    : 1;

                _dbSensores.Add(nuevoSensor);

                return Ok(nuevoSensor);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al registrar sensor: {ex.Message}");
            }
        }

        // =========================================
        // PUT
        // =========================================
        [HttpPut("{id}")]
        public IActionResult Put(
            int id,
            [FromBody] Sensor sensorEditado)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existente =
                    _dbSensores.FirstOrDefault(s => s.Id == id);

                if (existente == null)
                    return NotFound(
                        "Sensor no encontrado.");

                existente.Nombre =
                    sensorEditado.Nombre;

                existente.Tipo =
                    sensorEditado.Tipo;

                existente.Ubicacion =
                    sensorEditado.Ubicacion;

                existente.ValorCalibracion =
                    sensorEditado.ValorCalibracion;

                return Ok(existente);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al actualizar: {ex.Message}");
            }
        }

        // =========================================
        // DELETE
        // =========================================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existente =
                    _dbSensores.FirstOrDefault(s => s.Id == id);

                if (existente == null)
                    return NotFound(
                        "Sensor no encontrado.");

                _dbSensores.Remove(existente);

                return Ok(
                    "Sensor eliminado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Error al eliminar: {ex.Message}");
            }
        }
    }
}