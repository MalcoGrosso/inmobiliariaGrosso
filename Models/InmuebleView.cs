using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaGrosso.Models;

namespace InmobiliariaGrosso.Models
{
 
	 
	public class InmuebleView
    {

		public InmuebleView(Task<Inmueble?> inmmueble) {}

		public InmuebleView(IQueryable<Inmueble?> inmmueble ) {

		}

		public InmuebleView(Inmueble i)
		{
			this.Id = i.Id;
			this.Direccion = i.Direccion;
			this.Ambientes = i.Ambientes;
			this.Superficie = i.Superficie;
			this.Latitud = i.Latitud;
			this.Longitud = i.Longitud;
			this.Uso = i.Uso;
			this.Tipo = i.Tipo;
			this.Disponible = i.Disponible;
			this.Precio = i.Precio;
			

		}
		

    public int Id { get; set; }
 //       [Required(ErrorMessage = "Campo obligatorio")]
		public string? Direccion { get; set; }
//        [Required(ErrorMessage = "Campo requerido")]
		public int? Ambientes { get; set; }
 //       [Required(ErrorMessage = "Campo requerido")]
		public int? Superficie { get; set; }
 //       [Required(ErrorMessage = "Campo requerido")]
		public string? Latitud { get; set; }
 //       [Required(ErrorMessage = "Campo requerido")]
        public string? Longitud { get; set; }
 //       [Required(ErrorMessage = "Campo requerido")]
        public string Uso { get; set; }
//        [Required(ErrorMessage = "Campo requerido")]
        public string Tipo { get; set; } 
//        [Required(ErrorMessage = "Campo requerido")]
        public bool Disponible { get; set; }
//        [Required(ErrorMessage = "Campo requerido")]
        public double? Precio { get; set; }
               

	}
	 
}
