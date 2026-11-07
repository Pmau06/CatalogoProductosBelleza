
using CatalogoProductosBelleza.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProductosBelleza.Data
{
    // Clase que permite trabajar con la base de datos
    public class CatalogoDbContext : DbContext
    {
        // Constructor que recibe la configuración
        // de conexión a SQL Server
        public CatalogoDbContext(
            DbContextOptions<CatalogoDbContext> options)
            : base(options)
        {
        }

        // Representa la tabla Productos de SQL Server
        public DbSet<Producto> Productos { get; set; } = null!;
           
    }
}
