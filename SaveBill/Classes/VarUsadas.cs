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
using Newtonsoft.Json;
using Android.Util;
//using Java.IO;

namespace SaveBill
{
    public static class VarUsadas
    {
        //Baneiras
        public static readonly int BANDEIRA_VERDE = 0;
        public static readonly int BANDEIRA_AMARELO = 1;
        public static readonly int BANDEIRA_VERMELHO_1 = 2;
        public static readonly int BANDEIRA_VERMELHO_2 = 2;

        //Servicos
        public static readonly int SERVICO_ENERGIA = 0;
        public static readonly int SERVICO_AGUA = 1;

        //Empresas de energia
        public static readonly int DISTRI_ENERGISA = 0;

        //Empresa de agua
        public static readonly int DISTRI_AGUA = 0;

        //Kwh
        public static readonly int RESI_SEM_BENEFICIO = 0;
        public static readonly int RESI_BR = 1;
        public static readonly int RESI_RURAL = 2;

        public static readonly int ECONOM_VERDE = 0;
        public static readonly int ECONOM_AMARELO = 1;
        public static readonly int ECONOM_VERMELHO = 2;
        public static readonly int ECONOM_NENHUM = 3;

        public static DateTime dataInicialMes { get; set; }
        public static DateTime dataFinalMes { get; set; }
        
        //Taxas

        //Configuração total
        //public static int Servico { get; set; }
        public static int Distribuidora { get; set; }

        public static int BeneficiosKwh { get; set; }

        //Valore e taxas
        public static float ValorAtual { get; set; }
        public static float Bandeira { get; set; }
        public static float Pis { get; set; }
        public static float Cofins { get; set; }
        public static float Icms { get; set; }
        public static int Kwh { get; set; }
        public static float ValorKwh { get; set; }

        public static float MetaMes { get; set; }
        public static bool ModoEconomia { get; set; }
        //Vandeiras geradas pelo sistema do app
        public static int BandeiraEconomia { get; set; }

        //Lista com as leituras
        public static List<int> ListaLeituras = new List<int>();

        public static void ResetaVariaveis()
        {
            //Servico = SERVICO_ENERGIA;
            Distribuidora = DISTRI_ENERGISA;
            BeneficiosKwh = RESI_SEM_BENEFICIO;

            Bandeira = BANDEIRA_VERDE;
            //Taxas Contantes
            //Mes maio
            Pis = 0.57f;
            Cofins = 2.61f;

            Icms = 0.0000f;
            Kwh = 0;

            ValorAtual = 0.00f;

            MetaMes = 0;
            ModoEconomia = false;
            BandeiraEconomia = ECONOM_NENHUM;

            ListaLeituras.Clear();

            dataInicialMes = DateTime.Now;
            dataFinalMes = new DateTime(dataInicialMes.Year, dataInicialMes.Month + 1, dataInicialMes.Day);
        }

        public static bool SalvaBancoDados(String path)
        {
            return true;
        }

        public static bool CarregaBancoDados(String path)
        {
            return true;
        }

        public static bool SetaBancoDados(BancoDados bd)
        {
            if (bd == null)
                return false;

            bd.DataInicialMes = dataInicialMes;
            bd.DataFinalMes = dataFinalMes;
            bd.Distribuidora = Distribuidora;
            bd.BeneficiosKwh = BeneficiosKwh;
            bd.ValorAtual = ValorAtual;
            bd.Bandeira = Bandeira;
            bd.Pis = Pis;
            bd.Cofins = Cofins;
            bd.Icms = Icms;
            bd.Kwh = Kwh;
            bd.ValorKwh = ValorKwh;
            bd.MetaMes = MetaMes;
            bd.ModoEconomia = ModoEconomia;
            bd.BandeiraEconomia = BandeiraEconomia;
            bd.ListaLeituras = JsonConvert.SerializeObject(ListaLeituras);

            return true;
        }

        public static bool PegaBancoDados(BancoDados db)
        {
            if (db == null)
                return false;

            dataInicialMes = db.DataInicialMes;
            dataFinalMes = db.DataFinalMes;
            Distribuidora = db.Distribuidora;
            BeneficiosKwh = db.BeneficiosKwh;
            ValorAtual = db.ValorAtual;
            Bandeira = db.Bandeira;
            Pis = db.Pis;
            Cofins = db.Cofins;
            Icms = db.Icms;
            Kwh = db.Kwh;
            ValorKwh = db.ValorKwh;
            MetaMes = db.MetaMes;
            ModoEconomia = db.ModoEconomia;
            BandeiraEconomia = db.BandeiraEconomia;
            try
            {
                ListaLeituras = JsonConvert.DeserializeObject<List<int>>(db.ListaLeituras);
            }
            catch(NullReferenceException ex)
            {
                Log.Debug("Json", ex.Message);

            }

            return true;
        }

