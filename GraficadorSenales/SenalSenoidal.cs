using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSenales
{
    class SenalSenoidal : Senal
    {
        // Exclusivos de señal senoidal.
        public double amplitud { get; set; }
        public double fase { get; set; }
        public double frecuencia { get; set; }
        //

        public SenalSenoidal()
        {
            amplitud = 1.0;
            fase = 0.0;
            frecuencia = 1.0;
            amplitudMaxima = 0.0;
            Muestras = new List<Muestra>();
        }

        public SenalSenoidal(double Amplitud, double Fase, double Frecuencia)
        {
            amplitud = Amplitud;
            fase = Fase;
            frecuencia = Frecuencia;
            amplitudMaxima = 0.0;
            Muestras = new List<Muestra>();
        }

        public override double evaluar(double tiempo)
        {
            double resultado;
            resultado = amplitud * Math.Sin((2 * Math.PI * tiempo * frecuencia) + fase);

            return resultado;
        }
    }
}
