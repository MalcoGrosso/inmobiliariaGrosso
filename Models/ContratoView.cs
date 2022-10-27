using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaGrosso.Models;

namespace InmobiliariaGrosso.Models
{
 
	 
	public class ContratoView
    {
public ContratoView(Task<Contrato?> contrato) {}

public ContratoView(IEnumerable<Contrato?> contrato) {}


public ContratoView(Contrato c)
{
	this.Id = c.Id;
	this.IdInmueble = c.IdInmueble;
	this.IdInquilino = c.IdInquilino;
	this.Desde = c.Desde;
	this.Hasta = c.Hasta;
	this.MontoM = c.MontoM;
    this.Inquilino = c.Inquilino;
	

}

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
        public DateTime FechaHoy { get; set; }

        [Display(Name = "Fecha Actual")]
      
        public String? FechaHoy1 { get; set; }

        [Display(Name = "Multa si se cancela el Contrato")]
      
        public int Deuda { get; set; }
        
        [ForeignKey(nameof(IdInquilino))]
        public Inquilino? Inquilino { get; set; }

	}
	
	
}
