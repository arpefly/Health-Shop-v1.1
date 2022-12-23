using Health_Shop.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Health_Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EInfoPage : ContentPage
    {
        public EInfoPage(string code)
        {
            InitializeComponent();

            ParseEInfo(code);
        }

        private void ParseEInfo(string code)
        {
            EInformation eInfo = DataBank.EInfo.Where(x => x.Key.Contains(code)).First().Value;

            labelHeader.Text = code;
            labelName.Text = eInfo.Name;
            labelOtherNames.Text = eInfo.OtherNames;
            labelDanger.Text = eInfo.Danger;
            labelDanger.TextColor = eInfo.DangerColor;
            labelOrigin.Text = eInfo.Origin;
            labelOrigin.TextColor = eInfo.OriginColor;
            labelGeneralInformation.Text = eInfo.GeneralIfno;
            labelMainParameters.Text = string.Join("\n", eInfo.MainParameters.Split(':'));
            labelGoods.Text = eInfo.Goods;
            labelPoor.Text = eInfo.Poor;
            labelUsage.Text = eInfo.Usage;
            labelLegislation.Text = eInfo.Legislation;
        }
    }
}