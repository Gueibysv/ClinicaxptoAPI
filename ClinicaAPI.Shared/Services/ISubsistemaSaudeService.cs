using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface ISubsistemaSaudeService : IService<SubsistemaSaude, SubsistemaSaudeDto, CreateSubsistemaSaudeDto, UpdateSubsistemaSaudeDto>
    {
        // Métodos específicos para SubsistemaSaude, se houver
    }
}
