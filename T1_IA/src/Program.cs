namespace T1_IA
{
    class Program
    {
        static void Main(string[] args)
        {
            Labirinto labirinto = new Labirinto("labirinto1.txt");

            int coefTamanhoPopulacao = 10;
            int chanceMutacao = 50; // 0 ~ 100
            int elitismo = 10; // 0 ~ 100
            BuscaComidas buscaComidas = new BuscaComidas(labirinto, coefTamanhoPopulacao, chanceMutacao, elitismo);
        }
    }
}