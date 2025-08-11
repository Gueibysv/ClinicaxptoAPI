using AutoMapper;
using ClinicaAPI.DTO.Auth;
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
    public class AuthService : IAuthService
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public AuthService(
        UserManager<Utilizador> userManager,
        SignInManager<Utilizador> signInManager,
        RoleManager<IdentityRole> roleManager,
        IJwtService jwtService,
        IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginDto
        loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new ServiceResponse<AuthResponseDto>("Credenciais inválidas.", false);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user,
            loginDto.Password, false);
            if (!result.Succeeded)
            {
                return new ServiceResponse<AuthResponseDto>("Credenciais inválidas.", false);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id, user.Email,
            user.UserName, roles);
            var authResponse = _mapper.Map<AuthResponseDto>(user);
            authResponse.Token = token;
            authResponse.Roles = roles.ToList();
            // A expiração deve ser definida no JwtService ou lida da configuração
            // Por simplicidade, vamos assumir 60 minutos como no JwtService 
            authResponse.Expiration = DateTime.UtcNow.AddMinutes(60);
            return new ServiceResponse<AuthResponseDto>(authResponse, "Login realizado com sucesso.");
        }
        public async Task<ServiceResponse<AuthResponseDto>>
        RegisterAsync(RegisterDto registerDto)
        {
            var userExists = await
            _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
            {
                return new ServiceResponse<AuthResponseDto>("Utilizador com este email já existe.", false);
            }
            var user = _mapper.Map<Utilizador>(registerDto);
            user.EmailConfirmed = true; // Confirma o email automaticament para este cenário
            user.EstaRegistrado = true; // Marca como registado
            var result = await _userManager.CreateAsync(user,
            registerDto.Password);
            if (!result.Succeeded)
            {
                return new ServiceResponse<AuthResponseDto>(
                "Falha ao registar utilizador.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            // Atribuir role padrão se não for especificada ou se a role especificada não existir
            var roleToAssign = registerDto.Role ??
            TipoUtilizador.Registado.ToString();
            if (!await _roleManager.RoleExistsAsync(roleToAssign))
            {
                // Se a role não existe, cria-a (apenas para roles esperadas, como
                // 'Utente Registado', 'Administrativo', 'Administrador')
                await _roleManager.CreateAsync(new IdentityRole(roleToAssign));
            }
            await _userManager.AddToRoleAsync(user, roleToAssign);
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user.Id, user.Email,
            user.UserName, roles);
            var authResponse = _mapper.Map<AuthResponseDto>(user);
            authResponse.Token = token;
            authResponse.Roles = roles.ToList();
            authResponse.Expiration = DateTime.UtcNow.AddMinutes(60);
            return new ServiceResponse<AuthResponseDto>(authResponse,
            "Utilizador registado com sucesso.");
        }
        public async Task<ServiceResponse<bool>> ChangePasswordAsync(string
        userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>("Utilizador não encontrado.",
                false);
            }
            var result = await _userManager.ChangePasswordAsync(user,
            changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>(
                "Falha ao alterar password.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            return new ServiceResponse<bool>(true, "Password alterada com sucesso.");
        }
        public async Task<ServiceResponse<UserDto>> GetUserByIdAsync(string
        userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<UserDto>("Utilizador não encontrado.", false);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles.ToList();
            return new ServiceResponse<UserDto>(userDto);
        }
        public async Task<ServiceResponse<IEnumerable<UserDto>>>
        GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = roles.ToList();
                userDtos.Add(userDto);
            }
            return new ServiceResponse<IEnumerable<UserDto>>(userDtos);
        }
        public async Task<ServiceResponse<bool>> AssignRoleAsync(AssignRoleDto
        assignRoleDto)
        {
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);
            if (user == null)
            {
                return new ServiceResponse<bool>("Utilizador não encontrado.",
                false);
            }
            if (!await _roleManager.RoleExistsAsync(assignRoleDto.Role))
            {
                return new ServiceResponse<bool>($"Role '{assignRoleDto.Role}' não existe.", false);
            }
            var result = await _userManager.AddToRoleAsync(user,
            assignRoleDto.Role);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>(
                "Falha ao atribuir role.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            return new ServiceResponse<bool>(true, "Role atribuída com sucesso.");
        }
        public async Task<ServiceResponse<bool>> RemoveRoleAsync(AssignRoleDto
        removeRoleDto)
        {
            var user = await _userManager.FindByIdAsync(removeRoleDto.UserId);
            if (user == null)
            {
                return new ServiceResponse<bool>("Utilizador não encontrado.",
                false);
            }
            if (!await _roleManager.RoleExistsAsync(removeRoleDto.Role))
            {
                return new ServiceResponse<bool>($"Role '{removeRoleDto.Role}' não existe.", false);
            }
            var result = await _userManager.RemoveFromRoleAsync(user,
            removeRoleDto.Role);
            if (!result.Succeeded)
            {
                return new ServiceResponse<bool>(
                "Falha ao remover role.",
                false,
                result.Errors.Select(e => e.Description).ToList()
                );
            }
            return new ServiceResponse<bool>(true, "Role removida com sucesso.");
        }
        public async Task<ServiceResponse<bool>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>("Utilizador não encontrado.",
                false);
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
    }
}
