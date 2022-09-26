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
    }
}
