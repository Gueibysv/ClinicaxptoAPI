using ClinicaAPI.DTO.Apoio;
using ClinicaAPI.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaAPI.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class ProfissionalController : ControllerBase
        {
            private readonly IProfissionalService _profissionalService;
            public ProfissionalController(IProfissionalService profissionalService)
            {
                _profissionalService = profissionalService;
            }
            [HttpGet]
            [AllowAnonymous] // Informação pública para formulários de marcação
            public async Task<IActionResult> GetAllProfissionais()
            {
                var response = await _profissionalService.GetAllAsync();
                return Ok(response);
            }
            [HttpGet("{id}")]
            [AllowAnonymous] // Informação pública
            public async Task<IActionResult> GetProfissionalById(int id)
            {
                var response = await _profissionalService.GetByIdAsync(id);
                if (!response.Success)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            [HttpPost]
            [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem adicionar
        public async Task<IActionResult> AddProfissional([FromBody]
                CreateProfissionalDto createDto)
            {
                var response = await _profissionalService.AddAsync(createDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            [HttpPut("{id}")]
            [Authorize(Roles = "Administrador,Administrativo")] // Apenas administradores e administrativos podem atualizar
        public async Task<IActionResult> UpdateProfissional(int id, [FromBody]
            UpdateProfissionalDto updateDto)
            {
                var response = await _profissionalService.UpdateAsync(id,
                updateDto);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            [HttpDelete("{id}")]
            [Authorize(Roles = "Administrador")] // Apenas administradores podem eliminar
        public async Task<IActionResult> DeleteProfissional(int id)
            {
                var response = await _profissionalService.DeleteAsync(id);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
        }
   }

