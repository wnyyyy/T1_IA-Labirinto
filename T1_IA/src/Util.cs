using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal static class Util
    {
        public static TipoCaminho? GetDirecaoFromCoords((int, int) from, (int, int) to)
        {
            (int, int) calc = _calcCoords(from, to);
            int x = calc.Item1;
            int y = calc.Item2;

            if (x == 0)
            {
                if (y < 0)
                    return TipoCaminho.Oeste;
                if (y > 0)
                    return TipoCaminho.Leste;
            }
            if (x > 0)
            {
                if (y == 0)
                    return TipoCaminho.Sul;
                if (y < 0)
                    return TipoCaminho.Sudoeste;
                if (y > 0)
                    return TipoCaminho.Sudeste;
            }
            if (x < 0)
            {
                if (y == 0)
                    return TipoCaminho.Norte;
                if (y < 0)
                    return TipoCaminho.Noroeste;
                if (y > 0)
                    return TipoCaminho.Nordeste;
            }

            return null;
        }

        public static bool ValidaCaminho(List<Caminho> rota)
        {
            (int, int)? oldDestino = null;
            foreach (Caminho caminho in rota)
            {
                TipoCaminho? direcao = GetDirecaoFromCoords(caminho.Origem, caminho.Destino);
                if (direcao! != caminho.Direcao)
                    return false;
                if (oldDestino is not null && oldDestino != caminho.Origem)
                    return false;
                oldDestino = caminho.Destino;

            }
            return true;
        }

        private static (int, int) _calcCoords((int, int) from, (int, int) to)
        {
            int x1 = from.Item1;
            int y1 = from.Item2;
            int x2 = to.Item1;
            int y2 = to.Item2;
            return (x2 - x1, y2 - y1);
        }
    }
}
