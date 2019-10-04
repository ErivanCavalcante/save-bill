using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;

namespace SaveBill
{
    [Activity(Label = "ECCOM")]
    public class Graficos : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.estatistica_uso);

            PlotView plotView = FindViewById<PlotView>(Resource.Id.pvGraficos);

            plotView.Model = CriaPlotModel();
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        PlotModel CriaPlotModel()
        {
            var plot = new PlotModel { Title = "Grafico de Consumo (Kwh)" };

            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 1000 });

            var serie = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            var lista = VarUsadas.PegaListaKwh();

            if (lista.Count != 0)
            {
                for (int i = 0; i < lista.Count; ++i)
                    serie.Points.Add(new DataPoint(i, lista[i]));
            }

            plot.Series.Add(serie);

            return plot;
        }
    }
}