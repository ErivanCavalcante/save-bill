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
using Android.Graphics;

namespace SaveBill
{
    [Activity(Label = "ValorAtual")]
    public class ValorAtual : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.conta_atual);

            CorFundoEconomia();

            TextView tValor = FindViewById<TextView>(Resource.Id.textValorAtual);

            VarUsadas.CalculaValorAtual();

            tValor.Text = "" + VarUsadas.ValorAtual;
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        void CorFundoEconomia()
        {
            var layout = FindViewById<LinearLayout>(Resource.Id.layValorAtual);

            //Ajusta o backgroud de acordo com o modo economia
            switch (VarUsadas.BandeiraEconomia)
            {
                case 0:
                    layout.SetBackgroundColor(Color.Green);
                    break;
                case 1:
                    layout.SetBackgroundColor(Color.Yellow);
                    break;
                case 2:
                    layout.SetBackgroundColor(Color.Red);
                    break;
                case 3:
                    layout.SetBackgroundColor(Color.Black);
                    break;
            }

        }
    }
}