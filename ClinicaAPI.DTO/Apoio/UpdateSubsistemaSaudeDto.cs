using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class UpdateSubsistemaSaudeDto
    {
        [Required(ErrorMessage = "O nome do subsistema de saúde é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; }
        [StringLength(500)]
        public string? Descricao { get; set; }
    }
}
