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
        public long TempoSelecao { get; private set; }
        public long TempoCrossover{ get; private set; }
        private Random _rand = new Random();
        private Stopwatch _stopwatch = new Stopwatch();
        private BuscaRota _buscaRota;

        public Crossover(List<int> aptidaoAcumulada, List<Agente> populacao, Labirinto labirinto)
        {
            _stopwatch.Restart();
            FirstParent = _selecionarPopulacao(aptidaoAcumulada, populacao);
            SecondParent = _selecionarPopulacao(aptidaoAcumulada, populacao);
            _stopwatch.Restart();
            TempoSelecao = _stopwatch.ElapsedTicks;
            TempoCrossover = 0;
            Labirinto = labirinto;
            _buscaRota = new BuscaRota(labirinto);

        }

        public Agente CriarFilho(int id, int numGeracoes)
        {
            _stopwatch.Restart();
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
            _stopwatch.Stop();
            TempoCrossover = _stopwatch.ElapsedTicks;

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
