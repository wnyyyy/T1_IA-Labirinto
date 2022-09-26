using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1_IA.AStar;

namespace T1_IA
{
    internal class BuscaComidas
    {
        public Labirinto Labirinto { get; }
        public List<Agente> Populacao { get; private set; }
        public int Geracao { get; private set; }
        public Opcoes Opcoes { get; }
        private Random _rand { get; set; }
        private BuscaRota _buscaRota { get; }

        public BuscaComidas(Labirinto labirinto, int coefTamanhoPopulacao, int taxaMutacao, int elitismo)
        {
            Opcoes = new Opcoes(coefTamanhoPopulacao, taxaMutacao, elitismo, labirinto);
            _rand = new Random();
            Labirinto = labirinto;
            Geracao = 1;
            _buscaRota = new BuscaRota(labirinto);
            Populacao = _fillPopulacao();
        }

        public void NovaGeracao()
        {
            List<Agente> novaPopulacao = new List<Agente>();
            Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            _aplicarElitismo(novaPopulacao);
            _realizarCrossover(novaPopulacao);
            Populacao = novaPopulacao;
            Geracao++;
        }

        private Agente _gerarCromossomo(int id)
        {
            Agente agente = new Agente(Labirinto, id, Geracao);
            while (agente.Rota.Count < Opcoes.LimiteMovimentos)
            {
                List<TipoCaminho> direcoes = Labirinto.Celulas.GetValueOrDefault(agente.Coords)!.Caminhos.Select(x => x.Direcao).ToList();
                TipoCaminho direcao = direcoes[_rand.Next(direcoes.Count)];
                if (!agente.Mover(direcao))
                    break;
            }

            _atualizarAptidao(agente);
            return agente;
        }

        private List<Agente> _fillPopulacao()
        {
            List<Agente> populacao = new List<Agente>();
            for (int i = 1; i < Opcoes.TamPopulacao+1; i++)
            {
                populacao.Add(_gerarCromossomo(i));
            }

            return populacao;
        }

        private void _aplicarElitismo(List<Agente> novaPopulacao)
        {            
            novaPopulacao.AddRange(Populacao.Take(Opcoes.TamPopulacao * Opcoes.Elitismo / 100));
        }

        private void _realizarCrossover(List<Agente> novaPopulacao)
        {
            int id = Populacao.Select(x => x.Id).Max() + 1;
            List<Agente> best50 = Populacao.Take(Populacao.Count/2).ToList();
            List<int> aptidaoAcumulada = _getAptidaoAcumulada(best50);
            while (novaPopulacao.Count < Opcoes.TamPopulacao)
            {
                Agente a1 = _selecionarPopulacao(aptidaoAcumulada, best50);
                Agente a2 = _selecionarPopulacao(aptidaoAcumulada, best50);
                Agente filho = _criarFilho(a1, a2, id);
                int numMutacoes = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (_rand.Next(1, 101) <= Opcoes.TaxaMutacao)
                        numMutacoes++;
                }
                for (int i = 0; i < numMutacoes; i++)
                {
                    _aplicarMutacao(filho);
                }
                filho.Rota = filho.Rota.Take(Opcoes.LimiteMovimentos).ToList();
                filho.RecalcularComida();
                _atualizarAptidao(filho);
                //Console.WriteLine(Util.ValidaCaminho(filho.Rota));
                novaPopulacao.Add(filho);
                id++;
            }
        }

        private Agente _criarFilho(Agente a1, Agente a2, int id)
        {        
            Agente filho = new Agente(Labirinto, id, Geracao);
            int size = Math.Min(a1.Rota.Count, a2.Rota.Count);

            List<Caminho> rotaInicial = a1.Rota.Take(size/2).ToList();
            List<Caminho> rotaFinal = a2.Rota.Skip(size/2).ToList();
            (int, int) destInicio = rotaInicial[rotaInicial.Count-1].Destino;
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

        private void _aplicarMutacao(Agente agente)
        {
            int dist = Labirinto.Dimensao * Opcoes.TaxaMutacao / 50;
            int chanceTam = 50 * Opcoes.TaxaMutacao / 25;
            int chanceMut = 25 * Opcoes.TaxaMutacao / 25;

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
            IEnumerable<Caminho> inicio = agente.Rota.Take(indexPivot+1);
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
            if (_rand.Next(1,101) > 500)
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

        private void _atualizarAptidao(Agente agente)
        {
            int qtdRepetidos = (agente.Rota.Count - agente.Rota.GroupBy(x => x.Destino).Count());
            int ptsColeta = agente.ComidasColetadas.Count * (Opcoes.MovePts + 1);
            int ptsUnicidade = (Opcoes.LimiteMovimentos - qtdRepetidos) * 10;
            int ptsTamanho = Opcoes.LimiteMovimentos - agente.Rota.Count;
            int ptsTotal = ptsColeta + ptsTamanho + ptsUnicidade;
            agente.Aptidao = 1.0 - ((double)ptsTotal / Opcoes.CeilingPts);

            //agente.Aptidao = ((1 - (double)agente.ComidasColetadas.Count / Labirinto.NumComidas) + (double)qtdRepetidos / LimiteMovimentos) / 2;
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
