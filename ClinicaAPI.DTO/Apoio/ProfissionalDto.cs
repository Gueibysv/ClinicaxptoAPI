using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class ProfissionalDto
    {
        public int ProfissionalId { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
