using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubsistemaSaudeController : ControllerBase
    {
        private readonly ISubsistemaSaudeService _subsistemaSaudeService;
        public SubsistemaSaudeController(ISubsistemaSaudeService
        subsistemaSaudeService)
        {
            _subsistemaSaudeService = subsistemaSaudeService;
        }
        [HttpGet]
        [AllowAnonymous] // Informação pública para formulários de marcação
        public async Task<IActionResult> GetAllSubsistemaSaude()
        {
            var response = await _subsistemaSaudeService.GetAllAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        [AllowAnonymous] // Informação pública
        public async Task<IActionResult> GetSubsistemaSaudeById(int id)
        {
            var response = await _subsistemaSaudeService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem adicionar
        public async Task<IActionResult> AddSubsistemaSaude([FromBody] CreateSubsistemaSaudeDto createDto)
        {
            var response = await _subsistemaSaudeService.AddAsync(createDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem atualizar
        public async Task<IActionResult> UpdateSubsistemaSaude(int id, [FromBody] UpdateSubsistemaSaudeDto updateDto)
        {
            var response = await _subsistemaSaudeService.UpdateAsync(id,
            updateDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")] // Apenas administradores podem eliminar
        public async Task<IActionResult> DeleteSubsistemaSaude(int id)
        {
            var response = await _subsistemaSaudeService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }

}
