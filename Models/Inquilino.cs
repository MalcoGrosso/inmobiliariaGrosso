using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Models
{
    public class Inquilino
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
