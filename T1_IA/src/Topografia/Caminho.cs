using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal class Caminho
    {
        public TipoCaminho Direcao { get; }
        public (int, int) Destino { get; }

        public Caminho((int, int) destino, TipoCaminho direcao)
        {
            Destino = destino;
            Direcao = direcao;
        }

        public override string ToString()
        {
            return Direcao.ToString() + ": " + Destino.ToString();
        }
    }
}
