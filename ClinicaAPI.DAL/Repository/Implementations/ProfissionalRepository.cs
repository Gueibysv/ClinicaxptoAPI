using ClinicaAPI.Model;
using ClinicaAPI.Shared.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DAL.Repository.Implementations
{
    public class ProfissionalRepository : Repository<Profissional>, IProfissionalRepository
    {
        public ProfissionalRepository(ApplicationDbContext context) :
        base(context)
        { }
    }
}
