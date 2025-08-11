using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Auth
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "O ID do utilizador é obrigatório.")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "A role é obrigatória.")]
        public string Role { get; set; }
    }

}
