using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;


namespace SaveBill
{
    [Activity(Label = "ECCOM")]
    public class Principal: Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.principal);

            //Muda as telas
            Button btLeitura = FindViewById<Button>(Resource.Id.btLeitura);
            Button btMetaMes = FindViewById<Button>(Resource.Id.btMetaMes);
            Button btValorAtual = FindViewById<Button>(Resource.Id.btValorAtual);
            Button btGraficos = FindViewById<Button>(Resource.Id.btEstatistica);
            Button btCreditos = FindViewById<Button>(Resource.Id.btCreditos);

            CorFundoEconomia();

            btLeitura.Click += (sender, e) =>
            {
                Intent it = new Intent(this, typeof(Leitura));
                StartActivity(it);
            };

            btMetaMes.Click += (sender, e) =>
            {
                Intent it = new Intent(this, typeof(MetaMes));
                StartActivity(it);
            };

            btValorAtual.Click += (sender, e) =>
            {
                Intent it = new Intent(this, typeof(ValorAtual));
                StartActivity(it);
            };

            btGraficos.Click += (sender, e) =>
            {
                Intent it = new Intent(this, typeof(Graficos));
                StartActivity(it);
            };

            btCreditos.Click += (sender, e) =>
            {
                Intent it = new Intent(this, typeof(Creditos));
                StartActivity(it);
            };

        }

        protected override void OnResume()
        {
            CorFundoEconomia();
            base.OnResume();
           
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        public override bool OnCreateOptionsMenu( IMenu menu )
        {
            MenuInflater.Inflate(Resource.Menu.menu_principal, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch(item.ItemId)
            {
                case Resource.Id.menu_beneficio:
                    {
                        var dialogo = DialogoBeneficio.NewInstance();
                        dialogo.Show(FragmentManager, "dialogo");
                    }
                    break;
                case Resource.Id.menu_dist:
                    {
                        Intent it = new Intent(this, typeof(Distribuidora));
                        StartActivity(it);
                        Finish();
                    }
                    break;
                case Resource.Id.menu_zerar:
                    VarUsadas.ResetaVariaveis();
                    //Seta o banco de dados pra zerar suas variaveis tbm
                    break;
                case Resource.Id.menu_sair:
                    Finish();
                    break;
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        void CorFundoEconomia()
        {
            var layout = FindViewById<LinearLayout>(Resource.Id.layPrincipal);

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

