using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using WPF_Ekzamen.Model;

namespace WPF_Ekzamen
{
    public partial class ReturnTicket : Window
    {
        private List<Ticket2> _tickets;

        public ReturnTicket(List<Ticket2> tickets)
        {
            InitializeComponent();
            _tickets = tickets;
            LoadTickets();
        }

        private void LoadTickets()
        {
            foreach (var ticket in _tickets)
            {
                TicketListBox.Items.Add($"Пользователь: {ticket.Login}, Билет: {ticket.Ticket_id}, Сеанс: {ticket.Title}, Время: {ticket.Time},  Цена: {ticket.Cost}");
            }
        }

        private async void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (TicketListBox.SelectedItem != null)
            {
                var selectedTicket = _tickets[TicketListBox.SelectedIndex];
                int ticketId = selectedTicket.Ticket_id;

                try
                {
                    string url = $"http://localhost:5181/ReturnTicket?ticket_id={ticketId}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PostAsync(url, null); 
                        response.EnsureSuccessStatusCode();

                        MessageBox.Show($"Билет {ticketId} возвращен!");
                        _tickets.Remove(selectedTicket);
                        TicketListBox.Items.Remove(TicketListBox.SelectedItem);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при возврате билета: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите билет для возврата.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool user = MainWindow.UserStatus;
            if (user)
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
    }
}
