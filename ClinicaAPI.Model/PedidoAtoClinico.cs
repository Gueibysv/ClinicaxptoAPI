using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Model
{
  

        public class PedidoAtoClinico
        {
            [Key]
            public int PedidoAtoClinicoId { get; set; }
            [Required]
            public int PedidoId { get; set; }
            [ForeignKey("PedidoId")]
            public Pedido Pedido { get; set; }
            [Required]
            public int AtoClinicoId { get; set; }
            [ForeignKey("AtoClinicoId")]
            public AtoClinico AtoClinico { get; set; }
            public int? SubsistemaSaudeId { get; set; }
            [ForeignKey("SubsistemaSaudeId")]
            public SubsistemaSaude? SubsistemaSaude { get; set; }
            public int? ProfissionalId { get; set; }
            [ForeignKey("ProfissionalId")]
            public Profissional? Profissional { get; set; }
        }

    }

