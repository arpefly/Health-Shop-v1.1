using Health_Shop.DataModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

using ZXing.Net.Mobile.Forms;

namespace Health_Shop
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        Label Label = new Label
        {
            Text = "Пищевых добавок не обнаружено",
            TextColor = Color.White,
            BackgroundColor = Color.FromHex("#9CBA25"),
            Padding = new Thickness(10, 0, 10, 0),
            HeightRequest = 50,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };


        private async void ButtonScan_Clicked(object sender, EventArgs e)
        {
            if (entrySearch.Text == "" || entrySearch.Text == null)
            {
                ZXingScannerPage scanPage = new ZXingScannerPage();
                scanPage.OnScanResult += (result) => {
                    scanPage.IsScanning = false;
                    Device.BeginInvokeOnMainThread(async () => {
                        await Navigation.PopModalAsync();

                        if (DataBank.ProductInfo.ContainsKey(result.Text))
                        {
                            DisplayProductCompositionResults(DataBank.ProductInfo[result.Text].ProductTitle, DataBank.ProductInfo[result.Text].ProductComposition);
                            DisplayEsInfo(DataBank.ProductInfo[result.Text].ProductComposition);
                            return;
                        }
                        (string title, string productComposition) = ParseSite(result.Text);
                        DisplayProductCompositionResults(title, productComposition);
                        DisplayEsInfo(productComposition);
                    });
                };
                await Navigation.PushModalAsync(scanPage);
            }   // По штрих коду (камера)
            else if (Regex.IsMatch(entrySearch.Text, @"^\d{8}") || Regex.IsMatch(entrySearch.Text, @"^\d{13}") || Regex.IsMatch(entrySearch.Text, @"^\d{14}"))
            {
                if (DataBank.ProductInfo.ContainsKey(entrySearch.Text))
                {
                    DisplayProductCompositionResults(DataBank.ProductInfo[entrySearch.Text].ProductTitle, DataBank.ProductInfo[entrySearch.Text].ProductComposition);
                    DisplayEsInfo(DataBank.ProductInfo[entrySearch.Text].ProductComposition);
                    return;
                }
                (string title, string productComposition) = ParseSite(entrySearch.Text);
                DisplayProductCompositionResults(title, productComposition);
                DisplayEsInfo(productComposition);
            }   // По штрих коду (текст)
        }

        private (string title, string productCompositions) ParseSite(string barCode)
        {
            #region Parsing
            HtmlDocument docSearch = new HtmlWeb().Load($"https://xn----7sbabas4ajkhfocclk9d3cvfsa.xn--p1ai/search/?q={barCode}&type=goods");
            HtmlNode desiredProductLink = docSearch.DocumentNode.SelectSingleNode("/html/body/section/section[2]/div[1]/div/section/div/div[2]/div/div/div[1]/div[1]/a");

            if (desiredProductLink == null)
                return ("Продукта нет в базе данных.", "");

            string productLink = desiredProductLink.Attributes["href"].Value.Replace("https://национальный-каталог.рф", "https://xn----7sbabas4ajkhfocclk9d3cvfsa.xn--p1ai");
            HtmlDocument product = new HtmlWeb().Load(productLink);

            string title = Regex.Replace(product.DocumentNode.SelectSingleNode("//title").InnerText, "\\(\\d{6,14}\\) \\| национальный-каталог\\.рф", "");
            var productСomposition = product.DocumentNode.SelectSingleNode("//*[@id='collapseOne']/div/div/div").ChildNodes.Where(node => node.InnerText.Contains("Состав")).First();
            if (productСomposition == null)
                return ("", "");
            string productCompositions = productСomposition.ChildNodes[3].ChildNodes[1].ChildNodes[3].InnerText.Replace("\n", "").Replace("  ", "");
            #endregion  // Парсинг и перекордирока кода сайта в кириллицу

            return (title, productCompositions.Replace("&amp;", "&"));
        }
        private void DisplayProductCompositionResults(string title, string productCompositions)
        {
            labelProductTitle.Text = title;
            labelProductCompositions.Text = productCompositions;
        }
        private void DisplayEsInfo(string productCompositions)
        {
            StackLayoutMainInfo.Children.Where(x => x.GetType() == typeof(Grid)).ToList().ForEach(x => StackLayoutMainInfo.Children.Remove(x));


            foreach (string line in NormalzeProductCompositions(productCompositions))
            {
                if (!DataBank.EInfo.Keys.Contains(line))
                    continue;

                Grid gridEInfo = new Grid()
                {
                    BackgroundColor = Color.FromHex("#333333"),
                    Margin = new Thickness(10, 0, 10, 0),
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = 70 },
                        new ColumnDefinition { Width = GridLength.Star }
                    },
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                Label labelECode = new Label
                {
                    Text = line,
                    TextColor = Color.White,
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontFamily = "Montserrat-Medium.ttf"
                };              //
                gridEInfo.Children.Add(labelECode, 0, 0);   // Код ешки
                Grid.SetRowSpan(labelECode, 3);             //

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer() { NumberOfTapsRequired = 1 };
                tapGestureRecognizer.Tapped += async (q, w) => await Navigation.PushAsync(new EInfoPage(line));
                gridEInfo.GestureRecognizers.Add(tapGestureRecognizer);

                gridEInfo.Children.Add(new Label
                {
                    Text = DataBank.EInfo[line].Name,
                    TextColor = Color.White,
                    FontFamily = "Montserrat-Regular.ttf"
                }, 1, 0);   // Назвение
                FormattedString formattedStringDanger = new FormattedString();                                                                                          //
                formattedStringDanger.Spans.Add(new Span { Text = "Опасность: ", TextColor = Color.White });                                                //
                formattedStringDanger.Spans.Add(new Span { Text = DataBank.EInfo[line].Danger, TextColor = DataBank.EInfo[line].DangerColor }); // Опасность
                Label labelDanger = new Label
                {
                    FontFamily = "Montserrat-Light.ttf",
                    Margin = new Thickness(0, -7, 0, -7),
                    FormattedText = formattedStringDanger
                };                                                                                                                                      //
                gridEInfo.Children.Add(labelDanger, 1, 1);                                                                                                                          //
                gridEInfo.Children.Add(new Label
                {
                    Text = $"Категория: {DataBank.EInfo[line].Category}",
                    TextColor = Color.White,
                    FontFamily = "Montserrat-Light.ttf"
                }, 1, 2);   // Категория

                StackLayoutMainInfo.Children.Add(gridEInfo);
            }

            if (StackLayoutMainInfo.Children.Where(x => x.GetType() == typeof(Grid)).Count() == 0 && !(labelProductTitle.Text == "" || labelProductTitle.Text == null))
            {
                StackLayoutMainInfo.Children.Add(Label);
            }
            else
            {
                StackLayoutMainInfo.Children.Remove(Label);
            }
        }

        /// <summary>
        /// Нормализует состав продукта
        /// </summary>
        /// <param name="productCompositions">Состав продукта</param>
        /// <returns>Массив компонентов состава</returns>
        private string[] NormalzeProductCompositions(string productCompositions)
        {
            new string[] { "(", ")", "[", "]", "{", "}", "-", "–", "─", "_", ".", "\"" }.ToList().ForEach(item =>
                productCompositions = productCompositions.Replace(item, ""));

            productCompositions = Regex.Replace(productCompositions, " {2,}", " ");
            productCompositions = productCompositions.Replace(", ", ";").Replace(" ", ";").Replace("Е", "E");

            return productCompositions.Split(';').Distinct().ToArray();
        }
        
    }
}