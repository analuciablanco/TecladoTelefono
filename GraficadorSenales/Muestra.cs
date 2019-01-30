using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSenales
{
    class Muestra
    {
        //El instante en que fue tomada la muestra.
        public double x { get; set; }
        //El valor de esa muestra en ese instante.
        public double y { get; set; }

        //Constructor sin parámetros
        public Muestra()
        {
            x = 0.0;
            y = 0.0;
        }

        //Constructor que inicializa valores.
        public Muestra(double X, double Y)
        {
            x = X;
            y = Y;
        }
    }
}
