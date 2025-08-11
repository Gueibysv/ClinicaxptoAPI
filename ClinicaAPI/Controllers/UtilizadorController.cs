using ClinicaAPI.DTO.Auth;
using ClinicaAPI.DTO.Utente;
using ClinicaAPI.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Identity;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorService _utilizadorService;
        private readonly IAuthService _authService; // Para registar utente anônimo como registado
        public UtilizadorController(IUtilizadorService utilizadorService,IAuthService authService)
        {
            _utilizadorService = utilizadorService;
            _authService = authService;
        }
        [HttpGet("{userId}")]
        [Authorize(Roles = "Administrador,Administrativo,Utente Registado")] //Administradores, administrativos e o próprio utente registado
        public async Task<IActionResult> GetUtilizadorById(string userId)
        {
            // Regra de Negócio: Utente Registado só pode ver o próprio perfil
            if (User.IsInRole("Utente Registado") &&
            User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return Forbid(); // Retorna 403 Forbidden
            }
            var response = await
            _utilizadorService.GetUtilizadorProfileAsync(userId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("{userId}")]
        [Authorize(Roles = "Administrador,Administrativo,Registado, registado")] //Administradores, administrativos e o próprio utente registado
        public async Task<IActionResult> UpdateUtilizador(string userId,[FromBody] UpdateUtenteDto updateDto)
        {
            // Regra de Negócio: Utente Registado só pode atualizar o próprio perfil
            if (User.IsInRole("Registado, registado") &&User.FindFirstValue(ClaimTypes.NameIdentifier) != userId)
            {
                return Forbid();
            }
            var response = await
            _utilizadorService.UpdateUtilizadorProfileAsync(userId, updateDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem listar todos
        public async Task<IActionResult> GetAllUtilizadores()
        {
            var response = await _utilizadorService.GetAllUtilizadoresAsync();
            return Ok(response);
        }
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Administrador")] // Apenas administradores podem eliminar utilizadores
        public async Task<IActionResult> DeleteUtilizador(string userId)
        {
            var response = await
            _utilizadorService.DeleteUtilizadorAsync(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("{userId}/register-anonymous")]
        [Authorize(Roles = "Administrador")] // Apenas administradores podem converter anônimos em registados
        public async Task<IActionResult> RegisterAnonymousUser(string userId,[FromBody] RegisterDto registerDto)
        {
            var response = await
            _utilizadorService.RegisterAnonymousUserAsync(userId, registerDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
