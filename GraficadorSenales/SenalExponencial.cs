using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSenales
{
    class SenalExponencial : Senal
    {
        //Exclusivos de señal exponencial.
        public double alfa { get; set; }
        //

        public SenalExponencial()
        {
            alfa = 0.0;
            Muestras = new List<Muestra>();
        }

        public SenalExponencial(double Alfa)
        {
            alfa = Alfa;
            Muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;

            resultado = Math.Exp(alfa * tiempo);

            return resultado;
        }
    }
}
