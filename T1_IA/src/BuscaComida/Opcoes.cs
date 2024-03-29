﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{ 
    internal class Opcoes
    {
        public int LimiteMovimentos { get; }
        public int TamPopulacao { get; }
        public int TaxaMutacao { get; }
        public int AgressividadeMutacao { get; }
        public int Elitismo { get; }
        public int CeilingPts { get; }
        public int MovePts { get; }
        public int Dimensao { get; }

        public Opcoes(int coefTamanhoPopulacao, int taxaMutacao, int agressividadeMutacao, int elitismo, Labirinto labirinto)
        {
            AgressividadeMutacao = agressividadeMutacao;
            Elitismo = elitismo;
            TamPopulacao = 300 * coefTamanhoPopulacao / 100;
            TaxaMutacao = taxaMutacao;
            LimiteMovimentos = labirinto.Dimensao * labirinto.Dimensao - labirinto.NumParedes;
            Dimensao = labirinto.Dimensao;
            MovePts = LimiteMovimentos + LimiteMovimentos * 10;
            CeilingPts = labirinto.NumComidas * MovePts + labirinto.NumComidas + MovePts;
        }
    }
}
