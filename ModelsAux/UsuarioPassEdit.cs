using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.ModelsAux
{
    public class UsuarioPassEdit
    {
        public int Id { get; set; }

        [MinLength(1), MaxLength(10)]
        [Required(ErrorMessage ="Campo obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña antigua")]
        public string OldPass { get; set; }

        [MinLength(1), MaxLength(10)]
        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña nueva")]
        public string NewPass { get; set; }

    }
}
