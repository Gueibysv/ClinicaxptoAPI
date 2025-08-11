using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Auth
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "A password atual é obrigatória.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "A nova password é obrigatória.")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e nomáximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "A nova password e a confirmação da nova password não coincidem.")]
        public string ConfirmNewPassword { get; set; }
    }

}
