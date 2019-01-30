using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSenales
{
    class SenalRectangular : Senal
    {
        public SenalRectangular()
        {
            amplitudMaxima = 0.0;
            Muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;

            if (Math.Abs(tiempo) == 0.5)
            {
                resultado = 0.5;
            }
            else if (Math.Abs(tiempo) < 0.5)
            {
                resultado = 1;
            }
            else resultado = 0;

            return resultado;
        }
    }
}
