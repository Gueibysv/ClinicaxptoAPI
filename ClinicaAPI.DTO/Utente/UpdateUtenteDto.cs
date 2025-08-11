using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Utente
{
    public class UpdateUtenteDto
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(255)]
        public string NomeCompleto { get; set; }
        [StringLength(20)]
        public string? NumeroUtente { get; set; }
        public string? FotografiaUrl { get; set; }
        public DateTime? DataNascimento { get; set; }
        [StringLength(50)]
        public string? Genero { get; set; }
        [StringLength(20)]
        public string? Telemovel { get; set; }
        [StringLength(500)]
        public string? Morada { get; set; }
    }
}
