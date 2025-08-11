using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Utente
{
    public class UtenteDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string NomeCompleto { get; set; }
        public string? NumeroUtente { get; set; }
        public string? FotografiaUrl { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? Telemovel { get; set; }
        public string? Morada { get; set; }
        public bool EstaRegistrado { get; set; }
        public List<string> Roles { get; set; }
    }
}
