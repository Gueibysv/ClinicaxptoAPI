using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Expiration { get; set; }
    }
}
