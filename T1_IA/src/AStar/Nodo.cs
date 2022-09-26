using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA.AStar
{
    internal class Nodo
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Custo { get; set; }
        public int Distancia { get; private set; }
        public int CustoDistancia => Custo + Distancia;
        public Nodo? Pai { get; set; }

        public Nodo(int x, int y)
        {
            X = x;
            Y = y;
            Custo = 0;
        }

        public void SetDistancia(int toX, int toY)
        {
            int dx = Math.Abs(X - toX);
            int dy = Math.Abs(Y - toY);
            int test = 1 * (dx + dy) + (1 - 2 * 1) * Math.Min(dx, dy);
            int test2 = Math.Max(dx, dy);
            Distancia = dx + dy - Math.Min(dx, dy);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ") - Custo: " + CustoDistancia;
        }

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return base.Equals(obj);
            else
            {
                Nodo target = (obj as Nodo)!;
                return this.X == target.X && this.Y == target.Y;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
