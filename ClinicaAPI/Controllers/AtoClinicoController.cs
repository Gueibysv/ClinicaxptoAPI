using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtoClinicoController : ControllerBase
    {
        private readonly IAtoClinicoService _atoClinicoService;
        public AtoClinicoController(IAtoClinicoService atoClinicoService)
        {
            _atoClinicoService = atoClinicoService;
        }
        [HttpGet]
        [AllowAnonymous] // Informação pública para formulários de marcação
        public async Task<IActionResult> GetAllAtoClinicos()
        {
            var response = await _atoClinicoService.GetAllAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        [AllowAnonymous] // Informação pública
        public async Task<IActionResult> GetAtoClinicoById(int id)
        {
            var response = await _atoClinicoService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem adicionar
        public async Task<IActionResult> AddAtoClinico([FromBody]
            CreateAtoClinicoDto createDto)
        {
            var response = await _atoClinicoService.AddAsync(createDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem atualizar
        public async Task<IActionResult> UpdateAtoClinico(int id, [FromBody]UpdateAtoClinicoDto updateDto)
        {
            var response = await _atoClinicoService.UpdateAsync(id, updateDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")] // Apenas administradores podem eliminar
        public async Task<IActionResult> DeleteAtoClinico(int id)
        {
            var response = await _atoClinicoService.DeleteAsync(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
