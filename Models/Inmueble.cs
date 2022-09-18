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
		public string Direccion { get; set; }
		public int Ambientes { get; set; }
		public int Superficie { get; set; }
		public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Uso { get; set; }
        public string Tipo { get; set; } 
        public bool Disponible { get; set; }
        public string DisponibleN => Disponible ? "Sí" : "No";
               
        [Display(Name = "Propietario")]
        public int IdPropietario { get; set; }
        [Display(Name = "Dueño")]
        public Propietario Duenio { get; set; }
    }
}