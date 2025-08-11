using ClinicaAPI.Model;
using ClinicaAPI.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DAL.Repository.Implementations
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Pedido?> GetPedidoWithDetailsAsync(int pedidoId)
        {
            return await _dbSet
            .Include(p => p.Utilizador)
            .Include(p => p.PedidoAtoClinicos)
            .ThenInclude(pac => pac.AtoClinico)
            .Include(p => p.PedidoAtoClinicos)
            .ThenInclude(pac => pac.Profissional)
            .Include(p => p.PedidoAtoClinicos)
            .ThenInclude(pac => pac.SubsistemaSaude)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
        }
        public async Task<IEnumerable<Pedido>>
        GetPedidosByUtilizadorIdAsync(string utilizadorId)
        {
            return await _dbSet
            .Where(p => p.UtilizadorId == utilizadorId)
            .Include(p => p.PedidoAtoClinicos)
            .ThenInclude(pac => pac.AtoClinico)
            .Include(p => p.PedidoAtoClinicos)
            .ThenInclude(pac => pac.Profissional)
            .Include(p => p.PedidoAtoClinicos)
            .ThenInclude(pac => pac.SubsistemaSaude)
            .ToListAsync();
        }
    }

}
