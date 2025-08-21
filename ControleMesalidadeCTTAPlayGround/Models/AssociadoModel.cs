using ControleMesalidadeCTTAPlayGround.Enum;
using System.ComponentModel.DataAnnotations;

namespace ControleMesalidadeCTTAPlayGround.Models
{
    public class AssociadoModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "RG ou CPF")]
        public string? Documento { get; set; } = string.Empty;

        [EmailAddress]

        public string? Email { get; set; } = string.Empty;

        public string? Telefone { get; set; } = string.Empty;

        [Display(Name = "Endereço")]
        public string? Endereco { get; set; } = string.Empty;




        [Display(Name = "Data Aniversário")]
        public DateTime DataAniversario { get; set; }


        public CategoriaEnum Categoria { get; set; }

        [Display(Name = "Equipamento Próprio")]
        public bool Equipamento { get; set; }


        [Display(Name = "Necessidades Especiais")]
        public bool Necessidade { get; set; }


        public bool Ativo { get; set; }


        public List<PagamentoModel>? Pagamentos { get; set; }


        public DateTime? ObterVencimentoAtual()
        {
            if (!Pagamentos.Any()) return null;

            var pagamentosOrdenados = Pagamentos.OrderBy(p => p.DataPagamento).ToList();
            DateTime? vencimento = null;

            foreach (var pagamento in pagamentosOrdenados)
            {
                vencimento = pagamento.CalcularVencimento(vencimento);
            }

            return vencimento;
        }

        public bool EstaAdimplente()
        {
            var vencimento = ObterVencimentoAtual();
            return vencimento.HasValue && vencimento.Value >= DateTime.Today;
        }

        public int DiasRestantes()
        {
            var vencimento = ObterVencimentoAtual();
            return vencimento.HasValue && vencimento >= DateTime.Today
                ? (vencimento.Value - DateTime.Today).Days
                : 0;
        }
    }
}

