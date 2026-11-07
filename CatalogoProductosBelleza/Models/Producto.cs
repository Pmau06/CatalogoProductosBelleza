using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoProductosBelleza.Models
{
    // Representa un producto de belleza
    [Table("Productos")]
    public class Producto
    {
        // Clave primaria
        [Key]
        public int Id { get; set; }

        // Nombre del producto
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // Categoría del producto
        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(50)]
        public string Categoria { get; set; } = string.Empty;

        // Marca del producto
        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(80)]
        public string Marca { get; set; } = string.Empty;

        // Precio en colones
        [Range(0, 99999999.99,
            ErrorMessage = "El precio no puede ser negativo")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        // Cantidad disponible
        [Range(0, int.MaxValue,
            ErrorMessage = "La cantidad no puede ser negativa")]
        public int Cantidad { get; set; }

        // Descripción opcional
        [StringLength(250)]
        public string? Descripcion { get; set; }
    }
}