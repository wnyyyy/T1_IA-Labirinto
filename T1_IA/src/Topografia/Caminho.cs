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
        public (int, int) Origem { get; }
        public (int, int) Destino { get; }

        public Caminho((int, int) destino, (int, int) origem, TipoCaminho direcao)
        {
            Origem = origem;
            Destino = destino;
            Direcao = direcao;
        }

        public bool IsDiagonal()
        {
            return Direcao == TipoCaminho.Noroeste ||
                   Direcao == TipoCaminho.Nordeste ||
                   Direcao == TipoCaminho.Sudeste  ||
                   Direcao == TipoCaminho.Sudoeste;
        }

        public override string ToString()
        {
            return Direcao.ToString() + ": " + Destino.ToString();
        }
    }
}
