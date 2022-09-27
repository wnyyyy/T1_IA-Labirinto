using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA.Interface
{
    internal class RenderLabirinto
    {
        public static string Show(Labirinto labirinto)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append(new String(' ', Console.WindowWidth / 2 - 1 - labirinto.Dimensao));
            sb.Append("Dimensão: ");
            sb.AppendLine(labirinto.Dimensao.ToString());
            sb.AppendLine();
            for (int i = 0; i < labirinto.Dimensao; i++)
            {
                sb.Append(new String(' ', Console.WindowWidth/2 - 1 - labirinto.Dimensao));
                for (int j = 0; j < labirinto.Dimensao; j++)
                {
                    Celula cel = labirinto.Celulas[(i,j)];
                    sb.Append((char)cel.Campo);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
