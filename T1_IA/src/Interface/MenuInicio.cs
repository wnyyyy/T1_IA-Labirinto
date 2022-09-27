using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA.Interface
{
    internal static class MenuInicio
    {
        public static void Show(string screen)
        {
            _appendScr(screen);
            Console.WriteLine("1 - Gerar Labirinto");
            Console.WriteLine("2 - Sair");
            bool quit = false;
            while (quit == false)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1':
                        _appendScr(screen);
                        Console.WriteLine("Digite o nome do arquivo, sem extensão (deve estar na mesma pasta do executável)");
                        Console.WriteLine();
                        string? name = Console.ReadLine();
                        if (name != null)
                        {
                            name = name + ".txt";
                            try
                            {
                                Console.WriteLine();
                                Labirinto labirinto = new Labirinto(name);
                                screen = RenderLabirinto.Show(labirinto);
                                MenuIntermediario.Show(screen, labirinto);
                            }
                            catch (LabirintoArquivoNaoEncontrado)
                            {
                                Console.WriteLine("Arquivo \""+name+"\" não encontrado.");
                                Console.ReadKey();
                                Show(screen);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Labirinto em formato inválido.");
                                Console.ReadKey();
                                Show(screen);
                            }
                        }
                        break;
                    case '2':
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
