﻿using System;
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
        public TipoCelula Campo { get; private set; }

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

        public Caminho GetDirecao(TipoCaminho direcao)
        {
            Caminho? caminho = Caminhos.Where(x => x.Direcao.Equals(direcao)).First();
            if (caminho is null)
                throw new AgenteMovimentoImpossivel();

            return caminho;
        }

        public bool ColetarComida()
        {
            if (Campo.Equals(TipoCelula.Comida))
            {
                Campo = TipoCelula.Vazio;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return Campo + ": " + Coords.ToString();
        }
    }
}
