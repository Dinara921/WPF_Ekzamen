using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Ekzamen.Model
{
    internal class User
    {
        public int Id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public bool status_user { get; set; }
    }
}
