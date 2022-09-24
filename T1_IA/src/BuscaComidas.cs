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
        public int Elitismo { get; }
        private Random _rand { get; set; }

        public BuscaComidas(Labirinto labirinto, int coefTamanhoPopulacao, int chanceMutacao, int elitismo)
        {
            _rand = new Random();
            Elitismo = elitismo;
            TamPopulacao = 300 * coefTamanhoPopulacao / 100;
            ChanceMutacao = chanceMutacao;
            Labirinto = labirinto;
            LimiteMovimentos = Labirinto.Dimensao * Labirinto.Dimensao - labirinto.NumParedes;
            Populacao = _fillPopulacao();
            NovaGeracao();
        }

        public void NovaGeracao()
        {
            List<Agente> novaPopulacao = new List<Agente>();
            _aplicarElitismo(novaPopulacao);
            _realizarCrossover(novaPopulacao);
            Populacao = novaPopulacao;
        }

        private Agente _gerarCromossomo(int id)
        {
            Agente agente = new Agente(Labirinto, id);
            while (agente.Rota.Count < LimiteMovimentos)
            {
                List<TipoCaminho> direcoes = Labirinto.Celulas.GetValueOrDefault(agente.Coords)!.Caminhos.Select(x => x.Direcao).ToList();
                TipoCaminho direcao = direcoes[_rand.Next(direcoes.Count)];
                if (!agente.Mover(direcao))
                    break;
            }

            agente.AtualizarAptidao();
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

            Agente filho = new Agente(Labirinto, id);
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
                filho.RecalcularComida();
                filho.AtualizarAptidao();
            }
            else
            {
                //_buscarPontoComum()
                return a1;
            }
            return filho;
        }

        private Agente _selecionarPopulacao(List<int> aptidaoAcumulada)
        {
            double rand = _rand.NextDouble();
            int p = (int)(rand * aptidaoAcumulada.Last());
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
                aux += (int)Math.Ceiling(a.Aptidao * 1000);
                aptidaoAcumulada.Add(aux);
            }
            return aptidaoAcumulada;
        }
    }
}
