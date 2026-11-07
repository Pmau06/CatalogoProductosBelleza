
using CatalogoProductosBelleza.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar los servicios de MVC
builder.Services.AddControllersWithViews();

// Configurar Entity Framework Core con SQL Server
builder.Services.AddDbContext<CatalogoDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSQL")
    )
);

var app = builder.Build();

// Configurar el manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redireccionar a HTTPS
app.UseHttpsRedirection();

// Permitir archivos CSS, JavaScript e imágenes
app.UseStaticFiles();

// Configurar las rutas
app.UseRouting();

app.UseAuthorization();

// Definir la ruta principal de MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
