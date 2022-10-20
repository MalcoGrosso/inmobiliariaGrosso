using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InmobiliariaGrosso.Models
{
    public class Propietario
    { 

        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public Propietario(){}


        
        public int Id { get; set; }
     //   [Required(ErrorMessage = "Campo obligatorio")]
        public string? Dni { get; set; }
   //     [Required(ErrorMessage = "Campo obligatorio")]
        public string? Nombre { get; set; }
   //     [Required(ErrorMessage = "Campo obligatorio")]
        public string? Apellido { get; set; }
    //    [Required(ErrorMessage = "Campo obligatorio")]
        public string? Telefono { get; set; }
    //    [Required(ErrorMessage = "Campo obligatorio")]
        public string? Email { get; set; }
    //    [Required(ErrorMessage = "Campo obligatorio")]
        public string? Clave { get; set; }
        
    }
}