        //Adiciona as leituras
        public static bool AdicionaLeitura( int leitura )
        {
            if (ListaLeituras.Count > 0 && leitura < ListaLeituras.Last())
                return false;

            //Adiciona o maximo de 10 leituras
            if(ListaLeituras.Count < 10)
            {
                ListaLeituras.Add(leitura);
            }
            else
            {
                ListaLeituras.Remove(0);
                ListaLeituras.Add(leitura);
            }

            //Calcula quanto d kwh foi usado ate agora
            CalculaQtdKwhMes();

            return true;
        }

        public static void SetaData( DateTime inicial, DateTime final )
        {
            dataInicialMes = inicial;
            dataFinalMes = final;
        }

        public static void CalculaCorEconomia()
        {
            if (VarUsadas.ValorAtual < VarUsadas.MetaMes / 2)
            {
                VarUsadas.BandeiraEconomia = VarUsadas.ECONOM_VERDE;
            }
            else if (VarUsadas.ValorAtual > VarUsadas.MetaMes / 2 &&
                        VarUsadas.ValorAtual < VarUsadas.MetaMes)
            {
                VarUsadas.BandeiraEconomia = VarUsadas.ECONOM_AMARELO;
            }
            else if (VarUsadas.ValorAtual > VarUsadas.MetaMes)
            {
                VarUsadas.BandeiraEconomia = VarUsadas.ECONOM_VERMELHO;
            }
        }

        public static void LigaEconomia( float meta )
        {
            VarUsadas.ModoEconomia = true;
            VarUsadas.MetaMes = meta;

            CalculaCorEconomia();
        }

        public static void CalculaValorAtual()
        {
            //Calcula a qtd d Kwh
            //CalculaQtdKwhMes();

            if(Kwh <= 0)
                return;

            CalculaValorKwh();

            //Calcula os valores
            ValorAtual = Kwh * ValorKwh;

            //Calcula as taxas
            ValorAtual = CalculaIcms(ValorAtual);

            //Valor final
            ValorAtual += (CalculaBandeiras() + Pis + Cofins);
        }

        static void CalculaQtdKwhMes()
        {
            if (ListaLeituras.Count < 2)
                return;

            int ultimaLeitura = ListaLeituras[ListaLeituras.Count - 1];
            int penultimaLeitura = ListaLeituras[ListaLeituras.Count - 2];

            //Testa se ja passou um mes
            if (dataInicialMes >= dataFinalMes)
            {
                //Comeca a contar outra vez
                dataInicialMes = dataFinalMes;
                Kwh = 0;
            }

            Kwh += (ultimaLeitura - penultimaLeitura < 0) ? 0 : ultimaLeitura - penultimaLeitura;
        }

        static float CalculaIcms( float contaAtual )
        {
            float valorAtualizado = contaAtual;

            if(BeneficiosKwh == RESI_SEM_BENEFICIO || BeneficiosKwh == RESI_BR)
            {
                if(Kwh > 51 && Kwh < 101)
                {
                    valorAtualizado *= 1.25f;
                }    
                else if(Kwh > 101)
                {
                    valorAtualizado *= 1.27f;
                }
            }

            return valorAtualizado;
        }

        static float CalculaBandeiras()
        {
            float valorFinal = 0.000f;

            if(Bandeira == BANDEIRA_AMARELO)
            {
                valorFinal = Kwh * 0.015f;
            }
            else if(Bandeira == BANDEIRA_VERMELHO_1)
            {
                valorFinal = Kwh * 0.030f;
            }
            else if (Bandeira == BANDEIRA_VERMELHO_2)
            {
                valorFinal = Kwh * 0.045f;
            }

            return valorFinal;
        }

        static void CalculaValorKwh()
        {
            if (BeneficiosKwh == RESI_SEM_BENEFICIO)
                ValorKwh = 0.41817f;
            else if (BeneficiosKwh == RESI_BR)
            {
                if(Kwh < 31)
                {
                    ValorKwh = 0.14463f;
                }
                else if (Kwh > 30 && Kwh < 101)
                {
                    ValorKwh = 0.24794f;
                }
                else if (Kwh > 100 && Kwh < 221)
                {
                    ValorKwh = 0.37191f;
                }
                else if (Kwh > 220)
                {
                    ValorKwh = 0.41324f;
                }
            }
            else if(BeneficiosKwh == RESI_RURAL)
                ValorKwh = 0.29272f;
        }

        public static String[] PegaListaLeitura()
        {
            String[] str = new String[ListaLeituras.Count];

            for (int i = 0; i < ListaLeituras.Count; ++i)
            {
                str[i] = "Leitura = " + Convert.ToString(ListaLeituras[i]);
            }

            return str;
        }

        public static List<int> PegaListaKwh()
        {
            var lista = new List<int>();

            int valorBase = 0;
            int valorProximo = 0;
            int valor = 0;
            int i = 0;

            while(true)
            {
                if (ListaLeituras.Count - 1 < i + 1)
                    break;

                valorBase = ListaLeituras[i];
                valorProximo = ListaLeituras[i + 1];

                valor = valorProximo - valorBase;

                lista.Add(valor);

                i++;
            }

            return lista;
        }
    }
}