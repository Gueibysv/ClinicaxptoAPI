using ClinicaAPI.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DTO.Pedido
{
    public class UpdateEstadoPedidoDto
    {
        [Required(ErrorMessage = "O novo estado é obrigatório.")]
        public EstadoPedido NovoEstado { get; set; }
    }
}
