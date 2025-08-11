using ClinicaAPI.DTO.Auth;
using ClinicaAPI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterDto
        registerDto);
        Task<ServiceResponse<bool>> ChangePasswordAsync(string userId,
        ChangePasswordDto changePasswordDto);
        Task<ServiceResponse<UserDto>> GetUserByIdAsync(string userId);
        Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ServiceResponse<bool>> AssignRoleAsync(AssignRoleDto
        assignRoleDto);
        Task<ServiceResponse<bool>> RemoveRoleAsync(AssignRoleDto
        removeRoleDto);
        Task<ServiceResponse<bool>> DeleteUserAsync(string userId);
    }
}
