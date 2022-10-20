using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Models
{
    public class Contrato
    {
        [Display(Name  = "Índice")]
        public int Id { get; set; }

        [Display(Name = "Inmueble")]
        public int IdInmueble { get; set; }

        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Desde { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Hasta { get; set; }
        
        [Display(Name = "Monto Mensual")]
        public int MontoM { get; set; }
        [Required(ErrorMessage = "Campo requerido")]

        [Display(Name = "Fecha Actual")]
        [NotMapped]
        public DateTime FechaHoy { get; set; }

        [Display(Name = "Fecha Actual")]
        [NotMapped]
        public String? FechaHoy1 { get; set; }

        [Display(Name = "Multa si se cancela el Contrato")]
        [NotMapped]
        public int Deuda { get; set; }
        

        //Relaciones
        [ForeignKey(nameof(IdInmueble))]
        public Inmueble? Inmueble { get; set; } 

        [ForeignKey(nameof(IdInquilino))]
        public Inquilino? Inquilino { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           var errors = new List<ValidationResult>();

            if (Desde > Hasta){
                errors.Add(new ValidationResult("Hola", new String[] { "Hasta"}));
            }

            return errors;
              
        }

    }
}
