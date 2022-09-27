using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA.Interface
{
    internal class MenuIntermediario
    {
        public static void Show(string screen, Labirinto labirinto)
        {
            _appendScr(screen);
            Console.WriteLine("1 - Iniciar Busca");
            Console.WriteLine("2 - Iniciar Busca");
            Console.WriteLine("3 - Voltar");
            Console.WriteLine("4 - Sair");
            bool quit = false;
            while (quit == false)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1':
                        try
                        {
                            _appendScr(screen);
                            Console.WriteLine("Informe o coeficiente do tamanho da população (recomendado = 10)");
                            Console.WriteLine("A população total é igual a 300/coeficiente");
                            Console.WriteLine();
                            string? inp = null;
                            while (inp is null)
                                inp = Console.ReadLine();
                            int coefTamPopulacao = Int32.Parse(inp);
                            _appendScr(screen);
                            Console.WriteLine("Informe a porcentagem de elitismo (recomendado = 20)");
                            Console.WriteLine();
                            inp = null;
                            while (inp is null)
                                inp = Console.ReadLine();
                            int elitismo = Int32.Parse(inp);
                            _appendScr(screen);
                            Console.WriteLine("Informe a taxa de Mutação (recomendado = 25)");
                            Console.WriteLine();
                            inp = null;
                            while (inp is null)
                                inp = Console.ReadLine();
                            int taxaMutacao = Int32.Parse(inp);
                            BuscaComidas buscaComidas = new BuscaComidas(labirinto, coefTamPopulacao, taxaMutacao, 100, elitismo);
                            MenuBuscador.Show(screen, buscaComidas);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\n\nInforme um valor válido.");
                            Console.ReadKey();
                            MenuIntermediario.Show(screen, labirinto);
                        }
                        break;
                    case '5':
                        MenuInicio.Show(screen);
                        break;
                    case '6':
                        quit = true;
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
