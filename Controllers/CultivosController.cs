using AgroTechAPI.Data;
using AgroTech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AgroTechAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CultivosController : ControllerBase
    {

        private readonly AgroTechContext _context;


        public CultivosController(
            AgroTechContext context)
        {
            _context = context;
        }



        // GET TODOS
        [HttpGet]
        public async Task<ActionResult<List<Cultivo>>> GetCultivos()
        {
            return await _context.Cultivos
                .ToListAsync();
        }



        // GET POR AGRICULTOR
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<List<Cultivo>>>
            GetCultivosUsuario(int id)
        {

            return await _context.Cultivos
                .Where(c => c.AgricultorId == id)
                .ToListAsync();

        }



        // CREAR
        [HttpPost]
        public async Task<ActionResult<Cultivo>>
            CrearCultivo(Cultivo cultivo)
        {

            _context.Cultivos.Add(cultivo);

            await _context.SaveChangesAsync();


            return Ok(cultivo);

        }



        // EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult>
            ActualizarCultivo(
                int id,
                Cultivo cultivo)
        {

            if (id != cultivo.Id)
                return BadRequest();


            _context.Entry(cultivo)
                .State = EntityState.Modified;


            await _context.SaveChangesAsync();


            return NoContent();

        }



        // ELIMINAR
        [HttpDelete("{id}")]
        public async Task<IActionResult>
            EliminarCultivo(int id)
        {

            var cultivo =
                await _context.Cultivos
                .FindAsync(id);


            if (cultivo == null)
                return NotFound();


            _context.Cultivos.Remove(cultivo);


            await _context.SaveChangesAsync();


            return NoContent();

        }

    }
}