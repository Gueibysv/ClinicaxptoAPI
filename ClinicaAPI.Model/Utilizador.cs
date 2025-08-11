
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClinicaAPI.Model
{
        public class Utilizador : IdentityUser
    {
            // Propriedades personalizadas do utilizador
            [Required]
            [StringLength(255)]
            public string NomeCompleto { get; set; }
            [StringLength(20)]
            public string? NumeroUtente { get; set; } // Pode ser nulo para utentes anônimos
            public string? FotografiaUrl { get; set; }
            public DateTime? DataNascimento { get; set; }
            [StringLength(50)]
            public string? Genero { get; set; }
            [StringLength(500)]
            public string? Morada { get; set; }
            // Propriedade para identificar se o utilizador está registado (tem credenciais de login)
            public bool EstaRegistrado { get; set; } = false;
            // Propriedade para o tipo de utilizador (para simplificar o mapeamentode roles)
            public TipoUtilizador TipoUtilizador { get; set; } = TipoUtilizador.Anonimo;
            // Relacionamentos
            public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        }
        public enum TipoUtilizador
        {
            Anonimo = 0,
            Registado = 1,
            Administrativo = 2,
            Administrador = 3
        }

    }
