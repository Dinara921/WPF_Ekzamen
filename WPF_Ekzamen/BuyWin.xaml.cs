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
    /// Логика взаимодействия для BuyWin.xaml
    /// </summary>
    public partial class BuyWin : Window
    {
        private int HallId;
        private int SessionId;
        private int PlaceNumber;
        private string FilmTitle;
        private string FilmDur;
        private decimal AdultPrice;
        private decimal StudentPrice;
        private decimal ChildPrice;
        private int Us_id;
        public event EventHandler TicketBoughtSuccessfully;

        public BuyWin(int hallId, int sessionId, int placeNumber, string filmTit, string filmDur, decimal adultPrice, decimal studentPrice, decimal childPrice)
        {
            InitializeComponent();
            HallId = hallId;
            SessionId = sessionId;
            PlaceNumber = placeNumber;
            FilmTitle = filmTit;
            FilmDur = filmDur;
            AdultPrice = adultPrice;
            StudentPrice = studentPrice;
            ChildPrice = childPrice;
            LoadPrices();
            DisplaySelectedPlace();
        }
        private void LoadPrices()
        {
            AddPriceButton("Взрослый", AdultPrice);
            AddPriceButton("Студенческий", StudentPrice);
            AddPriceButton("Детский", ChildPrice);
        }

        private void DisplaySelectedPlace()
        {
            string placeInfo = $"Место: {PlaceNumber}, Зал: {HallId}, Время: {FilmDur}";
            PlaceInfoLabel.Content = placeInfo;
        }

        private void AddPriceButton(string ticketType, decimal price)
        {
            Button button = new Button();
            button.Content = $"{ticketType} ({price} тг.)";
            button.Width = 150;
            button.Height = 30;
            button.Margin = new Thickness(5);
            button.Tag = price; 
            button.Click += (sender, e) =>
            {
                decimal selectedPrice = (decimal)((Button)sender).Tag; 
                BuyTicket(selectedPrice);
            };

            TicketTypeStackPanel.Children.Add(button);
        }

        private async void BuyTicket(decimal price)
        {
            Us_id = MainWindow.UserID;
            decimal cost = price / 10000;
            try
            {
                string url = $"http://localhost:5181/BuyTicket?place_id={PlaceNumber}&session_id={SessionId}&user_id={Us_id}&amount={cost}";
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    MessageBox.Show("Билет куплен!");
                    TicketBoughtSuccessfully?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при покупке билета: {ex.Message}");
            }
        }
    }
}
