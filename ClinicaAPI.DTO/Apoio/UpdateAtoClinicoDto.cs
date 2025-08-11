using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class UpdateAtoClinicoDto
    {
        [Required(ErrorMessage = "O tipo de ato clínico é obrigatório.")]
        [StringLength(100)]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "O nome do ato clínico é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }
        [StringLength(1000)]
        public string? Descricao { get; set; }
    }
}
