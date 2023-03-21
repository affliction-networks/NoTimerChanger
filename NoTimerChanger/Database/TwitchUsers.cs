using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoTimerChanger.Database
{
    class TwitchUsers
    {
        [PrimaryKey]
        public string username { get; set; }
        public double totalAmount { get; set; }

        public TwitchUsers()
        {

        }
        public TwitchUsers(string user, double a)
        {
            this.username = user;
            this.totalAmount = a;
        }
    }
}
