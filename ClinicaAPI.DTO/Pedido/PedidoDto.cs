using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.DTO.Auth;
using ClinicaAPI.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Pedido
{
    public class PedidoDto
    {
        public int PedidoId { get; set; }
        public string? NomeCompletoAnonimo { get; set; }
        public string? EmailAnonimo { get; set; }
        public string? TelemovelAnonimo { get; set; }
        public string? UtilizadorId { get; set; }
        public UserDto? Utilizador { get; set; } // DTO simplificado do utilizador
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataInicioDesejada { get; set; }
        public DateTime DataFimDesejada { get; set; }
        public string? HorarioSolicitado { get; set; }
        public string? ObservacoesAdicionais { get; set; }
        public EstadoPedido Estado { get; set; }
        public List<PedidoAtoClinicoDto> PedidoAtoClinicos { get; set; }
    }

}
