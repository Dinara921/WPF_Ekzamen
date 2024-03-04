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
    /// Логика взаимодействия для FilmWin.xaml
    /// </summary>
    public partial class FilmWin : Window
    {
        public FilmWin()
        {
            InitializeComponent();
            LoadFilms();
        }
        private async void LoadFilms(string data="")
        {
            try
            {
                string url = "http://localhost:5181/GetFilm";

                if (!string.IsNullOrEmpty(data))
                {
                    url += $"?data={data}";
                }

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Film> films = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Film>>(responseBody);

                    FilmsDataGrid.ItemsSource = films;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о фильмах: {ex.Message}");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string data = Search_tb.Text;
            try
            {
                string url = $"http://localhost:5181/GetFilm?data={data}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Film> films = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Film>>(responseBody);

                    FilmsDataGrid.ItemsSource = films;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о фильмах: {ex.Message}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SessionsWin sessionsWin = new SessionsWin();
            sessionsWin.Show();
            this.Close();
        }
    }
}
