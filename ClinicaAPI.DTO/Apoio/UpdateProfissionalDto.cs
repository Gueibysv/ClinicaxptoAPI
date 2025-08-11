using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Apoio
{
    public class UpdateProfissionalDto
    {
        [Required(ErrorMessage = "O nome do profissional é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; }
        [StringLength(20)]
        public string? Telefone { get; set; }
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string? Email { get; set; }
    }
}
