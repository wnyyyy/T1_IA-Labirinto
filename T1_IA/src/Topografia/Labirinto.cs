using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Labirinto
    {
        public Dictionary<(int, int), Celula> Celulas { get;  }
        public int Dimensao { get; }
        public (int, int) Entrada { get; private set; }
        public int NumComidas => Comidas.Count;
        public List<(int, int)> Comidas{ get; private set; }
        public int NumParedes { get; private set; }
        public string? Arquivo { get; }

        public Labirinto(string arquivo)
        {
            Comidas = new List<(int, int)>();
            NumParedes = 0;
            Celulas = new Dictionary<(int, int), Celula>();
            Arquivo = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\" + arquivo;

            string[] linhas;
            try
            {
                linhas = File.ReadAllLines(Arquivo);
            }
            catch (Exception)
            {
                throw new LabirintoArquivoNaoEncontrado();
            }
            
            try
            {
                Dimensao = Int32.Parse(linhas[0]);
            }
            catch (Exception)
            {
                throw new LabirintoDimensaoInvalida();
            }
                
            this._construir(linhas.Skip(1).ToArray());
        }

        private void _construir(string[] linhas)
        {
            char[][] tabela = new char[Dimensao][];
            try
            {
                for (int i = 0; i < Dimensao; i++)
                {
                    tabela[i] = linhas[i].Replace(" ", string.Empty).ToCharArray();
                    if (tabela[i].Length != Dimensao)
                        throw new LabirintoTabelaFormatoInvalido();
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new LabirintoTabelaFormatoInvalido();
            }

            bool hasEntrada = false;

            for (int i = 0; i < Dimensao; i++)
            {
                for (int j = 0; j < Dimensao; j++)
                {
                    char charTipo = Char.ToUpper(tabela[i][j]);
                    if (!Enum.IsDefined(typeof(TipoCelula), (int)charTipo))
                    {
                        throw new LabirintoTabelaCampoInvalido();
                    }
                    TipoCelula tipo = (TipoCelula)charTipo;
                    if (tipo == TipoCelula.Parede)
                        NumParedes++;
                    else if (tipo == TipoCelula.Comida)
                        Comidas.Add((i, j));
                    else if (tipo == TipoCelula.Entrada)
                    {
                        if (hasEntrada)
                            throw new LabirintoTabelaMultiplasEntradas();
                        hasEntrada = true;
                        Entrada = (i, j);
                    }

                    Celula celula = new Celula((i, j), tipo);
                    Celulas.Add(celula.Coords, celula);
                }
            }

            if (NumComidas == 0)
                throw new LabirintoTabelaSemComida();
            if (!hasEntrada)
                throw new LabirintoTabelaSemEntrada();

            foreach (Celula celula in Celulas.Select(x => x.Value).Where(x => x.IsValido()))
            {
                foreach (Caminho caminho in _getCamposVizinhos(celula.Coords))
                {
                    Celula? destino = Celulas.GetValueOrDefault(caminho.Destino);
                    if (destino is not null)
                    {
                        if (destino.IsValido())
                        {
                            if (!caminho.IsDiagonal() || _diagonalValida(celula.Coords, destino.Coords))
                                celula.AddCaminho(destino.Coords, celula.Coords, caminho.Direcao);
                        }
                    }                                              
                    else
                    {
                        throw new LabirintoTabelaFormatoInvalido();
                    }
                }
            }
        }

        private List<Caminho> _getCamposVizinhos((int, int) coords)
        {
            List<Caminho> ret = new List<Caminho>();

            int x = coords.Item1;
            int y = coords.Item2;

            if (_vizinhoValido(Dimensao,x, y - 1))
                ret.Add(new Caminho((x, y - 1), coords, TipoCaminho.Oeste));
            if (_vizinhoValido(Dimensao,x, y + 1))
                ret.Add(new Caminho((x, y + 1), coords, TipoCaminho.Leste));

            if (_vizinhoValido(Dimensao, x + 1, y - 1))
                ret.Add(new Caminho((x + 1, y - 1), coords, TipoCaminho.Sudoeste));
            if (_vizinhoValido(Dimensao, x + 1, y))
                ret.Add(new Caminho((x + 1, y), coords, TipoCaminho.Sul));
            if (_vizinhoValido(Dimensao,x + 1, y + 1))
                ret.Add(new Caminho((x + 1, y + 1), coords, TipoCaminho.Sudeste));

            if (_vizinhoValido(Dimensao, x - 1, y - 1))
                ret.Add(new Caminho((x - 1, y - 1), coords, TipoCaminho.Noroeste));
            if (_vizinhoValido(Dimensao,x - 1, y))
                ret.Add(new Caminho((x - 1, y), coords, TipoCaminho.Norte));
            if (_vizinhoValido(Dimensao,x - 1, y + 1))
                ret.Add(new Caminho((x - 1, y + 1), coords, TipoCaminho.Nordeste));

            return ret;
        }

        private bool _vizinhoValido(int dimensao, int x, int y)
        {
            if (x < 0 || x > dimensao-1 || y < 0 || y > dimensao-1)
                return false;
            return true;
        }

        private bool _diagonalValida((int, int) origem, (int,int) destino)
        {
            List<Caminho> origVizinhos = _getCamposVizinhos(origem);
            List<Caminho> destVizinhos = _getCamposVizinhos(destino);
            List<(int, int)> common = origVizinhos.Select(x => x.Destino).
                Intersect(destVizinhos.Select(x => x.Destino)).ToList();

            List<Caminho> aux = origVizinhos.Where(x => common.Any(y => y.Equals(x.Destino))).ToList();

            return aux.Any(x => Celulas[x.Destino].IsValido());
        }
    }
}
