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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Ekzamen.Model;

namespace WPF_Ekzamen
{
    /// <summary>
    /// Логика взаимодействия для FilmAdmWin.xaml
    /// </summary>
    public partial class FilmAdmWin : Window
    {
        public static int idFilm { get; set; }
        public static string FilmName { get; set; }
        public static string FilmDescription { get; set; }
        public static string FilmGenre { get; set; }
        public static string FilmDuration { get; set; }
        public static string FilmPoster { get; set; }

        private AddFilmWin curAddFilm;
        public FilmAdmWin()
        {
            InitializeComponent();
            LoadFilms();
            AddFilmWin addFilm = new AddFilmWin();
            addFilm.DataUpdated += async (sender, args) => await LoadFilms();
        }
        public void NotifyDataUpdatedd(AddFilmWin addFilm)
        {
            curAddFilm = addFilm;
            curAddFilm.DataUpdated += async (sender, args) => await LoadFilms();
            curAddFilm.OnDataUpdated();
        }
        private async Task LoadFilms()
        {
            try
            {
                string url = "http://localhost:5181/GetFilm";

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

        private void FilmDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender;
                if (dataGrid.SelectedItem != null)
                {
                    Film selectedFilm = (Film)dataGrid.SelectedItem;
                    idFilm = selectedFilm.id;
                    FilmName = selectedFilm.name;
                    FilmDescription = selectedFilm.description;
                    FilmDuration = selectedFilm.duration;
                    FilmGenre = selectedFilm.genre;
                    FilmPoster = selectedFilm.poster;  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private async void Button_Search(object sender, RoutedEventArgs e)
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

        private void Button_back(object sender, RoutedEventArgs e)
        {
            AdminWin adminWin = new AdminWin();
            adminWin.Show();
            this.Close();
        }

        private async void Button_Add(object sender, RoutedEventArgs e)
        {
            AddFilmWin adminWin = new AddFilmWin();
            adminWin.Show();
            NotifyDataUpdatedd(adminWin);
        }

        private void Button_Change(object sender, RoutedEventArgs e)
        {

            try
            {
                if (FilmsDataGrid.SelectedItem != null)
                {
                    Film selectedFilm = (Film)FilmsDataGrid.SelectedItem;

                    idFilm = selectedFilm.id;
                    FilmName = selectedFilm.name;
                    FilmDescription = selectedFilm.description;
                    FilmDuration = selectedFilm.duration;
                    FilmGenre = selectedFilm.genre;
                    FilmPoster = selectedFilm.poster;

                    AddFilmWin film = new AddFilmWin();

                    film.tb_name.Text = selectedFilm.name;
                    film.tb_description.Text = selectedFilm.description;
                    film.tb_genre.Text = selectedFilm.genre;
                    film.tb_duration.Text = selectedFilm.duration;
                    film.tb_poster.Text = selectedFilm.poster;

                    film.Show();
                    curAddFilm = film;
                    curAddFilm.DataUpdated += async (s, args) => await LoadFilms();
                    curAddFilm.OnDataUpdated();
                }
                else
                {
                    MessageBox.Show("Ничего не выбрано. Выберите зал и сеанс.");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сеанса: {ex.Message}");
            }
        }

        private async void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = $"http://localhost:5181/DeleteFilm?id={idFilm}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(url, null);
                    response.EnsureSuccessStatusCode();
                    LoadFilms();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о фильмах: {ex.Message}");
            }
        }
    }
}
