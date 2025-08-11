using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class CreatePedidoAtoClinicoDto
    {
        [Required(ErrorMessage = "O ID do ato clínico é obrigatório.")]
        public int AtoClinicoId { get; set; }
        public int? SubsistemaSaudeId { get; set; }
        public int? ProfissionalId { get; set; }
    }

}
