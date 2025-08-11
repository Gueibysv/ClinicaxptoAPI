using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password é obrigatória.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A password e a confirmação da password não coincidem.")]
        public string ConfirmPassword { get; set; }

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

        // Opcional: para o administrador especificar a role ao criar um utilizador
        public string? Role { get; set; }



    }
}
