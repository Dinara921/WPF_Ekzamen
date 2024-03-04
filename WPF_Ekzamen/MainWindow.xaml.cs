using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Ekzamen.Model;

namespace WPF_Ekzamen
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string login;
        public static int UserID { get; set; }
        public static bool UserStatus { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(LoginTextBox.Text) && !string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    string login = LoginTextBox.Text;
                    string pwd = PasswordBox.Password;

                    var users = await LoadUser(login, pwd);

                    if (users != null && users.Count > 0)
                    {
                        var user = users[0];
                        UserID = user.Id;
                        UserStatus = user.status_user;
                        if (user.status_user)
                        {
                            AdminWin newWin = new AdminWin();
                            newWin.Show();
                            this.Close();
                        }
                        else
                        {
                            SessionsWin newWin = new SessionsWin();
                            newWin.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Логин не верен или такого логина нет.");
                    }
                }
                else
                {
                    MessageBox.Show("Введите логин и пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task<List<User>> LoadUser(string login, string pwd)
        {
            var url = $"http://localhost:5181/LogIn?login={login}&pwd={pwd}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var res = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(res);

                return users;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
        }
    }
}
