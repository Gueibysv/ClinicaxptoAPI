using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class AtoClinicoDto
    {
        public int AtoClinicoId { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
    }

}
