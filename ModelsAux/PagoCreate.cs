using InmobiliariaGrosso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.ModelsAux
{
    public class PagoCreate
    {
        public Inquilino Inquilino { get; set; }
        public IList<Contrato> Contratos { get; set; }
    }
}
