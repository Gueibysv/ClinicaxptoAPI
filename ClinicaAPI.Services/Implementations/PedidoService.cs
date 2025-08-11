using AutoMapper;
using ClinicaAPI.DTO.Pedido;
using ClinicaAPI.Model;
using ClinicaAPI.Shared.Repository;
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
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IAtoClinicoRepository _atoClinicoRepository;
        private readonly ISubsistemaSaudeRepository _subsistemaSaudeRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly UserManager<Utilizador> _userManager; // Para buscar  dados do utilizador registado
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public PedidoService(
        IPedidoRepository pedidoRepository,
        IAtoClinicoRepository atoClinicoRepository,
        ISubsistemaSaudeRepository subsistemaSaudeRepository,
        IProfissionalRepository profissionalRepository,
        UserManager<Utilizador> userManager,
        IMapper mapper,
        IEmailService emailService)
        {
            _pedidoRepository = pedidoRepository;
            _atoClinicoRepository = atoClinicoRepository;
            _subsistemaSaudeRepository = subsistemaSaudeRepository;
            _profissionalRepository = profissionalRepository;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<PedidoDto>>
        CreatePedidoAnonimoAsync(CreatePedidoAnonimoDto createDto)
        {
            // 1. Criar um novo Utilizador anônimo
            var anonymousUser = new Utilizador
            {
                UserName = Guid.NewGuid().ToString(), // Um username único para o usuário anônimo
                Email = createDto.EmailAnonimo, // Usar o email do pedido para o usuário anônimo
                NomeCompleto = createDto.NomeCompletoAnonimo, // Usar o nome completo do pedido
                PhoneNumber = createDto.TelemovelAnonimo, // Usar o telemóvel do pedido
                EstaRegistrado = false, // Marcar como não registrado
                TipoUtilizador = Model.TipoUtilizador.Anonimo // Definir tipo como Anônimo
            };

            var createResult = await _userManager.CreateAsync(anonymousUser);
            if (!createResult.Succeeded)
            {
                return new ServiceResponse<PedidoDto>("Falha ao criar utilizador anônimo.", false, createResult.Errors.Select(e => e.Description).ToList());
            }

            // Adicionar o utilizador à role 'Anonimo'
            if (!await _userManager.IsInRoleAsync(anonymousUser, Model.TipoUtilizador.Anonimo.ToString()))
            {
                await _userManager.AddToRoleAsync(anonymousUser, Model.TipoUtilizador.Anonimo.ToString());
            }

            var pedido = _mapper.Map<Pedido>(createDto);
            pedido.Estado = Model.Enums.EstadoPedido.Pedido;
            pedido.DataSolicitacao = DateTime.UtcNow;
            pedido.UtilizadorId = anonymousUser.Id; // Associar o ID do usuário anônimo ao pedido

            // Adicionar os atos clínicos associados
            foreach (var atoClinicoDto in createDto.PedidoAtoClinicos)
            {
                var atoClinico = await _atoClinicoRepository.GetByIdAsync(atoClinicoDto.AtoClinicoId);
                if (atoClinico == null)
                {
                    return new ServiceResponse<PedidoDto>("Ato Clínico não encontrado.", false);
                }
                var pedidoAtoClinico = _mapper.Map<PedidoAtoClinico>(atoClinicoDto);
                pedidoAtoClinico.AtoClinico = atoClinico;
                pedido.PedidoAtoClinicos.Add(pedidoAtoClinico);
            }

            await _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();

            var pedidoDto = _mapper.Map<PedidoDto>(pedido);
            return new ServiceResponse<PedidoDto>(pedidoDto, "Pedido anônimo criado com sucesso.");
        }
        public async Task<ServiceResponse<PedidoDto>>
        CreatePedidoRegistadoAsync(string utilizadorId, CreatePedidoRegistadoDto
        createDto)
        {
            var utilizador = await _userManager.FindByIdAsync(utilizadorId);
            if (utilizador == null)
            {
                return new ServiceResponse<PedidoDto>("Utilizador registado não encontrado.", false);
            }
            var pedido = _mapper.Map<Pedido>(createDto);
            pedido.UtilizadorId = utilizadorId;
            pedido.Utilizador = utilizador; // Associar o objeto Utilizador
            pedido.Estado = Model.Enums.EstadoPedido.Pedido;
            pedido.DataSolicitacao = DateTime.UtcNow;
            // Adicionar os atos clínicos associados
            foreach (var atoClinicoDto in createDto.PedidoAtoClinicos)
            {
                var atoClinico = await
                _atoClinicoRepository.GetByIdAsync(atoClinicoDto.AtoClinicoId);
                if (atoClinico == null)
                {
                    return new ServiceResponse<PedidoDto>("Ato Clínico não encontrado.", false);
                }
                var pedidoAtoClinico = _mapper.Map<PedidoAtoClinico>
                (atoClinicoDto);
                pedidoAtoClinico.AtoClinico = atoClinico;
                pedido.PedidoAtoClinicos.Add(pedidoAtoClinico);
            }
            await _pedidoRepository.AddAsync(pedido);
            await _pedidoRepository.SaveChangesAsync();
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);
            return new ServiceResponse<PedidoDto>(pedidoDto, "Pedido registado criado com sucesso.");
        }
        public async Task<ServiceResponse<bool>> UpdateEstadoPedidoAsync(int
        pedidoId, UpdateEstadoPedidoDto updateDto)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null)
            {
                return new ServiceResponse<bool>("Pedido não encontrado.",
                false);
            }
            // Regras de negócio para transição de estados
            if (updateDto.NovoEstado == Model.Enums.EstadoPedido.Agendado &&
            pedido.Estado != Model.Enums.EstadoPedido.Pedido)
            {
                return new ServiceResponse<bool>("Não é possível agendar um pedido que não esteja no estado 'Pedido'.", false);
            }
            if (updateDto.NovoEstado == Model.Enums.EstadoPedido.Realizado &&
            pedido.Estado != Model.Enums.EstadoPedido.Agendado)
            {
                return new ServiceResponse<bool>("Não é possível marcar como  realizado um pedido que não esteja no estado 'Agendado'.", false);
            }
            if (updateDto.NovoEstado == Model.Enums.EstadoPedido.Agendado)
            {
                string destinatario = pedido.EmailAnonimo ?? pedido.Utilizador?.Email;

                if (!string.IsNullOrWhiteSpace(destinatario))
                {
                    string assunto = "Confirmação de Marcação - Clínica XPTO";
                    string mensagem = $@"
            <h3>Pedido Agendado com Sucesso</h3>
            <p>Estimado(a) {(pedido.NomeCompletoAnonimo ?? pedido.Utilizador?.NomeCompleto)},</p>
            <p>O seu pedido foi agendado com sucesso para o período entre <strong>{pedido.DataInicioDesejada:dd/MM/yyyy}</strong> e <strong>{pedido.DataFimDesejada:dd/MM/yyyy}</strong>, no horário <strong>{pedido.HorarioSolicitado}</strong>.</p>
            <p>Obrigado por confiar na Clínica XPTO.</p>";

                    await _emailService.SendEmailAsync(destinatario, assunto, mensagem);
                }
            }
            pedido.Estado = updateDto.NovoEstado;
            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync();
            // TODO: Enviar email ao utente se o estado for Agendado
            return new ServiceResponse<bool>(true, "Estado do pedido atualizado com sucesso.");
        }
        public async Task<ServiceResponse<IEnumerable<PedidoDto>>>
        GetAllPedidosWithDetailsAsync()
        {
            var pedidos = await _pedidoRepository.GetAllAsync(); // Pode precisar de um método no repositório para incluir detalhes
            var pedidoDtos = _mapper.Map<IEnumerable<PedidoDto>>(pedidos);
            return new ServiceResponse<IEnumerable<PedidoDto>>(pedidoDtos);
        }
        public async Task<ServiceResponse<PedidoDto>>
        GetPedidoWithDetailsAsync(int pedidoId)
        {
            var pedido = await
            _pedidoRepository.GetPedidoWithDetailsAsync(pedidoId);
            if (pedido == null)
            {
                return new ServiceResponse<PedidoDto>("Pedido não encontrado.",
                false);
            }
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);
            return new ServiceResponse<PedidoDto>(pedidoDto);
        }
        public async Task<ServiceResponse<IEnumerable<PedidoDto>>>
        GetPedidosByUtilizadorIdAsync(string utilizadorId)
        {
            var pedidos = await
            _pedidoRepository.GetPedidosByUtilizadorIdAsync(utilizadorId);
            var pedidoDtos = _mapper.Map<IEnumerable<PedidoDto>>(pedidos);
            return new ServiceResponse<IEnumerable<PedidoDto>>(pedidoDtos);
        }
        // Implementação dos métodos genéricos (se PedidoService herdar de Service)
        public async Task<ServiceResponse<IEnumerable<PedidoDto>>>
            GetAllAsync()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            var pedidoDtos = _mapper.Map<IEnumerable<PedidoDto>>(pedidos);
            return new ServiceResponse<IEnumerable<PedidoDto>>(pedidoDtos);
        }
        public async Task<ServiceResponse<PedidoDto>> GetByIdAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
            {
                return new ServiceResponse<PedidoDto>("Pedido não encontrado.",
                false);
            }
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);
            return new ServiceResponse<PedidoDto>(pedidoDto);
        }
        public async Task<ServiceResponse<PedidoDto>>
        AddAsync(CreatePedidoRegistadoDto createDto)
        {
            // Este método não será usado diretamente, pois temos CreatePedidoAnonimoAsync e CreatePedidoRegistadoAsync
            // Pode ser removido ou adaptado se o IService for estritamente necessário aqui.
        return new ServiceResponse<PedidoDto>("Método não implementado diretamente.", false);
        }
        public async Task<ServiceResponse<PedidoDto>> UpdateAsync(int id,
        PedidoDto updateDto)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
            {
                return new ServiceResponse<PedidoDto>("Pedido não encontradob para atualização.", false);
            }
            _mapper.Map(updateDto, pedido);
            _pedidoRepository.Update(pedido);
            await _pedidoRepository.SaveChangesAsync();
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);
            return new ServiceResponse<PedidoDto>(pedidoDto, "Pedido atualizado com sucesso.");
        }
        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null)
            {
                return new ServiceResponse<bool>("Pedido não encontrado para exclusão.", false);
            }
            _pedidoRepository.Delete(pedido);
            await _pedidoRepository.SaveChangesAsync();
            return new ServiceResponse<bool>(true, "Pedido excluído com sucesso.");
        }
    }
}
