using AutoMapper;
using ClinicaAPI.DTO.Auth;
using ClinicaAPI.DTO.Utente;
using ClinicaAPI.Model;
using ClinicaAPI.Shared.Services;
using ClinicaAPI.Shared.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Services.Implementations
{
    public class UtilizadorService : IUtilizadorService
    {
       
        private readonly UserManager<Utilizador> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; // Para registar utente anônimo como registado
        private readonly IEmailService _emailService;
        public UtilizadorService(
            UserManager<Utilizador> userManager,
            IMapper mapper,
            IAuthService authService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<UtenteDto>>
        GetUtilizadorProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<UtenteDto>("Utilizador não encontrado.", false);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UtenteDto>(user);
            userDto.Roles = roles.ToList();
            return new ServiceResponse<UtenteDto>(userDto);
        }
        public async Task<ServiceResponse<UtenteDto>>
        UpdateUtilizadorProfileAsync(string userId, UpdateUtenteDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<UtenteDto>("Utilizador não encontrado para atualização.", false);
            }
            // Atualizar propriedades do IdentityUser
            user.Email = updateDto.Email;
            user.UserName = updateDto.Email; // Manter UserName igual ao Email
            user.PhoneNumber = updateDto.Telemovel;
            // Atualizar propriedades personalizadas do Utilizador
            user.NomeCompleto = updateDto.NomeCompleto;
            user.NumeroUtente = updateDto.NumeroUtente;
            user.FotografiaUrl = updateDto.FotografiaUrl;
            user.DataNascimento = updateDto.DataNascimento;
            user.Genero = updateDto.Genero;
            user.Morada = updateDto.Morada;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResponse<UtenteDto>(
                "Falha ao atualizar perfil do utilizador.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UtenteDto>(user);
            userDto.Roles = roles.ToList();
            return new ServiceResponse<UtenteDto>(userDto, "Perfil do utilizador atualizado com sucesso.");
        }
        public async Task<ServiceResponse<IEnumerable<UtenteDto>>>
        GetAllUtilizadoresAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UtenteDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UtenteDto>(user);
                userDto.Roles = roles.ToList();
                userDtos.Add(userDto);
            }
            return new ServiceResponse<IEnumerable<UtenteDto>>(userDtos);
        }
        public async Task<ServiceResponse<bool>> DeleteUtilizadorAsync(string
        userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>("Utilizador não encontrado para exclusão.", false);
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>(
                "Falha ao eliminar utilizador.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            return new ServiceResponse<bool>(true, "Utilizador eliminado com sucesso.");
        }
        public async Task<ServiceResponse<AuthResponseDto>>
        RegisterAnonymousUserAsync(string userId, RegisterDto registerDto)
        {
         // Este método é para converter um Utilizador existente (criado via pedido anônimo) em um Utilizador Registado
        // e atribuir-lhe credenciais de login.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<AuthResponseDto>("Utilizador anônimo não encontrado.", false);
            }
            // Verificar se o utilizador já tem credenciais de login
            if (user.PasswordHash != null)
            {
                return new ServiceResponse<AuthResponseDto>("Utilizador já registado.", false);
            }
            // Atualizar dados do utilizador com base no RegisterDto
            user.Email = registerDto.Email;
            user.UserName = registerDto.Email;
            user.PhoneNumber = registerDto.Telemovel;
            user.NomeCompleto = registerDto.NomeCompleto;
            user.NumeroUtente = registerDto.NumeroUtente;
            user.FotografiaUrl = registerDto.FotografiaUrl;
            user.DataNascimento = registerDto.DataNascimento;
            user.Genero = registerDto.Genero;
            user.Morada = registerDto.Morada;
            user.EstaRegistrado = true; // Marcar como registado
            user.TipoUtilizador = Model.TipoUtilizador.Registado; // Definir tipo como Registado
            var result = await _userManager.AddPasswordAsync(user,
            registerDto.Password);
            if (!result.Succeeded)
            {
                return new ServiceResponse<AuthResponseDto>(
                "Falha ao registar utilizador anônimo.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            // Atribuir a role 'Utente Registado'
            if (!await _userManager.IsInRoleAsync(user,
            Model.TipoUtilizador.Registado.ToString()))
            {
                await _userManager.AddToRoleAsync(user,
                Model.TipoUtilizador.Registado.ToString());
            }
            var roles = await _userManager.GetRolesAsync(user);
            var authResponse = _mapper.Map<AuthResponseDto>(user);
            authResponse.Roles = roles.ToList();
            authResponse.Expiration = DateTime.UtcNow.AddMinutes(60);
            // ENVIO DE EMAIL
            string assunto = "Bem-vindo à Clínica XPTO - Acesso à sua conta";
            string corpo = $@"
                <h3>Conta criada com sucesso</h3>
                <p>Estimado(a) {user.NomeCompleto},</p>
                <p>A sua conta na Clínica XPTO foi criada com sucesso.</p>
                <p><strong>Credenciais de acesso:</strong></p>
                <ul>
               <li>Email: <strong>{registerDto.Email}</strong></li>
                <li>Senha: <strong>{registerDto.Password}</strong></li>
                 </ul>
                <p>Por favor, aceda ao sistema para acompanhar os seus pedidos e marcações.</p>
                <p>Obrigado por confiar em nós.</p>";

            await _emailService.SendEmailAsync(user.Email, assunto, corpo);
            return new ServiceResponse<AuthResponseDto>(authResponse, "Utilizador anônimo registado com sucesso.");
        }
    }
}
