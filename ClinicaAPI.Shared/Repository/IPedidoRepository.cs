using ClinicaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Repository
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido?> GetPedidoWithDetailsAsync(int pedidoId);
        Task<IEnumerable<Pedido>> GetPedidosByUtilizadorIdAsync(string
        utilizadorId);
    }

}
