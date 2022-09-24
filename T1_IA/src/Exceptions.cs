using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1_IA
{
    public class LabirintoArquivoNaoEncontrado : Exception
    {
        public LabirintoArquivoNaoEncontrado() { }
    }

    public class LabirintoDimensaoInvalida : Exception
    {
        public LabirintoDimensaoInvalida() { }
    }

    public class LabirintoTabelaCampoInvalido : Exception
    {
        public LabirintoTabelaCampoInvalido() { }
    }

    public class LabirintoTabelaFormatoInvalido : Exception
    {
        public LabirintoTabelaFormatoInvalido() { }
    }

    public class LabirintoTabelaSemEntrada : Exception
    {
        public LabirintoTabelaSemEntrada() { }
    }

    public class LabirintoTabelaSemComida : Exception
    {
        public LabirintoTabelaSemComida() { }
    }
}
