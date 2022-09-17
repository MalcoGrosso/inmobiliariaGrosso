using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Models
{
    public class Pago
    {
        [Key]
        [Display (Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El contrato es necesario"), Display (Name ="Contrato")]
        public int IdContrato { get; set; }

        [Required(ErrorMessage ="El Numero de pago es requerido"), Display(Name ="Numero de Pago")]
        public int NumeroPago { get; set; }

        [Required(ErrorMessage = "La fecha de pago es requerida")]
        [Display(Name ="Fecha de pago")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage ="El monto es requerido")]
        public int Monto { get; set; }

        public Contrato Contrato { get; set; }
        
    }
}
