using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicaAPI.Model.Enums;

namespace ClinicaAPI.Model
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }
        // Dados do utente anônimo (se aplicável)
        [StringLength(255)]
        public string? NomeCompletoAnonimo { get; set; }
        [StringLength(255)]
        public string? EmailAnonimo { get; set; }
        [StringLength(20)]
        public string? TelemovelAnonimo { get; set; }
        // Dados do utente registado (se aplicável)
        public string? UtilizadorId { get; set; } // Foreign Key para Utilizador
        [ForeignKey("UtilizadorId")]
        public Utilizador? Utilizador { get; set; }
        [Required]
        public DateTime DataSolicitacao { get; set; }
        public DateTime DataInicioDesejada { get; set; }
        public DateTime DataFimDesejada { get; set; }
        [StringLength(100)]
        public string? HorarioSolicitado { get; set; } // Ex: "Manhã", "Tarde","Qualquer"
        [StringLength(1000)]
        public string? ObservacoesAdicionais { get; set; }
        [Required]
        public EstadoPedido Estado { get; set; } = EstadoPedido.Pedido;
        // Relacionamento com Atos Clínicos do Pedido
        public ICollection<PedidoAtoClinico> PedidoAtoClinicos { get; set; } = new List<PedidoAtoClinico>();
    }
}
