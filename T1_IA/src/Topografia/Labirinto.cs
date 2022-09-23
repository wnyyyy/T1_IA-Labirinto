using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Labirinto
    {
        public Dictionary<(int, int), Celula> Celulas { get;  }
        public int Dimensao { get; }
        public string? Arquivo { get; }

        public Labirinto(string arquivo)
        {
            Celulas = new Dictionary<(int, int), Celula>();
            string _file = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\" + arquivo;
            if (File.Exists(_file))
            {
                Arquivo = _file;
                string[] linhas = File.ReadAllLines(Arquivo);
                Dimensao = Int32.Parse(linhas[0]);
                this._construir(linhas.Skip(1).ToArray());
            }
        }

        private void _construir(string[] linhas)
        {
            char[][] tabela = new char[Dimensao][];
            for (int i = 0; i < linhas.Length; i++)
            {
                tabela[i] = linhas[i].Replace(" ", string.Empty).ToCharArray();
            }

            for (int i = 0; i < Dimensao; i++)
            {
                for (int j = 0; j < Dimensao; j++)
                {
                    Celula? celula;
                    if (!Celulas.TryGetValue((i, j), out celula))
                    {
                        celula = new Celula((i, j), (TipoCelula) tabela[i][j]);
                        Celulas.Add(celula.Coords, celula);                        
                    }

                    foreach (Caminho vizinho in _getCamposVizinhos(celula.Coords))
                    {
                        Celula? destino;
                        if (!Celulas.TryGetValue(vizinho.Destino, out destino))
                        {
                            destino = new Celula(vizinho.Destino, (TipoCelula)tabela[vizinho.Destino.Item1][vizinho.Destino.Item2]);
                            Celulas.Add(destino.Coords, destino);
                        }
                        if (!_diagonalBloqueada(celula.Coords, destino.Coords))
                            celula.AddCaminho(destino.Coords, vizinho.Direcao);
                    }
                }
            }
        }

        private List<Caminho> _getCamposVizinhos((int, int) coords)
        {
            List<Caminho> ret = new List<Caminho>();

            int x = coords.Item1;
            int y = coords.Item2;

            if (_vizinhoValido(Dimensao, x, y - 1))
                ret.Add(new Caminho((x, y - 1), TipoCaminho.Norte));
            if (_vizinhoValido(Dimensao, x, y + 1))
                ret.Add(new Caminho((x, y + 1), TipoCaminho.Sul));

            if (_vizinhoValido(Dimensao, x + 1, y - 1))
                ret.Add(new Caminho((x + 1, y - 1), TipoCaminho.Nordeste));
            if (_vizinhoValido(Dimensao, x + 1, y))
                ret.Add(new Caminho((x + 1, y), TipoCaminho.Leste));
            if (_vizinhoValido(Dimensao, x + 1, y + 1))
                ret.Add(new Caminho((x + 1, y + 1), TipoCaminho.Sudeste));

            if (_vizinhoValido(Dimensao, x - 1, y - 1))
                ret.Add(new Caminho((x - 1, y - 1), TipoCaminho.Noroeste));
            if (_vizinhoValido(Dimensao, x - 1, y))
                ret.Add(new Caminho((x - 1, y), TipoCaminho.Oeste));
            if (_vizinhoValido(Dimensao, x - 1, y + 1))
                ret.Add(new Caminho((x - 1, y + 1), TipoCaminho.Nordeste));

            return ret;
        }

        private bool _vizinhoValido(int dimensao, int x, int y)
        {
            if (x < 0 || x > dimensao-1 || y < 0 || y > dimensao-1)
                return false;
            return true;
        }

        private bool _diagonalBloqueada((int, int) origem, (int,int) destino)
        {
            List<(int, int)> origVizinhos = _getCamposVizinhos(origem).Select(x => x.Destino).ToList();
            List<(int, int)> destVizinhos = _getCamposVizinhos(destino).Select(x => x.Destino).ToList();
            List<(int, int)> common = origVizinhos.Intersect(destVizinhos).ToList();

            return false;
        }
    }
}
