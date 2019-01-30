using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NAudio.Wave;

namespace GraficadorSenales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        double amplitudMaxima = 1;

        Senal senal;
        Senal senalResultado;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            AudioFileReader reader = new AudioFileReader(txtRutaArchivo.Text);

            double tiempoInicial = 0;
            double tiempoFinal = reader.TotalTime.TotalSeconds;
            double frecMuestreo = reader.WaveFormat.SampleRate;

            txtFrecMuestreo.Text = frecMuestreo.ToString();
            txtTiempoInicial.Text = "0";
            txtTiempoFinal.Text = tiempoFinal.ToString();

            senal = new SenalPersonalizada();

            senal.TiempoInicial = tiempoInicial;
            senal.TiempoFinal = tiempoFinal;
            senal.FrecMuestreo = frecMuestreo;

            //Construye nuestra señal a través del archivo de audio.
            var bufferLectura = new float[reader.WaveFormat.Channels];
            int muestrasLeidas = 1;
            double instanteActual = 0;
            double intervaloMuestra = 1.0 / frecMuestreo;

            do
            {
                muestrasLeidas = reader.Read(bufferLectura, 0, reader.WaveFormat.Channels);

                if (muestrasLeidas > 0)
                {
                    double max = bufferLectura.Take(muestrasLeidas).Max();

                    senal.Muestras.Add(new Muestra(instanteActual, max));
                }

                instanteActual += intervaloMuestra;

            } while (muestrasLeidas > 0);

            //Establecer amplitud máxima.
            senal.actualizarAmplitudMaxima();
            amplitudMaxima = senal.amplitudMaxima;

            //Limpia las gráficas.
            plnGrafica.Points.Clear();

            lblAmplitudMaximaY.Text = amplitudMaxima.ToString("F");
            lblAmplitudMaximaY_Negativa.Text = "-" + amplitudMaxima.ToString("F");

            //Graficando PRIMERA señal.
            if (senal != null)
            {
                //Recorrer una colección o arreglo.
                foreach (Muestra muestra in senal.Muestras)
                {
                    plnGrafica.Points.Add(new Point((muestra.x - tiempoInicial) * scrContenedor.Width,
                        (muestra.y / amplitudMaxima) * ((scrContenedor.Height / 2.0) - 30) * -1 + (scrContenedor.Height / 2)));
                }
            }
       
            //Graficando el eje de X
            plnEjeX.Points.Clear();
            //Punto de inicio.
            plnEjeX.Points.Add(new Point(0, (scrContenedor.Height / 2)));
            //Punto de fin.
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, (scrContenedor.Height / 2)));

            //Graficando el eje de Y
            plnEjeY.Points.Clear();
            //Punto de inicio.
            plnEjeY.Points.Add(new Point(0 - tiempoInicial * scrContenedor.Width, scrContenedor.Height));
            //Punto de fin.
            plnEjeY.Points.Add(new Point(0 - tiempoInicial * scrContenedor.Width, scrContenedor.Height * -1));
        }

        private void btnFourier_Click(object sender, RoutedEventArgs e)
        {
            Senal transformada = Senal.transformar(senal);
            transformada.actualizarAmplitudMaxima();

            //Limpia las gráficas.
            plnGraficaResultado.Points.Clear();

            lblAmplitudMaximaY_Copy.Text = transformada.amplitudMaxima.ToString("F");
            lblAmplitudMaximaY_Negativa_Copy.Text = "-" + transformada.amplitudMaxima.ToString("F");

            //Graficando PRIMERA señal.
            if (transformada != null)
            {
                //Recorrer una colección o arreglo.
                foreach (Muestra muestra in transformada.Muestras)
                {
                    //Se va graficando la transformada.
                    plnGraficaResultado.Points.Add(new Point((muestra.x - transformada.TiempoInicial) * scrResultadoOperacion.Width,
                        (muestra.y / transformada.amplitudMaxima) * ((scrResultadoOperacion.Height / 2.0) - 30) * -1 + (scrResultadoOperacion.Height / 2)));
                }

                double valorMaximo = 0;
                int indiceMaximo = 0;
                int indiceActual = 0;

                //Recorrer una colección o arreglo.
                foreach (Muestra muestra in transformada.Muestras)
                {
                    //Buscamos el valor máximo y el indice actual.
                    if (muestra.y > valorMaximo)
                    {
                        valorMaximo = muestra.y;
                        indiceMaximo = indiceActual;
                    }

                    indiceActual++;

                    //Evaluamos en la primera mitad de la gráfica de la transformada.
                    if (indiceActual > (double)transformada.Muestras.Count/2.0)
                    {
                        break;
                    }
                }

                double frecuenciaFundamental = (double)indiceMaximo * senal.FrecMuestreo / (double)transformada.Muestras.Count;
                lblHz.Text = frecuenciaFundamental.ToString() + " Hz";


                double indiceMax = (frecuenciaFundamental * senal.FrecMuestreo) / (double)transformada.Muestras.Count;
                lbl_indiceMax.Text = indiceMax.ToString() + " índice máximo";

                for (int i = 0; i < length; i++)
                {

                }
            }

            //Graficando el eje de X
            plnEjeXResultado.Points.Clear();
            //Punto de inicio.
            plnEjeXResultado.Points.Add(new Point(0, (scrResultadoOperacion.Height / 2)));
            //Punto de fin.
            plnEjeXResultado.Points.Add(new Point((transformada.TiempoFinal - transformada.TiempoInicial) * scrResultadoOperacion.Width, (scrResultadoOperacion.Height / 2)));

            //Graficando el eje de Y
            plnEjeYResultado.Points.Clear();
            //Punto de inicio.
            plnEjeYResultado.Points.Add(new Point(0 - transformada.TiempoInicial * scrResultadoOperacion.Width, scrResultadoOperacion.Height));
            //Punto de fin.
            plnEjeYResultado.Points.Add(new Point(0 - transformada.TiempoInicial * scrResultadoOperacion.Width, scrResultadoOperacion.Height * -1));
        }

        private void btn_Examinar(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if ( (bool) fileDialog.ShowDialog() )
            {
                txtRutaArchivo.Text = fileDialog.FileName;
            }
            
        }
    }
}
