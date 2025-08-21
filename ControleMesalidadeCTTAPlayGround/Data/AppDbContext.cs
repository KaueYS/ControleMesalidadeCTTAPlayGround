using ControleMesalidadeCTTAPlayGround.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleMesalidadeCTTAPlayGround.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AssociadoModel> Associados { get; set; }
        public DbSet<PagamentoModel> Pagamentos { get; set; } 
    }
}
