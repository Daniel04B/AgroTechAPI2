using AgroTechAPI.Models;
using Microsoft.EntityFrameworkCore;
using AgroTech.Models;

namespace AgroTechAPI.Data
{
    public class AgroTechContext : DbContext
    {
        public AgroTechContext(
            DbContextOptions<AgroTechContext> options)
            : base(options)
        {
        }
        public DbSet<Cultivo> Cultivos { get; set; }

        public DbSet<Agricultor> Agricultor { get; set; }

        public DbSet<Zona> Zonas { get; set; }

        public DbSet<LecturaSensor> Lecturas { get; set; }

        public DbSet<Sensor> Sensores { get; set; }
    }
}