using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class BuscaRota
    {
        public Labirinto Labirinto { get; set; }
        public Dictionary<(int, int), Nodo> Nodos { get; set; }

        public BuscaRota(Labirinto labirinto)
        {
            Labirinto = labirinto;
            Nodos = _construir();
        }

        public List<(int, int)> Buscar((int, int) from, (int, int) to)
        {
            Nodo inicio = Nodos.GetValueOrDefault(from)!;
            Nodo fim = Nodos.GetValueOrDefault(to)!;

            inicio.SetDistancia(fim.X, fim.Y);

            List<Nodo> ativos = new List<Nodo>();
            List<Nodo> visitados = new List<Nodo>();
            ativos.Add(inicio);

            while (ativos.Count > 0)
            {
                Nodo atual = ativos.OrderBy(x => x.CustoDistancia).First();
                if (atual.X == fim.X && atual.Y == fim.Y)
                {
                    return _final(ativos);
                }

                visitados.Add(atual);
                ativos.Remove(atual);

                List<Nodo> vizinhos = _getCaminhos(atual, fim);
                foreach (Nodo nodo in vizinhos)
                {
                    if (visitados.Contains(nodo))
                        continue;

                    if (ativos.Contains(nodo))
                    {
                        Nodo existente = ativos.First(n => n.X == nodo.X && n.Y == nodo.Y);
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

            return _final(ativos);
        }

        private List<(int, int)> _final(List<Nodo> ativos)
        {
            List<(int, int)> ret = new List<(int, int)> ();

            return ret;
        }

        private List<Nodo> _getCaminhos(Nodo curr, Nodo target)
        {
            List<Nodo> lst = new List<Nodo>();
            Celula celCurr = Labirinto.Celulas.GetValueOrDefault((curr.X, curr.Y))!;
            foreach (Caminho possivel in celCurr.Caminhos)
            {
                Nodo vizinho = Nodos.GetValueOrDefault(possivel.Destino)!;
                vizinho.Custo = curr.Custo + 1;
                vizinho.Pai = curr;
                vizinho.SetDistancia(target.X, target.Y);
                lst.Add(vizinho);
            }

            return lst;
        }

        private Dictionary<(int, int), Nodo> _construir()
        {
            Dictionary<(int, int), Nodo> ret = new Dictionary<(int, int), Nodo>();
            IEnumerable<(int, int)> validos = Labirinto.Celulas.Where(x => x.Value.IsValido()).Select(x => x.Key);
            foreach ((int, int) c in validos)
            {
                ret.Add((c.Item1, c.Item2), new Nodo(c.Item1, c.Item2));
            }

            return ret;
        }
    }
}
