using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoTimerChanger.Database
{
    class Totals
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int totalAmountDonated = 0;
    }
}
