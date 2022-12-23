using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Health_Shop.DataModels;
using System.Reflection;

[assembly: ExportFont("Montserrat-Black.ttf")]
[assembly: ExportFont("Montserrat-BlackItalic.ttf")]
[assembly: ExportFont("Montserrat-Bold.ttf")]
[assembly: ExportFont("Montserrat-BoldItalic.ttf")]
[assembly: ExportFont("Montserrat-ExtraBold.ttf")]
[assembly: ExportFont("Montserrat-ExtraBoldItalic.ttf")]
[assembly: ExportFont("Montserrat-ExtraLight.ttf")]
[assembly: ExportFont("Montserrat-ExtraLightItalic.ttf")]
[assembly: ExportFont("Montserrat-Italic.ttf")]
[assembly: ExportFont("Montserrat-Light.ttf")]
[assembly: ExportFont("Montserrat-LightItalic.ttf")]
[assembly: ExportFont("Montserrat-Medium.ttf")]
[assembly: ExportFont("Montserrat-MediumItalic.ttf")]
[assembly: ExportFont("Montserrat-Regular.ttf")]
[assembly: ExportFont("Montserrat-SemiBold.ttf")]
[assembly: ExportFont("Montserrat-SemiBoldItalic.ttf")]
[assembly: ExportFont("Montserrat-Thin.ttf")]
[assembly: ExportFont("Montserrat-ThinItalic.ttf")]

[assembly: ExportFont("Panton-BlackCaps.otf")]
[assembly: ExportFont("Panton-BlackitalicCaps.otf")]
[assembly: ExportFont("Panton-LightCaps.otf")]
[assembly: ExportFont("Panton-LightitalicCaps.otf")]
namespace Health_Shop
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
