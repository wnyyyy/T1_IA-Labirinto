using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using T1_IA.AStar;

namespace T1_IA
{
    internal class Mutacao
    {
        public bool Reduz { get; private set; }
        public bool Explora { get; private set; }
        public long TempoReducao { get; private set; }
        public long TempoExploracao { get; private set; }
        public long TempoTotal => TempoReducao == -1 ? 
            (TempoExploracao == -1 ? 0 : TempoExploracao) : (TempoExploracao == -1 ? TempoReducao : TempoExploracao + TempoExploracao);
        public int Distancia { get; private set; }
        public Labirinto Labirinto;
        private BuscaRota _buscaRota;
        private Stopwatch _stopwatch = new Stopwatch();
        private Random _rand = new Random();

        public Mutacao(bool reduzir, bool explorar, Agente agente, Opcoes opcoes, Labirinto labirinto)
        {
            Labirinto = labirinto;
            _buscaRota = new BuscaRota(labirinto);
            Distancia = opcoes.Dimensao * opcoes.TaxaMutacao / 50;
            TempoReducao = -1;
            TempoExploracao = -1;
            Reduz = reduzir;
            Explora = explorar;
            if (reduzir)
                _reduzTamanho(agente);
            if (explorar)
                _explorar(agente);
        }

        private void _reduzTamanho(Agente agente)
        {
            _stopwatch.Restart();
            int dist = Distancia;
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
            _stopwatch.Stop();
            TempoReducao = _stopwatch.ElapsedTicks;
            //Console.WriteLine(Util.ValidaCaminho(novaRota));
        }

        private void _explorar(Agente agente)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Agente ").Append(agente.Id).Append("| ");
            Stopwatch debug = new Stopwatch();
            _stopwatch.Restart();

            int dist = Distancia;
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
            (int, int) firstGoal = agente.Rota[indexGoal].Destino;
            (int, int) goal = firstGoal;
            List<Caminho> toGoal = new List<Caminho>();

            debug.Restart();
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
            debug.Stop();
            toGoal.Reverse();

            List<Caminho> rota = _buscaRota.Buscar(pivot, goal);
            IEnumerable<Caminho> inicio = agente.Rota.Take(indexPivot + 1);
            IEnumerable<Caminho> fim = agente.Rota.Skip(indexGoal + 1);
            List<Caminho> novaRota = inicio.Concat(rota).Concat(toGoal).Concat(fim).ToList();
            //Console.WriteLine(Util.ValidaCaminho(novaRota));
            agente.Rota = novaRota;
            _stopwatch.Stop();
            TempoExploracao = _stopwatch.ElapsedTicks;
            //sb.Append(" T. Loop: ").Append(debug.ElapsedTicks).Append(" | T. Total: ").Append(TempoExploracao).Append(" | Pivot: ").Append(indexPivot).Append(" ").Append(pivot).Append(" | Goal: ").Append(indexGoal).Append(" ").Append(goal);
            //Console.WriteLine(sb.ToString());
        }
    }
}
