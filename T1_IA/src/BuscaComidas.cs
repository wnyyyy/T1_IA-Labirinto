using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class BuscaComidas
    {
        public Labirinto Labirinto { get; }
        public int LimiteMovimentos { get; }
        public int TamPopulacao { get; }
        public int ChanceMutacao { get; }
        public List<Agente> Populacao { get; private set; }
        private Random _rand { get; set; }

        public BuscaComidas(Labirinto labirinto, int coefTamanhoPopulacao, int chanceMutacao)
        {
            _rand = new Random();
            TamPopulacao = 300 * coefTamanhoPopulacao / 100;
            ChanceMutacao = chanceMutacao;
            Labirinto = labirinto;
            LimiteMovimentos = Labirinto.Dimensao * Labirinto.Dimensao;
            Populacao = _fillPopulacao();
        }

        private Agente _gerarCromossomo(int id)
        {
            Agente agente = new Agente(Labirinto, id);
            while (agente.Rota.Count < LimiteMovimentos)
            {
                List<TipoCaminho> direcoes = Labirinto.Celulas.GetValueOrDefault(agente.Coords)!.Caminhos.Select(x => x.Direcao).ToList();
                TipoCaminho direcao = direcoes[_rand.Next(direcoes.Count)];
                agente.Mover(direcao);                
            }

            return agente;
        }

        private List<Agente> _fillPopulacao()
        {
            List<Agente> populacao = new List<Agente>();
            for (int i = 1; i < TamPopulacao+1; i++)
            {
                populacao.Add(_gerarCromossomo(i));
            }

            return populacao;
        }
    }
}
