using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1_IA;
using T1_IA.AStar;

namespace T1_IA.Interface
{
    internal class MenuBuscador
    {
        public static void Show(string screen, BuscaComidas buscaComidas)
        {
            _appendScr(screen);
            StringBuilder menu = new StringBuilder();
            menu.AppendLine("Melhor Cromossomo: ");
            menu.AppendLine(buscaComidas.PopulacaoAtual.First().ToString());
            menu.AppendLine();
            menu.Append("Geração ");
            menu.AppendLine(buscaComidas.NumGeracoes.ToString());
            menu.Append("Tempo de geração (acumulado): ");
            menu.Append(String.Format("{0:0.#}", (double)buscaComidas.TempoTotal / TimeSpan.TicksPerMillisecond));
            menu.Append(" ms | ");
            menu.Append(String.Format("{0:0.##}", (double)buscaComidas.TempoTotal / TimeSpan.TicksPerSecond));
            menu.AppendLine(" sec");
            menu.AppendLine();
            menu.AppendLine("1 - Visualizar população atual");
            menu.AppendLine("2 - Histórico de populações");
            menu.AppendLine("3 - Nova geração");
            menu.AppendLine("4 - Percorrer com melhor Agente");
            menu.AppendLine("5 - Percorrer com solução de A*");
            menu.AppendLine("6 - Voltar");
            Console.WriteLine(menu.ToString());
            bool quit = false;
            while (quit == false)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1':                      
                        _visualizarPopulacao(screen, buscaComidas);
                        break;
                    case '2':
                        _visualizarHistorico(screen, buscaComidas);
                        break;
                    case '3':
                        Console.WriteLine("Quantidade de gerações: ");
                        int count = Util.LerInt();
                        for (int i = 0; i < count; i++)
                        {
                            buscaComidas.NovaGeracao();
                        }
                        Show(screen, buscaComidas);
                        break;
                    case '4':
                        Agente agente = buscaComidas.PopulacaoAtual[0];
                        _percorrerLabirinto(agente.Rota, buscaComidas.Labirinto, agente.ComidasColetadas, screen, buscaComidas);
                        break;
                    case '5':
                        BuscaRota buscaRota = new BuscaRota(buscaComidas.Labirinto);
                        List<Caminho> rota = buscaRota.Buscar(buscaComidas.Labirinto.Comidas, buscaComidas.Labirinto.Entrada);
                        _percorrerLabirinto(rota, buscaComidas.Labirinto, buscaComidas.Labirinto.Comidas, screen, buscaComidas);
                        break;
                    case '6':
                        MenuInicio.Show(screen);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void _appendScr(string screen)
        {
            Console.Clear();
            Console.WriteLine(screen);
            Console.WriteLine(new String('-', Console.WindowWidth - 1));
            Console.WriteLine();
        }

        private static void _visualizarPopulacao(string screen, BuscaComidas buscaComidas)
        {
            _appendScr(screen);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (Agente agente in buscaComidas.PopulacaoAtual)
            {
                sb.AppendLine(agente.ToString());
            }
            sb.AppendLine();
            sb.AppendLine("População atual");
            sb.AppendLine();
            sb.Append("Tempo de geração: ");
            sb.Append(String.Format("{0:0.#}", (double)buscaComidas.Geracoes.Last().TempoGerada / TimeSpan.TicksPerMillisecond));
            sb.Append(" ms | ");
            sb.Append(String.Format("{0:0.##}", (double)buscaComidas.Geracoes.Last().TempoGerada / TimeSpan.TicksPerSecond));
            sb.AppendLine(" sec");
            sb.AppendLine();
            sb.AppendLine("Pressione uma tecla para voltar.");
            Console.WriteLine(sb.ToString());
            Console.ReadKey();
            Show(screen, buscaComidas);
        }

        private static void _visualizarHistorico(string screen, BuscaComidas buscaComidas)
        {
            _appendScr(screen);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Histórico");
            foreach (Geracao geracao in buscaComidas.Geracoes)
            {
                sb.AppendLine();
                sb.AppendLine(geracao.ToString());
            }
            sb.AppendLine();
            sb.AppendLine("Pressione uma tecla para voltar.");
            Console.WriteLine(sb.ToString());
            Console.ReadKey();
            Show(screen, buscaComidas);
        }

        private static void _percorrerLabirinto(List<Caminho> caminhos, Labirinto labirinto, List<(int,int)> coletadas, string screen, BuscaComidas buscaComidas)
        {
            List<string> frames = RenderLabirinto.Percorrer(labirinto, caminhos, coletadas);
            foreach (string frame in frames)
            {
                Console.Clear();
                Console.WriteLine(frame);
                Thread.Sleep(500);
            }
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Pressione uma tecla para voltar.");
            Show(screen, buscaComidas);
        }
    }
}
