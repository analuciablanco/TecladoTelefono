#region // all using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
#endregion // all using

namespace GraficadorSenales
{
    abstract class Senal
    {
        public List<Muestra> Muestras { get; set; }

        public double amplitudMaxima { get; set; }
        public double TiempoInicial { get; set; }
        public double TiempoFinal { get; set; }
        public double FrecMuestreo { get; set; }

        public abstract double evaluar(double tiempo);

        public void construirSenalDigital()
        {
            double periodoMuestreo = 1 / FrecMuestreo;

            for (double i = TiempoInicial; i <= TiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra = evaluar(i);

                if (Math.Abs(valorMuestra) > amplitudMaxima)
                {
                    amplitudMaxima = Math.Abs(valorMuestra);
                }

                //se van añadiendo las muestras a la lista.
                Muestras.Add(new Muestra(i, valorMuestra));
            }
        }

        public void escalar(double factor)
        {
            //por cara muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //se multiplica por Y para escalar, no por X para conservar el instante de tiempo
                muestra.y *= factor;
            }
        }

        public void actualizarAmplitudMaxima()
        {
            amplitudMaxima = 0;

            foreach (Muestra muestra in Muestras)
            {
                if (Math.Abs(muestra.y) > amplitudMaxima)
                {
                    amplitudMaxima = Math.Abs(muestra.y);
                }
            }
        }

        public void desplazar(double factor)
        {
            //por cara muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //se suma en Y para desplazar, no por X para conservar el instante de tiempo
                muestra.y += factor;
            }
        }

        public void truncar(double umbral)
        {
            //por cara muestra se va a realizar esto
            foreach (Muestra muestra in Muestras)
            {
                //
                if (muestra.y > umbral) muestra.y = umbral;
                else if (muestra.y < (umbral * -1)) muestra.y = umbral * -1;
            }
        }

        public static Senal sumar(Senal suma1, Senal suma2)
        {
            //construimos la señal resultado
            SenalPersonalizada resultado = new SenalPersonalizada ();
            //sumamos muestra por muestra
            resultado.TiempoInicial = suma1.TiempoInicial;
            resultado.TiempoFinal = suma1.TiempoFinal;
            resultado.FrecMuestreo = suma1.FrecMuestreo;
            //recorremos 1 lista de muestras y a la 2 señal accedemos por un indice
            int indice = 0;
            foreach (Muestra muestra in suma1.Muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.x = muestra.x;
                muestraResultado.y = muestra.y + suma2.Muestras[indice].y;
                indice++;
                resultado.Muestras.Add(muestraResultado);
            }
            return resultado;
        }

        public static Senal multiplicar(Senal multiplicacion1, Senal multiplicacion2)
        {
            //construimos la señal resultado
            SenalPersonalizada resultado = new SenalPersonalizada();
            //sumamos muestra por muestra
            resultado.TiempoInicial = multiplicacion1.TiempoInicial;
            resultado.TiempoFinal = multiplicacion1.TiempoFinal;
            resultado.FrecMuestreo = multiplicacion1.FrecMuestreo;
            //recorremos 1 lista de muestras y a la 2 señal accedemos por un indice
            int indice = 0;
            foreach (Muestra muestra in multiplicacion1.Muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.x = muestra.x;
                muestraResultado.y = muestra.y * multiplicacion2.Muestras[indice].y;
                indice++;
                resultado.Muestras.Add(muestraResultado);
            }
            return resultado;
        }

        public static Senal convolucionar(Senal operando1, Senal operando2)
        {
            SenalPersonalizada resultado = new SenalPersonalizada();
            resultado.TiempoInicial = operando1.TiempoInicial + operando2.TiempoInicial;
            resultado.TiempoFinal = operando1.TiempoFinal + operando2.TiempoFinal;
            resultado.FrecMuestreo = operando1.FrecMuestreo;

            double periodoMuestreo = 1 / resultado.FrecMuestreo;
            double duracionSenal = resultado.TiempoFinal - resultado.TiempoInicial;
            double cantidadMuestrasResultado = duracionSenal * resultado.FrecMuestreo;

            double instanteActual = resultado.TiempoInicial;
            for(int n=0; n<cantidadMuestrasResultado; n++)
            {
                double valorMuestra = 0;
                for(int k=0; k<operando2.Muestras.Count; k++)
                {
                    if((n-k) >= 0 && (n-k) < operando2.Muestras.Count)
                    {
                        valorMuestra += operando1.Muestras[k].y * operando2.Muestras[n - k].y;
                    }  
                }

                valorMuestra /= resultado.FrecMuestreo;
                //La linea de arriba es lo mismo que: valorMuestra = valorMuestra / resultado.FrecMuestreo;
                Muestra muestra = new Muestra(instanteActual, valorMuestra);
                resultado.Muestras.Add(muestra);
                instanteActual += periodoMuestreo;
            }

            return resultado;
        }

        public static Senal transformar(Senal senal)
        {
            SenalPersonalizada transformada = new SenalPersonalizada();
            transformada.TiempoInicial = senal.TiempoInicial;
            transformada.TiempoFinal = senal.TiempoFinal;
            transformada.FrecMuestreo = senal.FrecMuestreo;

            for(int k=0; k < senal.Muestras.Count; k++)
            {
                Complex muestra = 0;

                for(int n=0; n<senal.Muestras.Count; n++)
                {
                    muestra += senal.Muestras[n].y * Complex.Exp(-2 * Math.PI * Complex.ImaginaryOne * k * n / senal.Muestras.Count);
                }

                transformada.Muestras.Add(new Muestra((double)k/(double)senal.Muestras.Count, muestra.Magnitude));
            }

            return transformada;
        }
    }
}
