using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Agente
    {
        public (int, int) Coords { get; private set; }
        public (int, int) Origem { get; }
        public int Id { get; }
        public List<Caminho> Rota { get; }
        public Labirinto Labirinto { get; }
        public List<(int,int)> ComidasColetadas { get; private set; }

        public Agente(Labirinto labirinto, int id)
        {       
            Id = id;
            ComidasColetadas = new List<(int, int)>();
            Coords = labirinto.Entrada;
            Origem = labirinto.Entrada;
            Labirinto = labirinto;
            Rota = new List<Caminho>();
        }

        public bool Mover(TipoCaminho direcao)
        {
            Celula origem = Labirinto.Celulas.GetValueOrDefault(Coords)!;
            try
            {
                (int, int) destino = origem.GetDirecao(direcao).Destino;
                Coords = destino;
                Rota.Add(new Caminho(destino, direcao));
                _checarCampo();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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
            return "Agente " + Id + " - " + ComidasColetadas.Count() + " comidas";
        }
    }
}
