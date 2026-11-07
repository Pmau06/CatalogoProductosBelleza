
using CatalogoProductosBelleza.Data;
using CatalogoProductosBelleza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoProductosBelleza.Controllers
{
    // Controlador encargado de gestionar los productos
    public class ProductosController : Controller
    {
        // Variable para acceder a la base de datos
        private readonly CatalogoDbContext _context;

        // Constructor que recibe el contexto
        public ProductosController(CatalogoDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // MOSTRAR Y BUSCAR PRODUCTOS
        // ==========================================

        // GET: Productos
        // Muestra todos los productos o los resultados
        // de una búsqueda por nombre, categoría o marca.

        [HttpGet]
        public async Task<IActionResult> Index(string? buscar)
        {
            // 1. Consultar los productos de SQL Server
            var productos = _context.Productos
                .AsNoTracking()
                .AsQueryable();

            // 2. Calcular el resumen general del inventario
            // Estos valores incluyen todos los productos registrados

            ViewBag.TotalProductos =
                await productos.CountAsync();

            ViewBag.TotalUnidades =
                await productos.SumAsync(p => (int?)p.Cantidad) ?? 0;

            ViewBag.ValorInventario =
                await productos.SumAsync(
                    p => (decimal?)(p.Precio * p.Cantidad)
                ) ?? 0m;

            // 3. Aplicar el filtro del buscador
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                productos = productos.Where(p =>
                    p.Nombre.Contains(buscar) ||
                    p.Categoria.Contains(buscar) ||
                    p.Marca.Contains(buscar));
            }

            // 4. Conservar el texto buscado
            ViewBag.Buscar = buscar;

            // 5. Obtener los productos para la tabla
            List<Producto> listaProductos =
                await productos
                    .OrderBy(p => p.Id)
                    .ToListAsync();

            // 6. Enviar los productos a la vista
            return View(listaProductos);
        }


        // ==========================================
        // REGISTRAR PRODUCTOS
        // ==========================================

        // GET: Productos/Create
        // Muestra el formulario para registrar un producto
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // Guarda el producto en SQL Server
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Nombre,Categoria,Marca,Precio,Cantidad,Descripcion")]
            Producto producto)
        {
            // Comprobar que los datos sean válidos
            if (ModelState.IsValid)
            {
                // Agregar el producto
                _context.Productos.Add(producto);

                // Guardar los cambios en SQL Server
                await _context.SaveChangesAsync();

                // Regresar al catálogo
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, regresar al formulario
            return View(producto);
        }

        // ==========================================
        // EDITAR PRODUCTOS
        // ==========================================

        // GET: Productos/Edit/1
        // Busca el producto que queremos editar
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // Comprobar que recibimos un ID
            if (id == null)
            {
                return NotFound();
            }

            // Buscar el producto en SQL Server
            var producto = await _context.Productos
                .FindAsync(id.Value);

            // Comprobar que el producto existe
            if (producto == null)
            {
                return NotFound();
            }

            // Mostrar el formulario con sus datos
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Guarda las modificaciones en SQL Server
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nombre,Categoria,Marca,Precio,Cantidad,Descripcion")]
            Producto producto)
        {
            // Verificar que el ID sea correcto
            if (id != producto.Id)
            {
                return NotFound();
            }

            // Comprobar los datos del formulario
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            // Buscar el producto original
            var productoOriginal =
                await _context.Productos.FindAsync(id);

            if (productoOriginal == null)
            {
                return NotFound();
            }

            // Actualizar sus propiedades
            productoOriginal.Nombre = producto.Nombre;
            productoOriginal.Categoria = producto.Categoria;
            productoOriginal.Marca = producto.Marca;
            productoOriginal.Precio = producto.Precio;
            productoOriginal.Cantidad = producto.Cantidad;
            productoOriginal.Descripcion = producto.Descripcion;

            // Guardar cambios en SQL Server
            await _context.SaveChangesAsync();

            // Regresar al catálogo
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // ELIMINAR PRODUCTOS
        // ==========================================

        // GET: Productos/Delete/2
        // Busca el producto que queremos eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Buscar el producto en SQL Server
            var producto = await _context.Productos
                .FindAsync(id);

            // Verificar que exista
            if (producto == null)
            {
                return NotFound();
            }

            // Mostrar los datos antes de eliminar
            return View(producto);
        }

        // POST: Productos/Delete/2
        // Elimina el producto después de confirmar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Buscar el producto en SQL Server
            var producto = await _context.Productos
                .FindAsync(id);

            // Verificar que exista
            if (producto == null)
            {
                return NotFound();
            }

            // Eliminar el producto
            _context.Productos.Remove(producto);

            // Guardar los cambios en SQL Server
            await _context.SaveChangesAsync();

            // Regresar al catálogo
            return RedirectToAction(nameof(Index));
        }
    }
}
