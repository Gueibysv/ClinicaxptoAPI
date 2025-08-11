using ClinicaAPI.DTO.Pedido;
using ClinicaAPI.Model;
using ClinicaAPI.Shared.Services;
using ClinicaAPI.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClinicaAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }
        [HttpPost("anonimo")]
        [AllowAnonymous] // Utentes anônimos podem criar pedidos
        public async Task<IActionResult> CreatePedidoAnonimo([FromBody]
            CreatePedidoAnonimoDto createDto)
        {
            var response = await
            _pedidoService.CreatePedidoAnonimoAsync(createDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("registado")]
        [Authorize(Roles = "Registado, registado")] // Apenas utentes registados podem criar pedidos
        public async Task<IActionResult> CreatePedidoRegistado([FromBody]
                CreatePedidoRegistadoDto createDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            ServiceResponse<PedidoDto>("Utilizador não autenticado.", false));
            var response = await
            _pedidoService.CreatePedidoRegistadoAsync(userId, createDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem alterar o estado
        public async Task<IActionResult> UpdateEstadoPedido(int id, [FromBody]
            UpdateEstadoPedidoDto updateDto)
        {
            var response = await _pedidoService.UpdateEstadoPedidoAsync(id,
            updateDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador,Administrativo,Registado, registado")]
        public async Task<IActionResult> GetPedidoById(int id)
        {
            var response = await _pedidoService.GetPedidoWithDetailsAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            // Regra de Negócio: Utente Registado só pode ver os próprios pedidos
        if (User.IsInRole("Registado"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (response.Data?.UtilizadorId != userId)
                {
                    return Forbid();
                }
            }
            return Ok(response);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem listar todos
        public async Task<IActionResult> GetAllPedidos()
        {
            var response = await
            _pedidoService.GetAllPedidosWithDetailsAsync();
            return Ok(response);
        }
        [HttpGet("my-pedidos")]
        [Authorize(Roles = "Registado, registado")] // Utente Registado pode ver seus próprios pedidos
        public async Task<IActionResult> GetMyPedidos()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            ServiceResponse<IEnumerable<PedidoDto>>("Utilizador não autenticado.", false));
            var response = await
            _pedidoService.GetPedidosByUtilizadorIdAsync(userId);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")] // Apenas administradores podem eliminar pedidos

        public async Task<IActionResult> DeletePedido(int id)
        {
                var response = await _pedidoService.DeleteAsync(id);
            if (!response.Success)
            {

                return BadRequest(response);

            }
            return Ok(response);


        }

    }
}
