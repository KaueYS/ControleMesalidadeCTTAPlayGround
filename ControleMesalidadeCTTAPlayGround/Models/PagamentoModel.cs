using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ControleMesalidadeCTTAPlayGround.Models
{
    public class PagamentoModel
    {

        public int Id { get; set; }
        public int SocioId { get; set; }

        [Display(Name = "Nome do Socio")]
        public AssociadoModel Socio { get; set; }

        [Display(Name ="Data do pagamento")]
        public DateTime DataPagamento { get; set; }

        [Display(Name = "Dias Pagos")]
        public int DiasAdimplencia { get; set; }

        public decimal Valor { get; set; }

        





        public PagamentoModel()
        {
            Valor = 60;
            DiasAdimplencia = CalcularDiasAdimplencia(Valor);
        }

        public static int CalcularDiasAdimplencia(decimal valor)
        {
            return (int)(valor / 60m * 30);
        }

        public DateTime CalcularVencimento(DateTime? vencimentoAnterior)
        {
            if (vencimentoAnterior.HasValue && vencimentoAnterior > DataPagamento)
                return vencimentoAnterior.Value.AddDays(DiasAdimplencia);

            return DataPagamento.AddDays(DiasAdimplencia);
        }
    }
}
