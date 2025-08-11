using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class PedidoAtoClinicoDto
    {
        public int PedidoAtoClinicoId { get; set; }
        public int PedidoId { get; set; }
        public AtoClinicoDto AtoClinico { get; set; }
        public SubsistemaSaudeDto? SubsistemaSaude { get; set; }
        public ProfissionalDto? Profissional { get; set; }
    }

}
