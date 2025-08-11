using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Model
{
    public class SubsistemaSaude
    {
        [Key]
        public int SubsistemaSaudeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [StringLength(500)]
        public string? Descricao { get; set; }
        // Relacionamento
        public ICollection<PedidoAtoClinico> PedidoAtoClinicos { get; set; } = new List<PedidoAtoClinico>();
    }



}

