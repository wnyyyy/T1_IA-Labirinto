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

        public void AddCaminho((int, int) destino, (int, int) origem, TipoCaminho direcao)
        {
            Caminhos.Add(new Caminho(destino, origem, direcao));
        }

        public Caminho GetDirecao(TipoCaminho direcao)
        {
            Caminho? caminho = Caminhos.Where(x => x.Direcao.Equals(direcao)).First();
            if (caminho is null)
                throw new AgenteMovimentoImpossivel();

            return caminho;
        }

        public override string ToString()
        {
            return Campo + ": " + Coords.ToString();
        }
    }
}
