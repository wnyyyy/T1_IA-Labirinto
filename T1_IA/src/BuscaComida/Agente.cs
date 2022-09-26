﻿using System;
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

        public Agente(Labirinto labirinto, int id, int geracao)
        {
            Aptidao = 1.0;
            Id = id;
            ComidasColetadas = new List<(int, int)>();
            Entrada = labirinto.Entrada;
            Labirinto = labirinto;
            Rota = new List<Caminho>();
            Geracao = geracao;
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

        public void RecalcularComida()
        {
            ComidasColetadas.Clear();
            IEnumerable<(int, int)> coleta = Rota.Where(x => Labirinto.Celulas.GetValueOrDefault(x.Destino)!.IsComida()).Select(x => x.Destino).Distinct();
            ComidasColetadas.AddRange(coleta);
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
            return "Agente " + Id + ", Ger. " + Geracao + " - " + ComidasColetadas.Count() + " comidas coletadas | aptidão: "
                + string.Format("{0:0.000}", Aptidao)+" | Tamanho da rota: "+Rota.Count + " | Repetições na rota: " + (Rota.Count - Rota.GroupBy(x => x.Destino).Count());
        }
    }
}