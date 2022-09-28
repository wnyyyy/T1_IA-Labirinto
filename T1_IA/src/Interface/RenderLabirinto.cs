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
            bool altRender = true;
            Dictionary<char, char> dictAltRender = new Dictionary<char, char> { { '0', '.' }, { '1', '#' }, { 'A', 'o' } };
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
                    char campo = (char)cel.Campo;
                    if (altRender)
                        campo = dictAltRender.ContainsKey(campo) ? dictAltRender[campo] : campo;
                    sb.Append(campo);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static List<string> Percorrer(Labirinto labirinto, List<Caminho> caminhos, List<(int,int)> coletadas)
        {
            bool altRender = true;
            Dictionary<char, char> dictAltRender = new Dictionary<char, char>{ {'0', '.' }, {'1','#'}, { 'A', 'o' } };
            List<string> result = new List<string>();
            for (int k = 0; k < caminhos.Count+1; k++)
            {
                (int, int) agente;
                if (k < caminhos.Count)
                    agente = caminhos[k].Origem;
                else
                    agente = caminhos[k-1].Destino;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < labirinto.Dimensao; i++)
                {
                    sb.Append(new String(' ', Console.WindowWidth / 2 - 1 - labirinto.Dimensao));
                    for (int j = 0; j < labirinto.Dimensao; j++)
                    {
                        Celula cel = labirinto.Celulas[(i, j)];
                        char campo = (char)cel.Campo;
                        if (cel.Coords == agente)
                            campo = (char)TipoCelula.Agente;
                        else if (coletadas.Contains(cel.Coords))
                            campo = (char)TipoCelula.ComidaColetada;
                        if (altRender)
                            campo = dictAltRender.ContainsKey(campo) ? dictAltRender[campo] : campo;
                        sb.Append(campo);
                        sb.Append(' ');
                    }
                    sb.AppendLine();
                }
                result.Add(sb.ToString());
            }
            return result;
        }
    }
}
