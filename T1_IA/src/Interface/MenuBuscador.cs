using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA.Interface
{
    internal class MenuBuscador
    {
        public static void Show(string screen, BuscaComidas buscaComidas)
        {
            _appendScr(screen);
            Console.WriteLine("Melhor Cromossomo: ");
            Console.WriteLine(buscaComidas.PopulacaoAtual.First().ToString());
            Console.WriteLine();
            Console.Write("Geração ");
            Console.WriteLine(buscaComidas.NumGeracoes);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1 - Visualizar população atual");
            Console.WriteLine("2 - Histórico de populações");
            Console.WriteLine("3 - Nova geração");
            Console.WriteLine("4 - Percorrer com melhor Agente");
            Console.WriteLine("5 - Percorrer com solução de A*");
            Console.WriteLine("6 - Voltar");
            bool quit = false;
            while (quit == false)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1':
                        Console.WriteLine("\n");
                        Console.WriteLine("Digite o nome do arquivo, sem extensão (deve estar na mesma pasta do executável)");
                        break;
                    case '3':
                        _appendScr(screen);
                        Console.WriteLine("Informe a quantidade de gerações novas");
                        string? inp = null;
                        while (inp is null)
                            inp = Console.ReadLine();
                        int qtdGeracoes = Int32.Parse(inp);
                        Console.WriteLine();
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
    }
}
