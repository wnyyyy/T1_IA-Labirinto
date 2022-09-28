using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA.AStar
{
    internal class BuscaRota
    {
        public Labirinto Labirinto { get; set; }

        public BuscaRota(Labirinto labirinto)
        {
            Labirinto = labirinto;
        }

        public List<Caminho> Buscar((int, int) from, (int, int) to)
        {
            Nodo inicio = new Nodo(from.Item1, from.Item2);
            Nodo fim = new Nodo(to.Item1, to.Item2);

            inicio.SetDistancia(fim.X, fim.Y);

            List<Nodo> ativos = new List<Nodo>();
            List<Nodo> visitados = new List<Nodo>();
            ativos.Add(inicio);

            while (ativos.Count > 0)
            {
                Nodo atual = ativos.OrderBy(x => x.CustoDistancia).First();
                if (atual.X == fim.X && atual.Y == fim.Y)
                {
                    return _final(atual);
                }

                visitados.Add(atual);
                ativos.Remove(atual);

                List<Nodo> vizinhos = _getCaminhos(atual, fim);
                foreach (Nodo nodo in vizinhos)
                {
                    if (visitados.Contains(nodo))
                        continue;

                    Nodo? existente = ativos.Where(x => x.Equals(nodo)).FirstOrDefault();
                    if (existente is not null)
                    {
                        if (existente.CustoDistancia > nodo.CustoDistancia)
                        {
                            ativos.Remove(existente);
                            ativos.Add(nodo);
                        }
                    }
                    else
                    {
                        ativos.Add(nodo);
                    }
                }
            }

            return new List<Caminho>();
        }

        public List<Caminho> Buscar(List<(int,int)> coords, (int,int) start)
        {
            List<Caminho> caminho = new List<Caminho>();
            (int, int) from = start;
            foreach ((int, int) x in coords)
            {
                caminho.AddRange(Buscar(from, x));
                from = x;
            }
            return caminho;
        }

        private List<Caminho> _final(Nodo fim)
        {
            List<Caminho> ret = new List<Caminho>();
            Nodo curr = fim;
            while (curr.Pai != null)
            {
                (int, int) to = (curr.X, curr.Y);
                (int, int) from = (curr.Pai.X, curr.Pai.Y);
                TipoCaminho? direcao = Util.GetDirecaoFromCoords(from, to);
                Caminho caminho = new Caminho(to, from, (TipoCaminho) direcao!);
                ret.Add(caminho);
                curr = curr.Pai;
            }
            ret.Reverse();

            return ret;
        }

        private List<Nodo> _getCaminhos(Nodo curr, Nodo target)
        {
            List<Nodo> lst = new List<Nodo>();
            Celula celCurr = Labirinto.Celulas.GetValueOrDefault((curr.X, curr.Y))!;
            foreach (Caminho possivel in celCurr.Caminhos)
            {
                Nodo vizinho = new Nodo(possivel.Destino.Item1, possivel.Destino.Item2);
                vizinho.Custo = curr.Custo + 1;
                vizinho.Pai = curr;
                vizinho.SetDistancia(target.X, target.Y);
                lst.Add(vizinho);
            }

            return lst;
        }
    }
}
