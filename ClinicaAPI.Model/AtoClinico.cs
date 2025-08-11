using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Model
{
    public class AtoClinico
    {
        [Key]
        public int AtoClinicoId { get; set; }
        [Required]
        [StringLength(100)]
        public string Tipo { get; set; } // Ex: "Consulta", "Exame"
        [Required]
        [StringLength(255)]
        public string Nome { get; set; }
        [StringLength(1000)]
        public string? Descricao { get; set; }
        // Relacionamento
        public ICollection<PedidoAtoClinico> PedidoAtoClinicos { get; set; } = new List<PedidoAtoClinico>();
    }
}
