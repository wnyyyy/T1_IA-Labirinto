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
        public Labirinto Labirinto { get; }

        public Agente(Labirinto labirinto)
        {
            Coords = labirinto.Entrada;
            Labirinto = labirinto;
        }

        public bool Mover(TipoCaminho direcao)
        {
            Celula? origem = Labirinto.Celulas.GetValueOrDefault(Coords);
            try
            {
                (int, int) destino = origem!.GetDirecao(direcao).Destino;
                Coords = destino;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool ChecarCampo()
        {
            Celula? celula = Labirinto.Celulas.GetValueOrDefault(Coords);
            return celula!.ColetarComida();
        }
    }
}
