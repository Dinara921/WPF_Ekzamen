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
    /// Логика взаимодействия для AddSession.xaml
    /// </summary>
    public partial class AddSession : Window
    {
        public event EventHandler DataUpdated;

        public AddSession()
        {
            InitializeComponent();
            LoadComboBoxes();
        }
        public void OnDataUpdated()
        {
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
        private async void LoadComboBoxes()
        {
            try
            {
                List<Film> films = await GetFilmsFromApi();
                cmb_Film.ItemsSource = films;

                List<Hall> halls = await GetHallsFromApi();
                cmb_Hall.ItemsSource = halls;

                Console.WriteLine("Films:");
                foreach (var film in films)
                {
                    Console.WriteLine($"{film.id}: {film.name}");
                }

                Console.WriteLine("Halls:");
                foreach (var hall in halls)
                {
                    Console.WriteLine($"{hall.id}: {hall.name}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
        private async Task<List<Film>> GetFilmsFromApi()
        {
            try
            {
                string url = "http://localhost:5181/GetFilmCmb";
                Console.WriteLine($"Requesting films from: {url}");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response body: {responseBody}");

                    List<Film> films = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Film>>(responseBody);
                    return films;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о фильмах: {ex.Message}");
                return null;
            }
        }

        private async Task<List<Hall>> GetHallsFromApi()
        {
            try
            {
                string url = "http://localhost:5181/GetHallCmb";
                Console.WriteLine($"Requesting halls from: {url}");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response body: {responseBody}");

                    List<Hall> halls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hall>>(responseBody);
                    return halls;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о залах: {ex.Message}");
                return null;
            }
        }
        private async void Button_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                Hall selectedHall = (Hall)cmb_Hall.SelectedItem;
                Film selectedFilm = (Film)cmb_Film.SelectedItem;
                if (selectedHall == null || selectedFilm == null)
                {
                    MessageBox.Show("Выберите зал и фильм.");
                    return;
                }

                Session newSession = new Session
                {
                    hall_id = selectedHall.id,
                    time = tb_time.Text,
                    film_id = selectedFilm.id,
                    priceAdult = decimal.Parse(tb_priceAdult.Text),
                    priceStudent = decimal.Parse(tb_priceStudent.Text),
                    priceChild = decimal.Parse(tb_priceChild.Text),
                };

                string confirmationMessage = $"Вы уверены, что хотите добавить сеанс?";

                MessageBoxResult result = MessageBox.Show(confirmationMessage, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string url = $"http://localhost:5181/AddOrEditSession?" +
                                 $"id=0&" +
                                 $"hall_id={newSession.hall_id}&" +
                                 $"time={newSession.time}&" +
                                 $"film_id={newSession.film_id}&" +
                                 $"priceAdult={newSession.priceAdult/10000}&" +
                                 $"priceStudent={newSession.priceStudent / 10000}&" +
                                 $"priceChild={newSession.priceChild / 10000}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode(); 
                    }
                    OnDataUpdated();
                    MessageBox.Show("Сеанс добавлен");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сеанса: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminWin adminWin = new AdminWin();
            adminWin.Show();
            this.Close();
        }

        private async void Button_Change(object sender, RoutedEventArgs e)
        {
            try
            {
                Hall selectedHall = (Hall)cmb_Hall.SelectedItem;
                Film selectedFilm = (Film)cmb_Film.SelectedItem;
                if (selectedHall == null || selectedFilm == null)
                {
                    MessageBox.Show("Выберите зал и фильм.");
                    return;
                }

                Session newSession = new Session
                {
                    hall_id = selectedHall.id,
                    time = tb_time.Text,
                    film_id = selectedFilm.id,
                    priceAdult = decimal.Parse(tb_priceAdult.Text),
                    priceStudent = decimal.Parse(tb_priceStudent.Text),
                    priceChild = decimal.Parse(tb_priceChild.Text),
                };

                string confirmationMessage = $"Вы уверены, что хотите изменить сеанс?";

                MessageBoxResult result = MessageBox.Show(confirmationMessage, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string url = $"http://localhost:5181/AddOrEditSession?" +
                                 $"id={AdminWin.id}&" +
                                 $"hall_id={newSession.hall_id}&" +
                                 $"time={newSession.time}&" +
                                 $"film_id={newSession.film_id}&" +
                                 $"priceAdult={newSession.priceAdult / 10000}&" +
                                 $"priceStudent={newSession.priceStudent / 10000}&" +
                                 $"priceChild={newSession.priceChild / 10000}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();
                    }
                    OnDataUpdated();
                    MessageBox.Show("Сеанс изменен");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сеанса: {ex.Message}");
            }
        }
    }
}
