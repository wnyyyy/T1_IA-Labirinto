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
        public int LimiteMovimentos { get; }
        public int TamPopulacao { get; }
        public int TaxaMutacao { get; }
        public List<Agente> Populacao { get; private set; }
        public int Elitismo { get; }
        public int Geracao { get; private set; }
        private Random _rand { get; set; }
        private BuscaRota _buscaRota { get; }
        private int _ceiling { get; }
        private int _movePts { get; }

        public BuscaComidas(Labirinto labirinto, int coefTamanhoPopulacao, int taxaMutacao, int elitismo)
        {            
            _rand = new Random();
            Elitismo = elitismo;
            TamPopulacao = 300 * coefTamanhoPopulacao / 100;
            TaxaMutacao = taxaMutacao;
            Labirinto = labirinto;
            LimiteMovimentos = Labirinto.Dimensao * Labirinto.Dimensao - labirinto.NumParedes;            
            Geracao = 1;
            _movePts = LimiteMovimentos + LimiteMovimentos * 10;
            _ceiling = Labirinto.NumComidas * _movePts + Labirinto.NumComidas + _movePts;
            _buscaRota = new BuscaRota(labirinto);
            Populacao = _fillPopulacao();
        }

        public void NovaGeracao()
        {
            List<Agente> novaPopulacao = new List<Agente>();
            _aplicarElitismo(novaPopulacao);
            _realizarCrossover(novaPopulacao);
            Populacao = novaPopulacao;
            Geracao++;
        }

        private Agente _gerarCromossomo(int id)
        {
            Agente agente = new Agente(Labirinto, id, Geracao);
            while (agente.Rota.Count < LimiteMovimentos)
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
            for (int i = 1; i < TamPopulacao+1; i++)
            {
                populacao.Add(_gerarCromossomo(i));
            }

            return populacao;
        }

        private void _aplicarElitismo(List<Agente> novaPopulacao)
        {
            Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            novaPopulacao.AddRange(Populacao.Take(TamPopulacao * Elitismo / 100));
        }

        private void _realizarCrossover(List<Agente> novaPopulacao)
        {
            List<int> aptidaoAcumulada = _getAptidaoAcumulada();
            int id = Populacao.Select(x => x.Id).Max() + 1;
            while (novaPopulacao.Count < TamPopulacao)
            {
                Agente a1 = _selecionarPopulacao(aptidaoAcumulada);
                Agente a2 = _selecionarPopulacao(aptidaoAcumulada);
                Agente filho = _criarFilho(a1, a2, id);
                int numMutacoes = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (_rand.Next(1, 101) <= TaxaMutacao)
                        numMutacoes++;
                }
                for (int i = 0; i < numMutacoes; i++)
                {
                    _aplicarMutacao(filho);
                }
                filho.Rota = filho.Rota.Take(LimiteMovimentos).ToList();
                filho.RecalcularComida();
                _atualizarAptidao(filho);
                novaPopulacao.Add(filho);
                id++;
            }
        }

        private Agente _criarFilho(Agente a1, Agente a2, int id)
        {
            List<(int, int)> pontosComum = a1.Rota
                .Where(x => a2.Rota
                .Any(y => y.Destino == x.Destino))
                .Select(x => x.Destino).ToList();

            Agente filho = new Agente(Labirinto, id, Geracao);
            (int, int) curr = filho.Coords;

            if (pontosComum.Count > 0)
            {
                bool aux = true;
                while (pontosComum.Count > 0 && filho.Rota.Count < LimiteMovimentos)
                {
                    if (aux)
                    {
                        filho.Rota.AddRange(a1.CaminhoFromToCoords(curr, pontosComum[0]));
                        aux = false;
                    }
                    else
                    {
                        filho.Rota.AddRange(a2.CaminhoFromToCoords(curr, pontosComum[0]));
                        aux = true;
                    }
                    curr = pontosComum[0];
                    pontosComum.RemoveAt(0);
                }
                if (aux) 
                    filho.Rota.AddRange(a1.CaminhoFromToCoords(curr, a1.Rota.Last().Destino));
                else
                    filho.Rota.AddRange(a2.CaminhoFromToCoords(curr, a2.Rota.Last().Destino));
                if (filho.Rota.Count > LimiteMovimentos)
                    filho.Rota.RemoveRange(LimiteMovimentos, filho.Rota.Count - LimiteMovimentos);
            }
            else
            {
                //_buscarPontoComum()
                return a1;
            }
            return filho;
        }

        private void _aplicarMutacao(Agente agente)
        {
            int dist = _rand.Next(1, Math.Min(Labirinto.Dimensao, TaxaMutacao * Labirinto.Dimensao / 70 + 1));
            int s = agente.Rota.Count - 1;
            int[] indexSecoes = new int[4] {s/4, s/2, s*3/4, s};
            int secao = _rand.Next(0, indexSecoes.Length);
            //int start = secao == 0 ? 0 : indexSecoes[secao - 1];
            //int end = indexSecoes[secao];
            int start = 0;
            int end = s;
            int i1 = _rand.Next(start, end);
            int i2 = _rand.Next(start, end);
            int indexPivot;
            int indexGoal;
            if (i1 <= i2)
            {
                indexPivot = i1;
                indexGoal = i2;
            }
            else
            {
                indexPivot = i2;
                indexGoal = i1;
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
            IEnumerable<Caminho> inicio = agente.Rota.Take(indexPivot+1);
            IEnumerable<Caminho> fim = agente.Rota.Skip(indexGoal+1);
            List<Caminho> novaRota = inicio.Concat(rota).Concat(toGoal).Concat(fim).ToList();
            agente.Rota = novaRota;            
        }

        private void _atualizarAptidao(Agente agente)
        {
            //_movePts = LimiteMovimentos + LimiteMovimentos * 10;
            //_ceiling = Labirinto.NumComidas * _movePts + Labirinto.NumComidas + _movePts;
            int qtdRepetidos = agente.Rota.GroupBy(x => x.Destino).Count();
            int ptsColeta = agente.ComidasColetadas.Count * (_movePts + 1);
            int ptsUnicidade = (LimiteMovimentos - qtdRepetidos) * 10;
            int ptsTamanho = LimiteMovimentos - agente.Rota.Count;
            int ptsTotal = ptsColeta + ptsTamanho + ptsUnicidade;
            agente.Aptidao = 1.0 - ((double)ptsTotal / _ceiling);
        }

        private Agente _selecionarPopulacao(List<int> aptidaoAcumulada)
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
            return Populacao[index];
        }

        private List<int> _getAptidaoAcumulada()
        {
            List<int> aptidaoAcumulada = new List<int>();
            int aux = 0;
            foreach (Agente a in Populacao)
            {
                aux += (int)Math.Ceiling((1 - a.Aptidao) * 100);
                aptidaoAcumulada.Add(aux);
            }
            return aptidaoAcumulada;
        }
    }
}
