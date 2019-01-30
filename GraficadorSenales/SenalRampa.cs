using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSenales
{
    class SenalRampa : Senal
    {
        public SenalRampa()
        {
            amplitudMaxima = 0.0;
            Muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;

            if(tiempo >= 0)
            {
                resultado = tiempo;
            }
            else
            {
                resultado = 0;
            }

            return resultado;
        }
    }
}
