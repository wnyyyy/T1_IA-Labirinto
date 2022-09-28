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
            Console.WriteLine("2 - Voltar");
            Console.WriteLine("3 - Sair");
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
                            Console.WriteLine("Informe o coeficiente do tamanho da população (recomendado = 15)");
                            Console.WriteLine("O coeficiente é uma porcentagem aplicada ao tamanho base da população: 300");
                            Console.WriteLine();
                            int coefTamPopulacao = Util.LerInt();
                            _appendScr(screen);
                            Console.WriteLine("Informe a porcentagem de elitismo (recomendado = 20)");
                            Console.WriteLine();
                            int elitismo = Util.LerInt();
                            _appendScr(screen);
                            Console.WriteLine("Informe a taxa de Mutação (recomendado = 35 ou 25~42)");
                            Console.WriteLine("Maior taxa de mutação aumenta a chance de achar todas as comidas, mas deixa a rota mais subótima");
                            Console.WriteLine();
                            int taxaMutacao = Util.LerInt();
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
                    case '2':
                        MenuInicio.Show(screen);
                        break;
                    case '3':
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
