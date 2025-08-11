using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Model
{
    public class Profissional
    {
        
            [Key]
            public int ProfissionalId { get; set; }
            [Required]
            [StringLength(255)]
            public string Nome { get; set; }
            [Required]
            [StringLength(100)]
            public string Especialidade { get; set; }
            [StringLength(20)]
            public string? Telefone { get; set; }
            [StringLength(255)]
            public string? Email { get; set; }
            // Relacionamento
            public ICollection<PedidoAtoClinico> PedidoAtoClinicos { get; set; } = new List<PedidoAtoClinico>();
        }






    }

