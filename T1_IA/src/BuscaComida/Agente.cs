using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Agente
    {
        public (int, int) Coords => Rota.Count > 0 ? Rota.Last().Destino : Entrada;
        public (int, int) Entrada { get; }
        public int Id { get; }
        public List<Caminho> Rota { get; set; }
        public Labirinto Labirinto { get; }
        public List<(int,int)> ComidasColetadas { get; private set; }
        public double Aptidao { get; set; }
        public int Geracao { get; }
        public List<Mutacao> Mutacoes { get; }
        public Crossover? Crossover { get; private set; }
        public long TempoMutacoes => Mutacoes.Sum(x => x.TempoTotal);
        public long TempoTotal => Crossover is null ? TempoMutacoes : TempoMutacoes + Crossover!.TempoSelecao + Crossover!.TempoCrossover;

        public Agente(Labirinto labirinto, int id, int geracao, Crossover? crossover = null)
        {
            Aptidao = 1.0;
            Id = id;
            ComidasColetadas = new List<(int, int)>();
            Entrada = labirinto.Entrada;
            Labirinto = labirinto;
            Rota = new List<Caminho>();
            Geracao = geracao;
            Mutacoes = new List<Mutacao>();
            Crossover = crossover;
        }

        public List<Caminho> CaminhoFromToCoords((int, int) from, (int, int) to)
        {
            List<Caminho> caminhos = new List<Caminho>();
            bool found = false;
            foreach (Caminho caminho in Rota)
            {
                if (found)
                {
                    if (caminho.Origem == to)
                    {
                        break;
                    }
                    caminhos.Add(caminho);
                }
                if (caminho.Origem == from)
                {
                    caminhos.Add(caminho);
                    found = true;
                }
            }
            return caminhos;
        }

        public bool Mover(TipoCaminho direcao)
        {
            if (!IsSatisfeito())
            {
                Celula origem = Labirinto.Celulas.GetValueOrDefault(Coords)!;
                try
                {
                    (int, int) destino = origem.GetDirecao(direcao).Destino;
                    Rota.Add(new Caminho(destino, origem.Coords, direcao));
                    _checarCampo();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool IsSatisfeito()
        {
            return ComidasColetadas.Count == Labirinto.NumComidas;
        }

        public void RecalcularAptidao(Opcoes opcoes)
        {
            int qtdRepetidos = (Rota.Count - Rota.GroupBy(x => x.Destino).Count());
            int ptsColeta = ComidasColetadas.Count * (opcoes.MovePts + 1);
            int ptsUnicidade = (opcoes.LimiteMovimentos - qtdRepetidos) * 10;
            int ptsTamanho = opcoes.LimiteMovimentos - Rota.Count;
            int ptsTotal = ptsColeta + ptsTamanho + ptsUnicidade;
            Aptidao = 1.0 - ((double)ptsTotal / opcoes.CeilingPts);
        }

        public void RecalcularComida()
        {
            ComidasColetadas.Clear();
            IEnumerable<(int, int)> coleta = Rota.Where(x => Labirinto.Celulas.GetValueOrDefault(x.Destino)!.IsComida()).Select(x => x.Destino).Distinct();
            ComidasColetadas.AddRange(coleta);
            if (IsSatisfeito())
            {
                int indexLast = Rota.FindLastIndex(x => x.Destino == ComidasColetadas.Last());
                Rota = Rota.Take(indexLast+1).ToList();
            }
        }

        public void AplicarMutacao(Opcoes opcoes)
        {
            Random rand = new Random();
            int chanceTam = 50;
            int chanceExp = 25;
            int numMut = 3 * opcoes.TaxaMutacao / 25;

            for (int i = 0; i < numMut; i++)
            {
                int proc = rand.Next(1, 101);
                bool reduzir;
                bool explorar = false;
                if (proc <= chanceTam)
                {
                    reduzir = true;
                    if (proc <= chanceExp)
                        explorar = true;
                    Mutacao mutacao = new Mutacao(reduzir, explorar, this, opcoes, Labirinto);
                    Mutacoes.Add(mutacao);
                }
            }
        }

        private void _checarCampo()
        {
            if (!ComidasColetadas.Any(x => x == Coords))
            {
                Celula celula = Labirinto.Celulas.GetValueOrDefault(Coords)!;
                if (celula.IsComida())
                {
                    ComidasColetadas.Add(celula.Coords);
                }
            }
        }

        public override string ToString()
        {
            return "Agente " + Id.ToString().PadRight(4) + "| Ger. " + Geracao.ToString().PadRight(3) + " | " + ComidasColetadas.Count().ToString().PadLeft(2) + " comidas coletadas | aptidão: "
                + string.Format("{0:0.000}", Aptidao)+" | Tam. rota: "+Rota.Count + " | Repetições: " + (Rota.Count - Rota.GroupBy(x => x.Destino).Count()
                + " | Mutações: " + Mutacoes.Count);
        }
    }
}
