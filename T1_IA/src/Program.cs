namespace T1_IA
{
    class Program
    {
        static void Main(string[] args)
        {
            Labirinto labirinto = new Labirinto("labirinto2.txt");

            int coefTamanhoPopulacao = 10;
            int taxaMutacao = 50; // 0 ~ 100
            int elitismo = 10; // 0 ~ 100
            BuscaComidas buscaComidas = new BuscaComidas(labirinto, coefTamanhoPopulacao, taxaMutacao, elitismo);
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));
            //buscaComidas.NovaGeracao();
            //buscaComidas.Populacao.Sort((x, y) => x.Aptidao.CompareTo(y.Aptidao));

            BuscaRota buscaRota = new BuscaRota(labirinto);
            buscaRota.Buscar((0, 0), (3, 3));
        }
    }
}