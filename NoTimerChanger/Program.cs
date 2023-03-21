using NoTimerChanger.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoTimerChanger
{
    static class Program
    {

        public static string SEKey = "";
        public static string channel = "";
        public static long secondsPerDono = 0;
        public static double donoAmountPer = 0;
        public static string negativeKeyword = "NONE";
        public static double negativeDonoAmount = 0;
        public static long negativeAmountPerNegativeDono = 0;
        public static long maxPer = 0;
        public static long maxTotal = 0;

        public static bool isErrorConfig = false;
        public static SQLiteConnection db;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var databasePath = Path.Combine(@"", "data.db");
            db = new SQLiteConnection(databasePath);
            db.CreateTable<Totals>();
            db.CreateTable<TwitchUsers>();
            loadConfig();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static double getTwitchTotal()
        {
            if(db.Table<Totals>().Count() == 0)
            {
                return 0;
            }
            var query = db.Table<Totals>().First();
            return query.totalAmountDonated;
        }

        public static double getTwitchUserTotal(string username)
        {
            if (db.Table<TwitchUsers>().Where(v => v.username == username).Count() == 0)
            {
                return 0;
            }
            var query = db.Table<TwitchUsers>().Where(v => v.username == username).First();
            return query.totalAmount;
        }
        public static void addTwitchDono(string username, double amount)
        {
            if (db.Table<TwitchUsers>().Where(v => v.username == username).Count() == 0)
            {
                db.Insert(new TwitchUsers(username, amount));
            } else
            {
                var query = db.Table<TwitchUsers>().Where(v => v.username == username).First();
                query.totalAmount += amount;
                db.Update(query);
            }

        }

        public static void loadConfig()
        {
           if(File.Exists("config.txt"))
           {
                try
                {
                    string config = File.ReadAllText("config.txt");
                    string[] cmd = config.Split(':');
                    secondsPerDono = long.Parse(cmd[0]);
                    donoAmountPer = double.Parse(cmd[1]);
                    negativeKeyword = cmd[2];
                    negativeDonoAmount = double.Parse(cmd[3]);
                    negativeAmountPerNegativeDono = long.Parse(cmd[4]);
                    maxPer = long.Parse(cmd[5]);
                    maxTotal = long.Parse(cmd[6]);
                    channel = cmd[7];
                } catch(Exception ex)
                {
                    isErrorConfig = true;
                    MessageBox.Show("There was an error loading the config.. Delete config.txt only and reconfigure..");
                }
           }
        }
    }
}
