using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content.Res;
using System.IO;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Health_Shop.Droid
{
    [Activity(Label = "Health Shop", Icon = "@mipmap/HEALTHSHOP", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            LoadApplication(new App());
            Window.SetStatusBarColor(Android.Graphics.Color.Rgb(211, 72, 23));

            // Парсинг информации о ешках
            AssetManager assets = this.Assets;
            using (StreamReader streamReaderE = new StreamReader(assets.Open("EsInfo.txt")))
            {
                DataBank.EInfo = new Dictionary<string, DataModels.EInformation>();

                while (!streamReaderE.EndOfStream)
                {
                    string[] EInfo = streamReaderE.ReadLine().Split(';');

                    DataBank.EInfo.Add(EInfo[0], new DataModels.EInformation
                    {
                        Name = EInfo[1],
                        OtherNames = EInfo[2],
                        Danger = EInfo[3],
                        DangerColor = Color.FromHex(EInfo[4]),
                        Origin = EInfo[5],
                        OriginColor = Color.FromHex(EInfo[6]),
                        Category = EInfo[7],
                        GeneralIfno = EInfo[8],
                        MainParameters = EInfo[9],
                        InfluenceOnTheBody = EInfo[10],
                        Goods = EInfo[11],
                        Poor = EInfo[12],
                        Usage = EInfo[13],
                        Legislation = EInfo[14]
                    });
                }
            }
            using (StreamReader streamReaderProduct = new StreamReader(assets.Open("ProductsInfo.txt")))
            {
                DataBank.ProductInfo = new Dictionary<string, DataModels.ProductInformation>();

                while (!streamReaderProduct.EndOfStream)
                {
                    string[] ProductInfo = streamReaderProduct.ReadLine().Split(';');

                    DataBank.ProductInfo.Add(ProductInfo[0], new DataModels.ProductInformation
                    {
                        ProductTitle = ProductInfo[1],
                        ProductComposition = ProductInfo[2],
                        ProductDescription = ProductInfo[3],
                        EnergyComposition = ProductInfo[4].Split(':')
                    });
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}