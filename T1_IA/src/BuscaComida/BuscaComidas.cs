using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using T1_IA.AStar;

namespace T1_IA
{
    internal class BuscaComidas
    {
        public Labirinto Labirinto { get; }
        public int NumGeracoes => Geracoes.Count;
        public List<Agente> PopulacaoAtual => Geracoes.Last().Populacao;
        public List<Geracao> Geracoes { get; private set; }
        public Opcoes Opcoes { get; }
        public long TempoTotal => Geracoes.Sum(a => a.TempoGerada);
        private Random _rand { get; set; }
        private BuscaRota _buscaRota { get; }
        private Stopwatch _stopwatch = new();

        public BuscaComidas(Labirinto labirinto, int coefTamanhoPopulacao, int taxaMutacao, int agressividadeMutacao, int elitismo)
        {
            Geracoes = new List<Geracao>();
            Opcoes = new Opcoes(coefTamanhoPopulacao, taxaMutacao, agressividadeMutacao, elitismo, labirinto);
            _rand = new Random();
            Labirinto = labirinto;
            _buscaRota = new BuscaRota(labirinto);
            _stopwatch.Start();
            List<Agente> inicial = _fillPopulacao();
            _stopwatch.Stop();
            Geracoes.Add(new Geracao(inicial, 1, _stopwatch.ElapsedTicks));
        }

        public void NovaGeracao()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            List<Agente> novaPopulacao = new List<Agente>();
            List<Agente> oldPopulacao = PopulacaoAtual;
            _aplicarElitismo(novaPopulacao, oldPopulacao);
            _realizarCrossover(novaPopulacao, oldPopulacao);
            _stopwatch.Stop();
            novaPopulacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            Geracao novaGeracao = new Geracao(novaPopulacao, NumGeracoes + 1, _stopwatch.ElapsedTicks);
            Geracoes.Add(novaGeracao);
        }

        private Agente _gerarCromossomo(int id)
        {
            Agente agente = new Agente(Labirinto, id, 1);
            while (agente.Rota.Count < Opcoes.LimiteMovimentos)
            {
                List<TipoCaminho> direcoes = Labirinto.Celulas.GetValueOrDefault(agente.Coords)!.Caminhos.Select(x => x.Direcao).ToList();
                TipoCaminho direcao = direcoes[_rand.Next(direcoes.Count)];
                if (!agente.Mover(direcao))
                    break;
            }

            agente.RecalcularAptidao(Opcoes);
            return agente;
        }

        private List<Agente> _fillPopulacao()
        {
            List<Agente> populacao = new List<Agente>();
            for (int i = 1; i < Opcoes.TamPopulacao+1; i++)
            {
                populacao.Add(_gerarCromossomo(i));
            }

            populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            return populacao;
        }

        private void _aplicarElitismo(List<Agente> novaPopulacao, List<Agente> oldPopulacao)
        {
            novaPopulacao.AddRange(oldPopulacao.Take(Opcoes.TamPopulacao * Opcoes.Elitismo / 100));
        }

        private void _realizarCrossover(List<Agente> novaPopulacao, List<Agente> oldPopulacao)
        {
            int id = oldPopulacao.Select(x => x.Id).Max() + 1;
            List<Agente> best50 = oldPopulacao.Take(oldPopulacao.Count/2).ToList();
            List<int> aptidaoAcumulada = _getAptidaoAcumulada(best50);
            while (novaPopulacao.Count < Opcoes.TamPopulacao)
            {
                Crossover crossover = new Crossover(aptidaoAcumulada, best50, Labirinto);
                Agente filho = crossover.CriarFilho(id, NumGeracoes+1);
                filho.AplicarMutacao(Opcoes);
                filho.Rota = filho.Rota.Take(Opcoes.LimiteMovimentos).ToList();                
                filho.RecalcularComida();
                filho.RecalcularAptidao(Opcoes);
                //Console.WriteLine(Util.ValidaCaminho(filho.Rota));
                novaPopulacao.Add(filho);
                id++;
            }
        }

        private List<int> _getAptidaoAcumulada(List<Agente> populacao)
        {
            List<int> aptidaoAcumulada = new List<int>();
            int aux = 0;
            foreach (Agente a in populacao)
            {
                aux += (int)Math.Ceiling((1 - a.Aptidao) * 100);
                aptidaoAcumulada.Add(aux);
            }
            return aptidaoAcumulada;
        }
    }
}
