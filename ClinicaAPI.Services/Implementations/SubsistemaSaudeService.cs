using AutoMapper;
using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Model;
using ClinicaAPI.Shared.Repository;
using ClinicaAPI.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Services.Implementations
{
    public class SubsistemaSaudeService : Service<SubsistemaSaude,
        SubsistemaSaudeDto, CreateSubsistemaSaudeDto, UpdateSubsistemaSaudeDto>,
        ISubsistemaSaudeService
    {
        public SubsistemaSaudeService(
        ISubsistemaSaudeRepository repository,
        IMapper mapper)
        : base(repository, mapper) { }
    }

}
