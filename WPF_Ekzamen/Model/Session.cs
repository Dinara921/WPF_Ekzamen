using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Ekzamen.Model
{
    internal class Session
    {
        public int id { get; set; }
        public int hall_id { get; set; }
        public string time { get; set; }
        public int film_id { get; set; }
        public decimal priceAdult { get; set; }
        public decimal priceStudent { get; set; }
        public decimal priceChild { get; set; }
    }
    public class SessionWithFilm
    {
        public string SessionTime { get; set; }
        public string FilmTitle { get; set; }
        public string FilmDuration { get; set; }
        public string Genre { get; set; }
        public decimal PriceAdult { get; set; }
        public decimal PriceStudent { get; set; }
        public decimal PriceChild { get; set; }
        public int sessionId { get; set; }
    }

    public class Film
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public string duration { get; set; }
        public string poster { get; set; }
    }
    public class Ticket
    {
        public string film_name { get; set; }
        public string session_time { get; set; }
        public int hall_id { get; set; }
        public int place_id { get; set; }
        public decimal amount { get; set; }
        public int ticket_id { get; set; }
    }
    public class Ticket2
    {
        public string Login { get; set; }
        public int Ticket_id { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public decimal Cost { get; set; }
    }
}
