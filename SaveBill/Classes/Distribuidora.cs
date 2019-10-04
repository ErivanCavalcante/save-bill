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
using SQLite;

namespace SaveBill
{
    [Activity(Label = "ECCOM", MainLauncher = true, Icon = "@drawable/icon")]
    public class Distribuidora : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Conect Banco
            ConectaBancoDados();

            // Create your application here
            SetContentView(Resource.Layout.distribuidora);

            //Prenche o spner de acordo com o servico
            Spinner sp = FindViewById<Spinner>(Resource.Id.spDistribuidora);
            ArrayAdapter<String> adp;

            //Se eh a conta de luz
            //if( VarUsadas.Servico == VarUsadas.SERVICO_ENERGIA )
            //{
                String[] nomes = { "Energisa" };
                adp = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, nomes);
            //}
            //Se eh conta de agua
            //else
            //{
            //  String[] nomes = { "Cagepa" };
            //  adp = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, nomes);
            //}

            sp.Adapter = adp;

            sp.ItemSelected += (sender, e) =>
            {
                VarUsadas.Distribuidora = (int)e.Id;

            };

            Button btSelecDist = FindViewById<Button>(Resource.Id.btSelecDist);
            //Selecao de um item
            btSelecDist.Click += (sender, e) =>
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
                    obj.Distribuidora = VarUsadas.Distribuidora;
                    //Atualiza a tabela
                    con.Update(obj);

                    con.Close();
                }
                catch (SQLiteException ex)
                {
                    Log.Debug("Erro Sqlite", ex.Message);
                }

                vaiParaPrincipal();
            };

        }

        public override void OnBackPressed()
        {
            Finish();
        }

        void ConectaBancoDados()
        {
            try
            {
                //Path do banco
                var path = System.Environment.GetFolderPath((System.Environment.SpecialFolder.Personal));
                var database = System.IO.Path.Combine(path, "db_conf.db");

                Log.Debug("Banco de Dados", System.Environment.GetFolderPath((System.Environment.SpecialFolder.Personal)));

                //Cria se nao existir a conexao
                var con = new SQLiteConnection(database);
                //Cria se nao existir a tabela
                con.CreateTable<BancoDados>();

                //Primeiro acesso a tabela
                if (con.Table<BancoDados>().Count() == 0)
                {
                    VarUsadas.ResetaVariaveis();

                    BancoDados bd = new BancoDados();

                    VarUsadas.SetaBancoDados(bd);

                    con.Insert(bd);
                }
                else
                {
                    var obj = con.Get<BancoDados>(1);
                    VarUsadas.PegaBancoDados(obj);

                    vaiParaPrincipal();
                }

                con.Close();
                Toast.MakeText(this, "Tudo certo!!!", ToastLength.Long).Show();
            }
            catch (SQLiteException ex)
            {
                Log.Debug("Erro  SQLite", ex.Message);
                Toast.MakeText(this, "Erro ao Criar o Banco" + ex.Message, ToastLength.Long).Show();
            }
        }

        private void vaiParaPrincipal()
        {
            Intent it = new Intent(this, typeof(Principal));
            
            StartActivity(it);
            Finish();
        }
    }
}