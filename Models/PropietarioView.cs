using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaGrosso.Models;

namespace InmobiliariaGrosso.Models
{
 
	 
	public class PropietarioView
    {
public PropietarioView(Task<Propietario?> prop) {}

public PropietarioView(Propietario p)
{
	this.Apellido = p.Apellido;
	this.Dni = p.Dni;
	this.Email = p.Email;
	this.Nombre = p.Nombre;
	this.Telefono = p.Telefono;
	this.Id = p.Id;

}

        [Key]
        public int Id { get; set; }
		[Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required]
		public string Dni { get; set; }
		public string Telefono { get; set; }
	//	[Required, EmailAddress]
		public string Email { get; set; }
		


	    public override string ToString()
    {
        return Apellido + ", " + Nombre;
    }




	}
	
	
}
