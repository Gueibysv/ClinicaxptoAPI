using ClinicaAPI.DTO.Auth;
using ClinicaAPI.Model;
using ClinicaAPI.Shared.Services;
using ClinicaAPI.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClinicaAPI.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;
            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }
            [HttpPost("login")]
            [AllowAnonymous] // Permite acesso sem autenticação
            public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
            {
                var response = await _authService.LoginAsync(loginDto);
                if (!response.Success)
                {
                    return Unauthorized(response); // Retorna 401 para falha de login
                }
                return Ok(response);
            }
            [HttpPost("register")]
            [Authorize(Roles = "Administrador")] // Apenas administradores podem registar novos utilizadores
            public async Task<IActionResult> Register([FromBody] RegisterDto
                registerDto)
            {
                var response = await _authService.RegisterAsync(registerDto);
                if (!response.Success)
                {
                    return BadRequest(response); // Retorna 400 para falha de registo
                }
                return Ok(response);
            }
            [HttpPost("change-password")]
            [Authorize] // Requer autenticação
            public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Obtém o ID do utilizador autenticado
            if (userId == null) return Unauthorized(new ServiceResponse<bool>
            ("Utilizador não autenticado.", false));
                var response = await _authService.ChangePasswordAsync(userId,
                changePasswordDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            [HttpGet("profile")]
            [Authorize] // Requer autenticação
        public async Task<IActionResult> GetProfile()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized(new
                ServiceResponse<UserDto>("Utilizador não autenticado.", false));
                var response = await _authService.GetUserByIdAsync(userId);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            [HttpGet("users")]
            [Authorize(Roles = "Administrador")] // Apenas administradores podem listar todos os utilizadores
            public async Task<IActionResult> GetAllUsers()
            {
                var response = await _authService.GetAllUsersAsync();
                return Ok(response);
            }
            [HttpPost("assign-role")]
            [Authorize(Roles = "Administrador")] // Apenas administradores podem atribuir roles
            public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto
               assignRoleDto)
            {
                var response = await _authService.AssignRoleAsync(assignRoleDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            [HttpPost("remove-role")]
            [Authorize(Roles = "Administrador")] // Apenas administradores podem remover roles
            public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto
               removeRoleDto)
            {
                var response = await _authService.RemoveRoleAsync(removeRoleDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            [HttpDelete("users/{userId}")]
            [Authorize(Roles = "Administrador")] // Apenas administradores podem eliminar utilizadores
            public async Task<IActionResult> DeleteUser(string userId)
            {
                var response = await _authService.DeleteUserAsync(userId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
        }
    }
