using ClinicaAPI.DTO.Pedido;
using ClinicaAPI.Model;
using ClinicaAPI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IPedidoService : IService<Pedido,PedidoDto, CreatePedidoRegistadoDto, PedidoDto>
    {
        Task<ServiceResponse<PedidoDto>>
        CreatePedidoAnonimoAsync(CreatePedidoAnonimoDto createDto);
        Task<ServiceResponse<PedidoDto>> CreatePedidoRegistadoAsync(string
        utilizadorId, CreatePedidoRegistadoDto createDto);
        Task<ServiceResponse<bool>> UpdateEstadoPedidoAsync(int pedidoId,
        UpdateEstadoPedidoDto updateDto);
        Task<ServiceResponse<IEnumerable<PedidoDto>>>
        GetAllPedidosWithDetailsAsync();
        Task<ServiceResponse<PedidoDto>> GetPedidoWithDetailsAsync(int
        pedidoId);
        Task<ServiceResponse<IEnumerable<PedidoDto>>>
        GetPedidosByUtilizadorIdAsync(string utilizadorId);
    }
}
