using T1_IA.AStar;

namespace T1_IA
{
    class Program
    {
        static void Main(string[] args)
        {
            Labirinto labirinto = new Labirinto("labirinto1.txt");

            //int coefTamanhoPopulacao = 10;
            //int taxaMutacao = 25; // 0 ~ 100
            //int agressividadeMutacao = 100; // em %. quanto maior, mais comidas coletadas mas maior rota
            //int elitismo = 20; // 0 ~ 100
            //BuscaComidas buscaComidas = new BuscaComidas(labirinto, coefTamanhoPopulacao, taxaMutacao, agressividadeMutacao, elitismo);
            //for (int i = 0; i < 100; i++)
            //{
            //    buscaComidas.NovaGeracao();
            //    foreach (Agente agente in buscaComidas.PopulacaoAtual)
            //    {
            //        Console.WriteLine(Util.ValidaCaminho(agente.Rota));
            //    }
            //}
            //Teste.Testar();
            Tela tela = new Tela();
        }
    }
}