using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using T1_IA.AStar;

namespace T1_IA
{
    internal class Crossover
    {
        public Agente FirstParent { get; private set; }
        public Agente SecondParent { get; private set; }
        public Labirinto Labirinto { get; private set; }
        private Random _rand = new Random();
        private BuscaRota _buscaRota;

        public Crossover(List<int> aptidaoAcumulada, List<Agente> populacao, Labirinto labirinto)
        {
            FirstParent = _selecionarPopulacao(aptidaoAcumulada, populacao);
            SecondParent = _selecionarPopulacao(aptidaoAcumulada, populacao);
            Labirinto = labirinto;
            _buscaRota = new BuscaRota(labirinto);
        }

        public Agente CriarFilho(int id, int numGeracoes)
        {
            Agente filho = new Agente(Labirinto, id, numGeracoes, this);
            int size = Math.Min(FirstParent.Rota.Count, SecondParent.Rota.Count);

            List<Caminho> rotaInicial = FirstParent.Rota.Take(size / 2).ToList();
            List<Caminho> rotaFinal = SecondParent.Rota.Skip(size / 2).ToList();
            (int, int) destInicio = rotaInicial[rotaInicial.Count - 1].Destino;
            (int, int) origFinal = rotaFinal[0].Origem;

            filho.Rota.AddRange(rotaInicial);
            if (origFinal != destInicio)
            {
                List<Caminho> rotaAux = _buscaRota.Buscar(destInicio, origFinal);
                filho.Rota.AddRange(rotaAux);
            }
            filho.Rota.AddRange(rotaFinal);

            return filho;
        }

        private Agente _selecionarPopulacao(List<int> aptidaoAcumulada, List<Agente> populacao)
        {
            int p = _rand.Next(aptidaoAcumulada.Last());
            int index = -1;
            for (int i = 0; i < aptidaoAcumulada.Count; i++)
            {
                if (aptidaoAcumulada[i] > p)
                {
                    index = i;
                    break;
                }
            }
            return populacao[index];
        }
    }
}
