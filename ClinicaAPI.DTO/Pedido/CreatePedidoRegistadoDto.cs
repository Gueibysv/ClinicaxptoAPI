using ClinicaAPI.DTO.Apoio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Pedido
{
    public class CreatePedidoRegistadoDto
    {
        // Não precisa de dados do utente, pois o ID do utilizador autenticado será usado
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
