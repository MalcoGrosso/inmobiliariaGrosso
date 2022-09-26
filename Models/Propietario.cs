using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGrosso.Models
{
    public class Propietario
    { 
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Dni { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Email { get; set; }

    }
}