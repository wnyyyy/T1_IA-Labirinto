namespace T1_IA
{
    class Program
    {
        static void Main(string[] args)
        {
            Labirinto labirinto = new Labirinto("labirinto1.txt");

            int coefTamanhoPopulacao = 10;
            int taxaMutacao = 100; // 0 ~ 100
            int elitismo = 10; // 0 ~ 100
            BuscaComidas buscaComidas = new BuscaComidas(labirinto, coefTamanhoPopulacao, taxaMutacao, elitismo);
            for (int i = 0; i < 100; i++)
            {
                buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
                buscaComidas.NovaGeracao();
            }
        }
    }
}