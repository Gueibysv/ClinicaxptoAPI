using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.DTO.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Pedido
{
    public class CreatePedidoAnonimoDto
    {
        [Required(ErrorMessage = "O nome completo do utente anônimo é obrigatório.")]
        [StringLength(255)]
        public string NomeCompletoAnonimo { get; set; }
        [Required(ErrorMessage = "O email do utente anônimo é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string EmailAnonimo { get; set; }
        [Required(ErrorMessage = "O telemóvel do utente anônimo é obrigatório.")]
        [StringLength(20)]
        public string TelemovelAnonimo { get; set; }
        [Required(ErrorMessage = "A data de início desejada é obrigatória.")]
        public DateTime DataInicioDesejada { get; set; }
        [Required(ErrorMessage = "A data de fim desejada é obrigatória.")]
        public DateTime DataFimDesejada { get; set; }
        [StringLength(100)]
        public string? HorarioSolicitado { get; set; }
        [StringLength(1000)]
        public string? ObservacoesAdicionais { get; set; }
        [Required(ErrorMessage = "Pelo menos um ato clínico é obrigatório.")]
        public List<CreatePedidoAtoClinicoDto> PedidoAtoClinicos { get; set; }
    }
}
