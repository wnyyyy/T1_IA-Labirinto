using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    enum TipoCelula
    {
        Comida = 'C',
        Entrada = 'E',
        Parede = '1',
        Vazio = '0',
        ComidaColetada = 'x',
        Agente = 'A',
    }

    enum TipoCaminho
    {
        Norte,
        Sul,
        Leste,
        Oeste,
        Noroeste,
        Nordeste,
        Sudoeste,
        Sudeste
    }
}
