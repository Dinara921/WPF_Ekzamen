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
    /// Логика взаимодействия для AdminWin.xaml
    /// </summary>
    public partial class AdminWin : Window
    {
        public static int id { get; set; }
        public static int sessionId { get; set; }
        public static int HallNumber { get; set; }
        public static int SessionNumber { get; set; }
        public static string FilmTitle { get; set; }
        public static string SessionTime { get; set; }
        public SessionWithFilm SelectedFilm { get; set; }
        public static decimal PriceAdult { get; set; }
        public static decimal PriceStudent { get; set; }
        public static decimal PriceChild { get; set; }

        private AddSession curAddSession;
        public AdminWin()
        {
            InitializeComponent();
            LoadHall();
            AddSession addSession = new AddSession();
            addSession.DataUpdated += async (sender, args) => await LoadHall();
        }
        public void NotifyDataUpdated(AddSession addSession)
        {
            curAddSession = addSession;
            curAddSession.DataUpdated += async (sender, args) => await LoadHall();
            curAddSession.OnDataUpdated();
        }
        public async Task LoadHall(string searchData = null)
        {
            try
            {
                string url = string.IsNullOrEmpty(searchData) ? "http://localhost:5181/GetSessionsByFilmOrGenre" : $"http://localhost:5181/GetSessionsByFilmOrGenre?data={searchData}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Hall> halls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hall>>(responseBody);

                    if (halls.Count > 0)
                    {
                        HallsDataGrid.ItemsSource = halls;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о залах: {ex.Message}");
            }
        }
        private async void Button_search(object sender, RoutedEventArgs e)
        {
            string searchData = Search_tb.Text.Trim();
            if (string.IsNullOrEmpty(searchData))
            {
                await LoadHall();
            }
            else
            {
                await LoadHall(searchData);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWin = new MainWindow();
            newWin.Show();
            this.Close();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = $"http://localhost:5181/GetTicketAdmin";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Ticket2> tickets = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Ticket2>>(responseBody);

                    if (tickets.Count > 0)
                    {
                        ReturnTicket tick = new ReturnTicket(tickets);
                        tick.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о билетах: {ex.Message}");
            }
        }

        private void Button_Film(object sender, RoutedEventArgs e)
        {
            FilmAdmWin film = new FilmAdmWin();
            film.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            Hall selectedHall = (Hall)HallsDataGrid.SelectedItem;

            if (selectedHall != null && SelectedFilm != null)
            {
                HallNumber = selectedHall.id;
                SessionNumber = SelectedFilm.sessionId;

                PlaceWin placeWin = new PlaceWin(HallNumber, SessionNumber, FilmTitle, SessionTime, PriceAdult, PriceStudent, PriceChild);
                placeWin.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите фильм и зал для покупки.");
            }
        }
        private void HallsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender;
                if (dataGrid.SelectedItem != null)
                {
                    Hall selectedHall = (Hall)dataGrid.SelectedItem;
                    id=selectedHall.id;

                    if (selectedHall != null && selectedHall.session != null && selectedHall.session.Count > 0)
                    {
                        DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
                        if (row != null)
                        {
                            DataGrid nestedDataGrid = FindNestedDataGrid(row);
                            if (nestedDataGrid != null && nestedDataGrid.SelectedItem != null)
                            {
                                SessionWithFilm selectedSession = (SessionWithFilm)nestedDataGrid.SelectedItem;
                                SelectedFilm = selectedSession;
                                PriceAdult = selectedSession.PriceAdult;
                                PriceStudent = selectedSession.PriceStudent;
                                PriceChild = selectedSession.PriceChild;
                                FilmTitle = selectedSession.FilmTitle;
                                SessionTime = selectedSession.SessionTime;
                                sessionId = selectedSession.sessionId;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private DataGrid FindNestedDataGrid(DataGridRow row)
        {
            var nestedDataGrid = FindVisualChild<DataGrid>(row);
            return nestedDataGrid;
        }
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        private async void Button_Add(object sender, RoutedEventArgs e)
        {
            AddSession addSession = new AddSession();
            addSession.Show();
            NotifyDataUpdated(addSession);

        }

        private async void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = $"http://localhost:5181/DeleteSession?id={sessionId}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(url, null);
                    response.EnsureSuccessStatusCode();
                    LoadHall();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о фильмах: {ex.Message}");
            }
        
        }

        private void Button_Change(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HallsDataGrid.SelectedItem != null)
                {
                    Hall selectedHall = (Hall)HallsDataGrid.SelectedItem;
                    id = selectedHall.id;

                    if (selectedHall != null && selectedHall.session != null && selectedHall.session.Count > 0)
                    {
                        DataGridRow row = (DataGridRow)HallsDataGrid.ItemContainerGenerator.ContainerFromItem(HallsDataGrid.SelectedItem);
                        if (row != null)
                        {
                            DataGrid nestedDataGrid = FindNestedDataGrid(row);
                            if (nestedDataGrid != null && nestedDataGrid.SelectedItem != null)
                            {
                                SessionWithFilm selectedSession = (SessionWithFilm)nestedDataGrid.SelectedItem;
                                SelectedFilm = selectedSession;
                                PriceAdult = selectedSession.PriceAdult;
                                PriceStudent = selectedSession.PriceStudent;
                                PriceChild = selectedSession.PriceChild;
                                FilmTitle = selectedSession.FilmTitle;
                                SessionTime = selectedSession.SessionTime;
                                sessionId = selectedSession.sessionId;

                                AddSession session = new AddSession();

                                session.tb_time.Text = selectedSession.SessionTime;
                                session.tb_priceAdult.Text = PriceAdult.ToString();
                                session.tb_priceStudent.Text = PriceStudent.ToString();
                                session.tb_priceChild.Text = PriceChild.ToString();
                                session.cmb_Hall.SelectedItem = selectedHall;
                                session.cmb_Film.SelectedItem = selectedSession.sessionId;

                                session.Show();
                                curAddSession = session;
                                curAddSession.DataUpdated += async (s, args) => await LoadHall();
                                curAddSession.OnDataUpdated();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ничего не выбрано. Выберите зал и сеанс.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сеанса: {ex.Message}");
            }
        }
    }
}
