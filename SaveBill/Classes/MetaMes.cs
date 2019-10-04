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
using Newtonsoft.Json;
using SQLite;

namespace SaveBill
{
    [Activity(Label = "ECCOM")]
    public class MetaMes : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.modo_economia);

            CorFundoEconomia();

            EditText txMeta = FindViewById<EditText>(Resource.Id.txMetaMes);
            Button btModoEcon = FindViewById<Button>(Resource.Id.btModoEconomia);

            if(VarUsadas.ModoEconomia)
            {
                txMeta.Enabled = false;
                btModoEcon.Text = "Desativar Modo Economia";
            }

            btModoEcon.Click += (sender, e) =>
            {
                //Se ja esta no modo economia
                //Remove esse modo
                if (VarUsadas.ModoEconomia)
                {
                    txMeta.Enabled = true;
                    btModoEcon.Text = "Iniciar Modo Economia";
                    txMeta.Hint = "Meta Mensal (R$)";
                    VarUsadas.ModoEconomia = false;
                    VarUsadas.MetaMes = 0;
                    VarUsadas.BandeiraEconomia = VarUsadas.ECONOM_NENHUM;
                    CorFundoEconomia();
                }
                else
                {
                    //Testa se tem valor
                    if (txMeta.Text != "")
                    {
                        VarUsadas.LigaEconomia((float)Convert.ToDouble(txMeta.Text));

                        Log.Debug("Variaveis", "Valor atual = " + VarUsadas.ValorAtual);
                        Log.Debug("Variaveis", "Cor = " + VarUsadas.BandeiraEconomia);
                        Log.Debug("Variaveis", "Meta MEs = " + VarUsadas.MetaMes);

                        txMeta.Enabled = false;
                        txMeta.Text = "";
                        txMeta.Hint = "";
                        btModoEcon.Text = "Desativar Modo Economia";
                        CorFundoEconomia();

                        Toast.MakeText(this, "Modo de Economia Ativo!", ToastLength.Long).Show();
                    }
                }

                ModificaBancoDados();
                    
            };
        }

        void CorFundoEconomia()
        {
            var layout = FindViewById<LinearLayout>(Resource.Id.layMetaMes);

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

        void ModificaBancoDados()
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
                obj.MetaMes = VarUsadas.MetaMes;
                obj.ModoEconomia = VarUsadas.ModoEconomia;
                obj.BandeiraEconomia = VarUsadas.BandeiraEconomia;
                //Atualiza a tabela
                con.Update(obj);

                con.Close();
            }
            catch (SQLiteException ex)
            {
                Log.Debug("Erro Sqlite", ex.Message);
            }
        }
    }
}