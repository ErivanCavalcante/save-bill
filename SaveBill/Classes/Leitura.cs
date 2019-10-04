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
using Android.Util;
using SQLite;
using Newtonsoft.Json;

namespace SaveBill
{
    [Activity(Label = "ECCOM")]
    public class Leitura : Activity
    {
        LinearLayout layout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.pega_leitura);

            layout = FindViewById<LinearLayout>(Resource.Id.layLeitura);

            AtualizaLista();

            CorFundoEconomia();

            Button btLeitura = FindViewById<Button>(Resource.Id.btSalvarLeitura);
            EditText txLeitura = FindViewById<EditText>(Resource.Id.txLeitura);
            btLeitura.Click += (sender, e) =>
            {
                if(txLeitura.Text != "")
                {
                    int valor = Convert.ToInt32(txLeitura.Text);

                    if(valor >= 0)
                    {
                        if (!VarUsadas.AdicionaLeitura(valor))
                        {
                            txLeitura.Text = "";
                            Toast.MakeText(this, "Leitura Incorreta.", ToastLength.Long).Show();
                            return;
                        }

                        //Recria a lista
                        AtualizaLista();

                        txLeitura.Text = "";

                        //Calcula o valor
                        VarUsadas.CalculaValorAtual();
                        //Testa se tem modo de economia
                        if(VarUsadas.ModoEconomia)
                        {
                            VarUsadas.CalculaCorEconomia();
                            CorFundoEconomia();
                        }
                    }
                }
                else
                {
                    Toast.MakeText(this, "Digite um valor para a leitura.", ToastLength.Long).Show();
                }
                
            };
        }

        public override void OnBackPressed()
        {
            try
            {
                var path = System.Environment.GetFolderPath((System.Environment.SpecialFolder.Personal));
                var database = System.IO.Path.Combine(path, "db_conf.db");

                Log.Debug("Banco de Dados", System.Environment.GetFolderPath((System.Environment.SpecialFolder.Personal)));

                //Cria se nao existir a conexao
                var con = new SQLiteConnection(database);

                //Pega a tabela
                var obj = con.Get<BancoDados>(1);
                //Ajusta o valor
                obj.ValorAtual = VarUsadas.ValorAtual;
                obj.ListaLeituras = JsonConvert.SerializeObject(VarUsadas.ListaLeituras);
                //Atualiza a tabela
                con.Update(obj);

                con.Close();
            }
            catch (SQLiteException ex)
            {
                Log.Debug("Erro Sqlite", ex.Message);
            }

            Finish();
        }

        void AtualizaLista()
        {
            if (VarUsadas.ListaLeituras.Count == 0)
                return;

            String[] str = VarUsadas.PegaListaLeitura();

            ListView lvLeituras = FindViewById<ListView>(Resource.Id.listUltimasLeituras);
            ArrayAdapter<String> adp = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, str);

            lvLeituras.Adapter = adp;
        }

        void CorFundoEconomia()
        {
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