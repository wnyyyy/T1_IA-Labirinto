using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Celula
    {
        public (int, int) Coords { get; }
        public List<Caminho> Caminhos { get; }
        public TipoCelula Campo { get; }

        public Celula((int, int) coords, TipoCelula campo)
        {
            Coords = coords;
            Campo = campo;
            Caminhos = new List<Caminho>();
        }

        public bool IsComida() { return Campo.Equals(TipoCelula.Comida); }

        public bool IsValido() { return !Campo.Equals(TipoCelula.Parede); }

        public void AddCaminho((int, int) destino, TipoCaminho direcao)
        {
            Caminhos.Add(new Caminho(destino, direcao));
        }

        public override string ToString()
        {
            return Campo + ": " + Coords.ToString();
        }
    }
}
