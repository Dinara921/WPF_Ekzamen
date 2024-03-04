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
    /// Логика взаимодействия для AddFilmWin.xaml
    /// </summary>
    public partial class AddFilmWin : Window
    {
        public event EventHandler DataUpdated;
        public AddFilmWin()
        {
            InitializeComponent();
        }
        public void OnDataUpdated()
        {
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }

        private async void Button_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                Film newFilm = new Film
                {
                    name = tb_name.Text,
                    description = tb_description.Text,
                    genre = tb_genre.Text,
                    duration = tb_duration.Text,
                    poster=tb_poster.Text,
                };

                string confirmationMessage = $"Вы уверены, что хотите добавить сеанс?";

                MessageBoxResult result = MessageBox.Show(confirmationMessage, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string url = $"http://localhost:5181/AddOrEditFilm?" +
                                 $"id=0&" +
                                 $"name={newFilm.name}&" +
                                 $"description={newFilm.description}&" +
                                 $"genre={newFilm.genre}&" +
                                 $"duration={newFilm.duration}&" +
                                 $"poster={newFilm.poster}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();
                    }
                    OnDataUpdated();
                    MessageBox.Show("Фильм добавлен");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сеанса: {ex.Message}");
            }
        }

        private async void Button_Change(object sender, RoutedEventArgs e)
        {
            try
            {
                Film newFilm = new Film
                {
                    name = tb_name.Text,
                    description = tb_description.Text,
                    genre = tb_genre.Text,
                    duration = tb_duration.Text,
                    poster = tb_poster.Text,
                };

                string confirmationMessage = $"Вы уверены, что хотите изменить сеанс?";

                MessageBoxResult result = MessageBox.Show(confirmationMessage, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string url = $"http://localhost:5181/AddOrEditFilm?" +
                                 $"id={FilmAdmWin.idFilm}&" +
                                 $"name={newFilm.name}&" +
                                 $"description={newFilm.description}&" +
                                 $"genre={newFilm.genre}&" +
                                 $"duration={newFilm.duration}&" +
                                 $"poster={newFilm.poster}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PostAsync(url, null);
                        response.EnsureSuccessStatusCode();
                    }
                    OnDataUpdated();
                    MessageBox.Show("Фильм изменен");
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
            FilmAdmWin filmAdmWin = new FilmAdmWin();
            filmAdmWin.Show();
            this.Close();
        }
    }
}
