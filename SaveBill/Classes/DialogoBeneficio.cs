using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;

namespace SaveBill
{
    class DialogoBeneficio : DialogFragment
    {
        Spinner sp;

        public static DialogoBeneficio NewInstance()
        {
            return new DialogoBeneficio();
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreateDialog(savedInstanceState);

            //Cria o dialogo
            var builder = new AlertDialog.Builder(Activity);

            //Pega o layout inflater
            var inf = Activity.LayoutInflater;

            //Infla o o layout do dialogo
            var lay = inf.Inflate(Resource.Layout.dialogo_beneficio, null);

            if (lay != null)
            {
                sp = lay.FindViewById<Spinner>(Resource.Id.spBeneficio);
                String[] str = { "Residencia Sem Beneficio", "Residencia BR", "Residencia Rural" };

                ArrayAdapter<String> adp = new ArrayAdapter<String>(Activity, 
                                                            Android.Resource.Layout.SimpleSpinnerItem, str);

                sp.Adapter = adp;

                sp.ItemSelected += (sender, e) =>
                {
                    VarUsadas.BeneficiosKwh = (int)e.Id;
                };
            }

            builder.SetView(lay);
            builder.SetPositiveButton("OK", (sender, item) => 
            {
                var dialog = (AlertDialog)sender;
                //Fecha o dialogo
                dialog.Dismiss();
            });
            builder.SetNegativeButton("Cancel", (sender, item) => 
            {
                var dialog = (AlertDialog)sender;
                //Fecha o dialogo
                dialog.Dismiss();
            });

            //Cria o dialogo
            var dialogo = builder.Create();

            return dialogo;
        }
        
    }
}