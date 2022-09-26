using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace T1_IA
{
    internal class Crossover
    {
        public Agente FirstParent { get; private set; }
        public Agente SecondParent { get; private set; }
        public Agente Child { get; private set; }        
        public long MilisecondsSelect { get; private set; }
        public long MilisecondsCriacao { get; private set; }
        public long MilisecondsTotal { get; private set; }
        private Opcoes Opcoes { get; }
        private Stopwatch _stopwatch = new Stopwatch();
        private Stopwatch _stopwatchAux = new Stopwatch();
        private Random _rand = new Random();

        public Crossover(List<Agente> populacao, int[] aptidaoAcumulada, int id, Opcoes opcoes)
        {
            Opcoes = opcoes;
            _stopwatch.Start();
            _stopwatchAux.Start();
            FirstParent = _selecionarPopulacao(aptidaoAcumulada, populacao);
            SecondParent = _selecionarPopulacao(aptidaoAcumulada, populacao);
            _stopwatchAux.Stop();
            MilisecondsSelect = _stopwatchAux.ElapsedMilliseconds;
            _stopwatchAux.Reset();
            _stopwatchAux.Start();
            Child = _criarFilho(FirstParent, SecondParent, id, this);
            MilisecondsCriacao = _stopwatchAux.ElapsedMilliseconds;

            for (int i = 0; i < 3; i++)
            {
                if (_rand.Next(1, 101) <= Opcoes.TaxaMutacao)
                {
                    Mutacao mutacao = _aplicarMutacao(Child);
                    Child.Mutacoes.Add(mutacao);
                }
                Child.Rota = Child.Rota.Take(Opcoes.LimiteMovimentos).ToList();
                Child.RecalcularComida();
                Child.RecalcularAptidao(opcoes);
                //Console.WriteLine(Util.ValidaCaminho(Child.Rota));
            }
        }

        private Agente _selecionarPopulacao(int[] aptidaoAcumulada, List<Agente> populacao)
        {
            int p = _rand.Next(aptidaoAcumulada.Last());
            int index = -1;
            for (int i = 0; i < aptidaoAcumulada.Length; i++)
            {
                if (aptidaoAcumulada[i] > p)
                {
                    index = i;
                    break;
                }
            }
            return populacao[index];
        }

        private Agente _criarFilho(Agente a1, Agente a2, int id)
        {
            Agente filho = new Agente(Labirinto, id, Geracao);
            int size = Math.Min(a1.Rota.Count, a2.Rota.Count);

            List<Caminho> rotaInicial = a1.Rota.Take(size / 2).ToList();
            List<Caminho> rotaFinal = a2.Rota.Skip(size / 2).ToList();
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

        private Mutacao _aplicarMutacao(Agente agente)
        {
            int dist = Labirinto.Dimensao * TaxaMutacao / 50;
            int chanceTam = 50 * TaxaMutacao / 25;
            int chanceMut = 25 * TaxaMutacao / 25;

            for (int i = 0; i < 4; i++)
            {
                int rand = _rand.Next(1, 101);
                if (rand <= chanceTam)
                {
                    _reduzTamanho(agente, dist);
                    if (rand <= chanceMut)
                        _explorar(agente, dist);
                }
            }
        }

        private void _reduzTamanho(Agente agente, int dist)
        {
            int indexPivot = _rand.Next(1, agente.Rota.Count);
            int indexGoal = agente.Rota.Count - 1;
            if (indexPivot + dist < agente.Rota.Count)
            {
                indexGoal = indexPivot + dist;
            }
            else if (indexPivot - dist > 0)
            {
                indexGoal = indexPivot;
                indexPivot = indexGoal - dist;
            }
            else
            {
                indexPivot = 0;
            }

            (int, int) pivot = agente.Rota[indexPivot].Destino;
            (int, int) goal = agente.Rota[indexGoal].Origem;
            IEnumerable<Caminho> inicio = agente.Rota.Take(indexPivot + 1);
            IEnumerable<Caminho> fim = agente.Rota.Skip(indexGoal);
            List<Caminho> rotaAux = new List<Caminho>();
            if (pivot != goal)
            {
                rotaAux = _buscaRota.Buscar(pivot, goal);
            }
            List<Caminho> novaRota = inicio.Concat(rotaAux).Concat(fim).ToList();
            agente.Rota = novaRota;
            //Console.WriteLine(Util.ValidaCaminho(novaRota));
        }

        private void _explorar(Agente agente, int dist)
        {
            int indexPivot = _rand.Next(1, agente.Rota.Count);
            int indexGoal = agente.Rota.Count - 1;
            if (indexPivot + dist < agente.Rota.Count)
            {
                indexGoal = indexPivot + dist;
            }
            else if (indexPivot - dist > 0)
            {
                indexGoal = indexPivot;
                indexPivot = indexGoal - dist;
            }
            else
            {
                indexPivot = 0;
            }
            if (_rand.Next(1, 101) > 500)
            {
                indexGoal = agente.Rota.Count - 1;
                indexPivot = indexGoal - dist < 0 ? 0 : indexGoal - dist;
            }

            (int, int) pivot = agente.Rota[indexPivot].Destino;
            (int, int) firstGoal = agente.Rota[indexGoal].Destino;
            (int, int) goal = firstGoal;
            List<Caminho> toGoal = new List<Caminho>();

            for (int i = 0; i < dist; i++)
            {
                (int, int) prev = goal;
                Celula celGoal = Labirinto.Celulas.GetValueOrDefault(goal)!;
                List<(int, int)> vizinhos = celGoal.Caminhos.Select(x => x.Destino).ToList();
                List<(int, int)> vizNaoVisitados = vizinhos.Where(x => !agente.Rota.Select(y => y.Destino).Contains(x)).ToList();
                vizNaoVisitados.Remove(Labirinto.Entrada);
                if (vizNaoVisitados.Count > 0)
                    goal = vizNaoVisitados[_rand.Next(vizNaoVisitados.Count)];
                else
                    goal = vizinhos[_rand.Next(vizinhos.Count)];
                TipoCaminho? direcao = Util.GetDirecaoFromCoords(goal, prev);
                Caminho caminho = new Caminho(prev, goal, (TipoCaminho)direcao!);
                toGoal.Add(caminho);
            }
            toGoal.Reverse();

            List<Caminho> rota = _buscaRota.Buscar(pivot, goal);
            IEnumerable<Caminho> inicio = agente.Rota.Take(indexPivot + 1);
            IEnumerable<Caminho> fim = agente.Rota.Skip(indexGoal + 1);
            List<Caminho> novaRota = inicio.Concat(rota).Concat(toGoal).Concat(fim).ToList();
            //Console.WriteLine(Util.ValidaCaminho(novaRota));
            agente.Rota = novaRota;
        }
    }
}
