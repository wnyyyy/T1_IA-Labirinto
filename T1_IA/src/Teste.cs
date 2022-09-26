using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    internal static class Teste
    {
        public static void Testar()
        {
            Labirinto labirinto = new Labirinto("labirinto1.txt");

            int loops = 100;
            Thread th1 = new Thread(() => _thread(labirinto, elitismo: 20, coefTamanhoPopulacao: 10, taxaMutacao: 100, loops, "Thread 1"));
            th1.Name = "Thread 1";

            Thread th2 = new Thread(() => _thread(labirinto, elitismo: 20, coefTamanhoPopulacao: 10, taxaMutacao: 50, loops, "Thread 2"));
            th2.Name = "Thread 2";

            Thread th3 = new Thread(() => _thread(labirinto, elitismo: 20, coefTamanhoPopulacao: 10, taxaMutacao: 25, loops, "Thread 3"));
            th3.Name = "Thread 3";

            Thread th4 = new Thread(() => _thread(labirinto, elitismo: 20, coefTamanhoPopulacao: 10, taxaMutacao: 15, loops, "Thread 4"));
            th3.Name = "Thread 4";

            Thread th5 = new Thread(() => _thread(labirinto, elitismo: 10, coefTamanhoPopulacao: 10, taxaMutacao: 25, loops, "Thread 5"));
            th3.Name = "Thread 5";

            Thread th6 = new Thread(() => _thread(labirinto, elitismo: 20, coefTamanhoPopulacao: 5, taxaMutacao: 25, loops, "Thread 6"));
            th3.Name = "Thread 6";

            //th1.Start(); 
            //th2.Start(); 
            th3.Start();
            //th4.Start(); 
            //th5.Start(); 
            //th6.Start();
            //th1.Join(); 
            //th2.Join(); 
            th3.Join(); 
            //th4.Join(); 
            //th5.Join(); 
            //th6.Join();
        }

        private static void _thread(Labirinto labirinto, int elitismo, int coefTamanhoPopulacao, int taxaMutacao, int loops, string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("Elitismo: ");
            sb.Append(elitismo);
            sb.AppendLine();
            sb.Append("Taxa de Mutação: ");
            sb.Append(taxaMutacao);
            sb.AppendLine();
            sb.Append("Tamanho da População: ");
            sb.Append(coefTamanhoPopulacao*10);
            sb.AppendLine();
            sb.Append("Número de Gerações: ");
            sb.Append(loops);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            for (int i = 1; i <= 100; i++)
            {
                sb.AppendLine("Execução " + i);
                sb.AppendLine();
                BuscaComidas buscar = new BuscaComidas(labirinto, coefTamanhoPopulacao, taxaMutacao, elitismo);
                for (int j = 0; j < loops; j++)
                {
                    buscar.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
                    buscar.NovaGeracao();
                }
                _escrever(buscar, sb);
                sb.AppendLine();
                sb.AppendLine();
            }
            _gravarResultados(sb, name);
        }

        private static void _escrever(BuscaComidas buscaComidas, StringBuilder sb)
        {   
            sb.AppendLine(buscaComidas.Populacao[0].ToString());
            sb.AppendLine(buscaComidas.Populacao[1].ToString());
            sb.AppendLine(buscaComidas.Populacao[2].ToString());
        }

        private static void _gravarResultados(StringBuilder sb, string name)
        {
            string path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\" + name+".txt";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding().GetBytes(sb.ToString());

                    fs.Write(info, 0, info.Length);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
