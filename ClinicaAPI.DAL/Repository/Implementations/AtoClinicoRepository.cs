using ClinicaAPI.Model;
using ClinicaAPI.Shared.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.DAL.Repository.Implementations
{
    public class AtoClinicoRepository : Repository<AtoClinico>,
IAtoClinicoRepository
    {
        public AtoClinicoRepository(ApplicationDbContext context) :
        base(context)
        { }
    }
}
