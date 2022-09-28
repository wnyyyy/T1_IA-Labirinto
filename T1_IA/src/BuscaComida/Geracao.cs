using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Geracao
    {
        public List<Agente> Populacao { get; }
        public int Numero { get; }
        public long TempoGerada { get; }

        public Geracao(List<Agente> populacao, int numero, long tempoGerada)
        {
            Populacao = populacao;
            Numero = numero;
            TempoGerada = tempoGerada;
        }

        public override string ToString()
        {
            int count = Math.Max(1, Populacao.Count / 10);
            StringBuilder sb = new StringBuilder();
            sb.Append("Geração ").AppendLine(Numero.ToString());
            sb.Append("Tempo de geração: ");
            sb.Append(String.Format("{0:0.##}", (double)TempoGerada / TimeSpan.TicksPerMillisecond));
            sb.AppendLine(" ms");
            sb.AppendLine();
            for (int i = 0; i < count; i++)
            {
                sb.AppendLine(Populacao[i].ToString());
            }
            return sb.ToString();
        }
    }
}
