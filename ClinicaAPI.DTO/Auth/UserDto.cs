using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Auth
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string NomeCompleto { get; set; }
        public string? NumeroUtente { get; set; }
        public string? Telemovel { get; set; }
        public bool EstaRegistrado { get; set; }
        public List<string> Roles { get; set; }
    }

}
