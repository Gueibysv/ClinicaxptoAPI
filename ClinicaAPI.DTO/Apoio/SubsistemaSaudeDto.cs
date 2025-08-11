using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class SubsistemaSaudeDto
    {
        public int SubsistemaSaudeId { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
    }

}
