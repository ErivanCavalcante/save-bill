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
using SQLite;


namespace SaveBill
{
    [Table("conf_db")]
    public class BancoDados
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public bool PrimeiraVez { get; set; }

        public int Distribuidora { get; set; }

        public int BeneficiosKwh { get; set; }

        //Valore e taxas
        public float ValorAtual { get; set; }
        public float Bandeira { get; set; }
        public float Pis { get; set; }
        public float Cofins { get; set; }
        public float Icms { get; set; }
        public int Kwh { get; set; }
        public float ValorKwh { get; set; }

        public float MetaMes { get; set; }
        public bool ModoEconomia { get; set; }
        //Vandeiras geradas pelo sistema do app
        public int BandeiraEconomia { get; set; }

        public String ListaLeituras { get; set; }

       public DateTime DataInicialMes { get; set; }
       public DateTime DataFinalMes { get; set; }
    }
}