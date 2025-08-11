using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IProfissionalService : IService<Profissional,ProfissionalDto,
CreateProfissionalDto, UpdateProfissionalDto>
    {
        // Métodos específicos para Profissional, se houver
    }

}
