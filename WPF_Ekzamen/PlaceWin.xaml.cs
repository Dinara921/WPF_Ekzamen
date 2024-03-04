using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Ekzamen.Model;

namespace WPF_Ekzamen
{
    /// <summary>
    /// Логика взаимодействия для PlaceWin.xaml
    /// </summary>
    public partial class PlaceWin : Window
    {
        private int HallId;
        private int SessionId;
        private int PlaceNumber;
        private string FilmTitle;
        private string FilmDur;
        private decimal AdultPrice;
        private decimal StudentPrice;
        private decimal ChildPrice;
        public static int SelectedPlace { get; set; }
        public PlaceWin(int hallId, int sessionId, string filmTit, string filmDur, decimal adultPrice, decimal StPrice, decimal ChPrice)
        {
            InitializeComponent();
            HallId = hallId;
            SessionId = sessionId;
            FilmTitle = filmTit;
            FilmDur = filmDur;
            AdultPrice = adultPrice;
            StudentPrice = StPrice;
            ChildPrice = ChPrice;
            LoadPlaces(HallId, SessionId);
        }
        private async void LoadPlaces(int hallId, int sessionId)
        {
            try
            {
                
                string url = $"http://localhost:5181/GetPlace?hall_id={hallId}&session_id={sessionId}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Place> places = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Place>>(responseBody);

                    foreach (var place in places)
                    {
                        Button button = new Button();
                        button.Content = place.place_number.ToString();
                        button.Width = 50;
                        button.Height = 50;
                        button.Margin = new Thickness(5);

                        if (place.is_occupied)
                        {
                            button.Background = Brushes.Red;
                        }
                        else
                        {
                            button.Background = Brushes.Green;

                            button.Click += (sender, e) =>
                            {
                                BuyWin buyWin = new BuyWin(HallId, SessionId, place.place_id, FilmTitle, FilmDur, AdultPrice, StudentPrice, ChildPrice);
                                buyWin.TicketBoughtSuccessfully += (s, args) => MarkButtonAsOccupied(button); 
                                buyWin.Show();
                            };
                        }

                        PlacesWrapPanel.Children.Add(button);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о местах: {ex.Message}");
            }
        }

        private void Button_back(object sender, RoutedEventArgs e)
        {
            SessionsWin sessionsWin = new SessionsWin();
            sessionsWin.Show();
            this.Close();
        }

        private void MarkButtonAsOccupied(Button button)
        {
            button.Background = Brushes.DarkRed;
            button.IsEnabled = false; 
        }
    }
}
