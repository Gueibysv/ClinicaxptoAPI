using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IAtoClinicoService : IService<AtoClinico, AtoClinicoDto, CreateAtoClinicoDto, UpdateAtoClinicoDto>
    {
        // Métodos específicos para AtoClinico, se houver
    }
}
