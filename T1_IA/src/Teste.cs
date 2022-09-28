using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Statistics;
using System.Threading.Tasks;

namespace T1_IA
{
    internal static class Teste
    {
        public static void Testar()
        {
            Labirinto labirinto = new Labirinto("labirinto1.txt");

            #region teste mutação 1
            //int[] arrElitismo = {
            //    20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20
            //};
            //int[] arrTaxaMutacao = {
            //    25, 25, 25, 50, 50, 50, 75, 75, 75, 15, 15, 15, 100, 100, 100
            //};
            //int[] arrCoefTamanhoPopulacao = {
            //    10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10
            //};
            //int[] arrAgressividadeMutacao = {
            //    100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100
            //};
            #endregion
            #region teste mutação 2
            //int[] arrElitismo = {
            //    20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20
            //};
            //int[] arrTaxaMutacao = {
            //    25, 25, 25, 30, 30, 30, 35, 35, 35, 40, 40, 40, 50, 50, 50
            //};
            //int[] arrCoefTamanhoPopulacao = {
            //    10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10
            //};
            //int[] arrAgressividadeMutacao = {
            //    100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100
            //};
            #endregion
            #region teste mutação 3
            //int[] arrElitismo = {
            //    20, 20, 20
            //};
            //int[] arrTaxaMutacao = {
            //    25, 30, 35
            //};
            //int[] arrCoefTamanhoPopulacao = {
            //    10, 10, 10
            //};
            //int[] arrAgressividadeMutacao = {
            //    100, 100, 100
            //};
            #endregion
            #region teste população
            int[] arrElitismo = {
                20, 20, 20, 20, 20, 20, 20, 20
            };
            int[] arrTaxaMutacao = {
                35, 35, 35, 35, 35, 35, 35, 35
            };
            int[] arrCoefTamanhoPopulacao = {
                10, 15, 20, 25, 30, 35, 40, 45
            };
            int[] arrAgressividadeMutacao = {
                100, 100, 100, 100, 100, 100, 100, 100
            };
            #endregion

            #region teste único
            //int[] arrElitismo = {
            //    20
            //};
            //int[] arrTaxaMutacao = {
            //    35
            //};
            //int[] arrCoefTamanhoPopulacao = {
            //    20
            //};
            //int[] arrAgressividadeMutacao = {
            //    100
            //};
            #endregion

            int numGeracoes = 100;
            int loops = 1000;
            double targetAptidao1 = 0.040;
            double targetAptidao2 = 0.020;

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < arrElitismo.Length; i++)
            {
                string name = "Execucão " + (i+1).ToString();
                int elitismo = arrElitismo[i];
                int taxaMutacao = arrTaxaMutacao[i];
                int coefTamanhoPopulacao = arrCoefTamanhoPopulacao[i];
                int agressividadeMutacao = arrAgressividadeMutacao[i];
                Thread t = new Thread(() => _thread(
                    labirinto, elitismo: elitismo, coefTamanhoPopulacao: coefTamanhoPopulacao, taxaMutacao: taxaMutacao, agressividadeMutacao: agressividadeMutacao,
                    loops, numGeracoes, targetAptidao1, targetAptidao2, name));
                t.Name = name;
                t.Start();
                threads.Add(t);
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }
        }

        private static void _thread(Labirinto labirinto, int elitismo, int coefTamanhoPopulacao,
            int taxaMutacao, int agressividadeMutacao, int loops, int numGeracoes, double targetAptidao1, double targetAptidao2, string name)
        {
            StringBuilder sb = new StringBuilder();
            int numTarget1 = 0;
            int numTarget2 = 0;
            sb.AppendLine(name);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append("Elitismo: ");
            sb.Append(elitismo);
            sb.AppendLine();
            sb.Append("Taxa de Mutação: ");
            sb.Append(taxaMutacao);
            sb.AppendLine();
            sb.Append("Agressividade da Mutação: ");
            sb.Append(agressividadeMutacao);
            sb.AppendLine();
            sb.Append("Tamanho da População: ");
            sb.Append(coefTamanhoPopulacao*10);
            sb.AppendLine();
            sb.Append("Número de Gerações: ");
            sb.Append(loops);
            sb.AppendLine();
            sb.AppendLine();
            StringBuilder sb2 = new StringBuilder();
            List<double> lstTempoGeracao = new List<double>();
            List<double> lstTempoExecucao = new List<double>();
            double aux = 0;
            for (int i = 1; i <= loops; i++)
            {
                sb2.AppendLine("Execução " + i);
                sb2.AppendLine();
                BuscaComidas buscar = new BuscaComidas(labirinto, coefTamanhoPopulacao, taxaMutacao, agressividadeMutacao, elitismo);
                for (int j = 0; j < numGeracoes-1; j++)
                {
                    buscar.PopulacaoAtual.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
                    buscar.NovaGeracao();
                    Console.WriteLine("Geração "+j+" | Loop "+i);
                    lstTempoGeracao.Add((double)buscar.Geracoes.Last().TempoGerada / TimeSpan.TicksPerMillisecond);
                }
                lstTempoExecucao.Add((double)buscar.TempoTotal / TimeSpan.TicksPerMillisecond);
                _escrever(buscar, sb2);
                sb2.AppendLine();
                for (int c = 0 ; c < 3; c++)
                {
                    if (buscar.PopulacaoAtual[c].Aptidao <= targetAptidao1)
                    {
                        numTarget1++;
                        if (buscar.PopulacaoAtual[c].Aptidao <= targetAptidao2)
                        {
                            numTarget2++;
                        }                            
                    }
                    aux++;
                }
                sb2.AppendLine();
                sb2.AppendLine();
            }
            sb.AppendLine("Target 1: " + String.Format("{0:0.##}", (double)(numTarget1*100.0 / aux)) + "%| Target 2: " + String.Format("{0:0.##}", (double)(numTarget2*100.0 / aux)) + "%");
            sb.AppendLine();
            sb.Append("Mediana Tempo por Nova Geração: ");
            sb.AppendLine(String.Format("{0:0.#}", lstTempoGeracao.Median()) + " ms");
            sb.Append("Média Harmônica Tempo por Execução: ");
            double ex = lstTempoExecucao.HarmonicMean();
            sb.AppendLine(String.Format("{0:0.##}", ex) + " ms || " + String.Format("{0:0.#}", ex/1000.0) + " sec");            
            sb.Append("Tempo Total: ");
            double total = lstTempoExecucao.Sum();
            sb.AppendLine(String.Format("{0:0.#}", total) + " ms || " + String.Format("{0:0.#}", total/1000.0) + " sec");
            sb.AppendLine();
            sb.AppendLine("--------------------------------------------------------------------------------------");
            sb.AppendLine();
            sb.Append(sb2);
            _gravarResultados(sb, name);
        }

        private static void _escrever(BuscaComidas buscaComidas, StringBuilder sb)
        {   
            sb.AppendLine(buscaComidas.PopulacaoAtual[0].ToString());
            sb.AppendLine(buscaComidas.PopulacaoAtual[1].ToString());
            sb.AppendLine(buscaComidas.PopulacaoAtual[2].ToString());
        }

        private static void _gravarResultados(StringBuilder sb, string name)
        {
            string dir = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\testes\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = dir + name+".txt";
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
