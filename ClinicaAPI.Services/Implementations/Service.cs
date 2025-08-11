using AutoMapper;
using ClinicaAPI.Shared.Repository;
using ClinicaAPI.Shared.Services;
using ClinicaAPI.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Services.Implementations
{
    public class Service<TEntity, TEntityDto, TCreateDto, TUpdateDto> : IService<TEntity, TEntityDto, TCreateDto, TUpdateDto>
            where TEntity : class
            where TEntityDto : class
            where TCreateDto : class
            where TUpdateDto : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        public Service(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<TEntityDto>>>
        GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var entityDtos = _mapper.Map<IEnumerable<TEntityDto>>(entities);
            return new ServiceResponse<IEnumerable<TEntityDto>>(entityDtos);
        }
        public async Task<ServiceResponse<TEntityDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ServiceResponse<TEntityDto>("Entidade não encontrada.", false);
            }
            var entityDto = _mapper.Map<TEntityDto>(entity);
            return new ServiceResponse<TEntityDto>(entityDto);
        }
        public async Task<ServiceResponse<TEntityDto>> AddAsync(TCreateDto
        createDto)
        {
            var entity = _mapper.Map<TEntity>(createDto);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            var entityDto = _mapper.Map<TEntityDto>(entity);
            return new ServiceResponse<TEntityDto>(entityDto, "Entidade criada com sucesso.");
        }
        public async Task<ServiceResponse<TEntityDto>> UpdateAsync(int id,
        TUpdateDto updateDto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ServiceResponse<TEntityDto>("Entidade não encontrada para atualização.", false);
            }
            _mapper.Map(updateDto, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
            var entityDto = _mapper.Map<TEntityDto>(entity);
            return new ServiceResponse<TEntityDto>(entityDto, "Entidade atualizada com sucesso.");
        }
        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ServiceResponse<bool>("Entidade não encontrada para exclusão.", false);
            }
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
            return new ServiceResponse<bool>(true, "Entidade excluída com sucesso.");
        }
    }
}
