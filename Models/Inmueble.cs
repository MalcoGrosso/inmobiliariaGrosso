using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Models
{
   public class Inmueble
	{
        
        public int Id { get; set; }
 //     [Required(ErrorMessage = "Campo obligatorio")]
		public string? Direccion { get; set; }
//      [Required(ErrorMessage = "Campo requerido")]
		public int? Ambientes { get; set; }
 //     [Required(ErrorMessage = "Campo requerido")]
		public int? Superficie { get; set; }
 //     [Required(ErrorMessage = "Campo requerido")]
		public string? Latitud { get; set; }
 //     [Required(ErrorMessage = "Campo requerido")]
        public string? Longitud { get; set; }
 //     [Required(ErrorMessage = "Campo requerido")]
        public string Uso { get; set; }
//      [Required(ErrorMessage = "Campo requerido")]
        public string Tipo { get; set; } 
//      [Required(ErrorMessage = "Campo requerido")]
        public bool Disponible { get; set; }
//      [Required(ErrorMessage = "Campo requerido")]
        public string DisponibleN => Disponible ? "Sí" : "No";

        public double? Precio { get; set; }
               
        [Display(Name = "Propietario")]
        public int IdPropietario { get; set; }
        [Display(Name = "Dueño")]
        [ForeignKey(nameof(IdPropietario))]
        public Propietario Duenio { get; set; }
        public string Imagen { get; set; }
      /*  
        [NotMapped]
        public string ImagenGuardar { get; set; }
*/
    }
}