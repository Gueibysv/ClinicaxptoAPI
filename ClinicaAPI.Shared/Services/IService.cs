using ClinicaAPI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IService<TEntity, TEntityDto, TCreateDto, TUpdateDto>
    {
        Task<ServiceResponse<IEnumerable<TEntityDto>>> GetAllAsync();
        Task<ServiceResponse<TEntityDto>> GetByIdAsync(int id);
        Task<ServiceResponse<TEntityDto>> AddAsync(TCreateDto createDto);
        Task<ServiceResponse<TEntityDto>> UpdateAsync(int id, TUpdateDto
        updateDto);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
