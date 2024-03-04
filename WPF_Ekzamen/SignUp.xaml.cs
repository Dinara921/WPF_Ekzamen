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
using System.Windows.Shapes;
using WPF_Ekzamen.Model;

namespace WPF_Ekzamen
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = tb_login.Text;
            string pwd = tb_pwd1.Password;
            string phone= tb_phone.Text;

            if (login == "" || login.Length < 5)
            {
                MessageBox.Show("Логин должен быть не менее 5 символов");
            }

            else if (pwd == "" || pwd.Length < 5)
            {
                MessageBox.Show("Введите пароль, пароль должен иметь не менее 5 символов");
            }
            else if (phone == "" || phone.Length < 11)
            {
                MessageBox.Show("Введите телефон");
            }
            else if (pwd != tb_pwd2.Password)
            {
                MessageBox.Show("Пароли не совпадают");
            }
            else
            {
                var url = $"http://localhost:5181/AddUsers?Login={login}&Password={pwd}&Phone={phone}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(url, null);
                    response.EnsureSuccessStatusCode();
                }
                MessageBox.Show("Пользователь зарегистрирован");
                this.Close();
            }   
        }
    }
}
