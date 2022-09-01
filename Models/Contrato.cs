using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int Precio{ get; set; }

        //Relaciones
        public Inmueble Inmueble { get; set; }
        public Inquilino Inquilino { get; set; }

    }
}
