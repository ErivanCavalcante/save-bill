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
using Android.Util;
using Newtonsoft.Json;

namespace SaveBill
{
    [Activity(Label = "ECCOM")]
    public class SelecaoServico : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.selecao_servico);

            VarUsadas.ResetaVariaveis();

            ImageView btContaLuz = FindViewById<ImageView>(Resource.Id.btContaLuz);
            ImageView btContaAgua = FindViewById<ImageView>(Resource.Id.btContaAgua);
            btContaLuz.Click += (sender, ev) =>
            {
                vaiParaDistribuidora(VarUsadas.SERVICO_ENERGIA);
            };
            btContaAgua.Click += (sender, ev) =>
            {
                vaiParaDistribuidora(VarUsadas.SERVICO_AGUA);
            };
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        void vaiParaDistribuidora( int servico )
        {
            Intent it = new Intent(this, typeof(Distribuidora));
            //Coloca o tipo de servico
            //0 = Luz
            //1 = agua
            //VarUsadas.Servico = servico;
          
            StartActivity(it);
            Finish();
        }
    }
}