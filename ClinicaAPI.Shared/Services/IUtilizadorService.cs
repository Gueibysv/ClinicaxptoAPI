using ClinicaAPI.DTO.Auth;
using ClinicaAPI.DTO.Utente;
using ClinicaAPI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IUtilizadorService
    {
        Task<ServiceResponse<UtenteDto>> GetUtilizadorProfileAsync(string
        userId);
        Task<ServiceResponse<UtenteDto>> UpdateUtilizadorProfileAsync(string
        userId, UpdateUtenteDto updateDto);
        Task<ServiceResponse<IEnumerable<UtenteDto>>>
        GetAllUtilizadoresAsync();
        Task<ServiceResponse<bool>> DeleteUtilizadorAsync(string userId);
        Task<ServiceResponse<AuthResponseDto>>
        RegisterAnonymousUserAsync(string userId, RegisterDto registerDto);
    }
}
