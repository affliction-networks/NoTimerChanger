using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoTimerChanger
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
            if (File.Exists("DONOTOPEN-ON-STREAM.txt"))
            {
                textBox1.Text = File.ReadAllText("DONOTOPEN-ON-STREAM.txt");
            }
            if (File.Exists("config.txt"))
            {
                string config = File.ReadAllText("config.txt");
                string[] cmd = config.Split(':');
                textBox2.Text = cmd[0];
                textBox3.Text = cmd[1];
                textBox4.Text = cmd[2];
                textBox5.Text = cmd[3];
                textBox6.Text = cmd[4];
                textBox7.Text = cmd[5];
                textBox8.Text = cmd[6];
                textBox9.Text = cmd[7];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Only if you changed your StreamElements key do you need to restart.");
            File.WriteAllText("DONOTOPEN-ON-STREAM.txt", textBox1.Text);
            String config = "";
            config += textBox2.Text + ":";
            config += textBox3.Text + ":";
            config += textBox4.Text + ":";
            config += textBox5.Text + ":";
            config += textBox6.Text + ":";
            config += textBox7.Text + ":";
            config += textBox8.Text + ":";
            config += textBox9.Text;

            File.WriteAllText("config.txt", config);
            Program.loadConfig();
            MessageBox.Show("Saved Config. You are free to close the config now.");
        }
    }
}
