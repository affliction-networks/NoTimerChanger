using NoTimerChanger.Inputs;
using NoTimerChanger.Timer;
using StreamElements.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoTimerChanger
{
    public partial class Form1 : Form
    {

        private TimerUtils timerUtils;
        private bool isPaused = true;
        public static CountdownDuration timer;
        public AuthRestClient authClient;

        public Form1()
        {
            InitializeComponent();
            if(Program.SEKey != "" && Program.channel != "")
            {
                if(!File.Exists("limit.txt"))
                {
                    File.Create("limit.txt");
                }
                // Initilize Stream Elements API
                authClient = new AuthRestClient(Program.SEKey);
                Thread newThread = new Thread(DoWork);
                newThread.Start(authClient);
            }
            timerUtils = new TimerUtils();
            timer = CountdownDuration.Parse(0);
        }
        public async void DoWork(object data)
        {
            AuthRestClient client = (AuthRestClient)data;
            while(true)
            {
                try
                {
                    Thread.Sleep(100);
                    var tip = await client.GetTips(Program.channel);
                    if(Program.maxTotal != -1 && Program.getTwitchTotal() >= Program.maxTotal)
                    {
                        File.WriteAllText("limit.txt", "TOTAL LIMIT REACHED");
                        return;
                    }
                    Program.addTwitchDono(tip.docs.donation.user.username, tip.docs.donation.amount);
                    if(Program.negativeKeyword != "NONE" && tip.docs.donation.message.Contains(Program.negativeKeyword))
                    {
                        if(tip.docs.donation.amount >= Program.negativeDonoAmount)
                        {
                            if(Program.maxPer == -1)
                            {
                                double amountToRemove = tip.docs.donation.amount * Program.negativeAmountPerNegativeDono;
                                // Assume it's in seconds
                                try
                                {
                                    isPaused = true;
                                    long finalSeconds = timer.Seconds - (int)amountToRemove;
                                    if(finalSeconds <= 0)
                                    {
                                        finalSeconds = 0;
                                    }
                                    timer = CountdownDuration.Parse(finalSeconds);
                                    isPaused = false;
                                }
                                catch (Exception ex)
                                {
                                }
                            } else
                            {
                                long maxAllowed = Program.maxPer;
                                if(Program.getTwitchUserTotal(tip.docs.donation.user.username) >= maxAllowed)
                                {
                                    File.AppendAllText("limitlogs.txt", tip.ToString() + "\n\n");
                                } else
                                {
                                    double amountToRemove = tip.docs.donation.amount * Program.negativeAmountPerNegativeDono;
                                    // Assume it's in seconds
                                    try
                                    {
                                        isPaused = true;
                                        long finalSeconds = timer.Seconds - (int)amountToRemove;
                                        if (finalSeconds <= 0)
                                        {
                                            finalSeconds = 0;
                                        }
                                        timer = CountdownDuration.Parse(finalSeconds);
                                        isPaused = false;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                    } else
                    {
                        if(tip.docs.donation.amount >= Program.donoAmountPer)
                        {
                            if (Program.maxPer == -1)
                            {
                                double amountToAdd = tip.docs.donation.amount * Program.negativeAmountPerNegativeDono;
                                // Assume it's in seconds
                                try
                                {
                                    isPaused = true;
                                    long finalSeconds = timer.Seconds + (int)amountToAdd;
                                    timer = CountdownDuration.Parse(finalSeconds);
                                    isPaused = false;
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                long maxAllowed = Program.maxPer;
                                if (Program.getTwitchUserTotal(tip.docs.donation.user.username) >= maxAllowed)
                                {
                                    File.AppendAllText("limitlogs.txt", tip.ToString() + "\n\n");
                                }
                                else
                                {
                                    double amountToAdd = tip.docs.donation.amount * Program.negativeAmountPerNegativeDono;
                                    // Assume it's in seconds
                                    try
                                    {
                                        isPaused = true;
                                        long finalSeconds = timer.Seconds + (int)amountToAdd;
                                        timer = CountdownDuration.Parse(finalSeconds);
                                        isPaused = false;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
        }

        private static void WriteFile(string input)
        {
            File.WriteAllText("countdown.txt", input);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                timerStatus.Text = "Paused";
            }
            else
            {
                timerStatus.Text = "Active";
            }
            timer1.Enabled = !isPaused;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Program.isErrorConfig)
            {
                MessageBox.Show("Due to a config error.. The timer will not function until you reset the config");
                timer1.Enabled = false;
                return;
            }
            timer.Tick();
            string result = timer.GetOutput();
            tickingTimer.Text = result;
            WriteFile(result);
        }

        // Set time button
        private void button4_Click(object sender, EventArgs e)
        {
            string result = "";
            InputBox.Show("Please enter a time (You may use d for days, h for hours, m for minutes and s for seconds):", 
                                         "Input Date", ref result);
            if(result.Count(x => x == 'd') > 1)
            {
                MessageBox.Show("Invalid.. Only use one day modifier.");
                return;
            }
            if (result.Count(x => x == 'h') > 1)
            {
                MessageBox.Show("Invalid.. Only use one hour modifier.");
                return;
            }
            if (result.Count(x => x == 'm') > 1)
            {
                MessageBox.Show("Invalid.. Only use one minute modifier.");
                return;
            }
            if (result.Count(x => x == 's') > 1)
            {
                MessageBox.Show("Invalid.. Only use one second modifier.");
                return;
            }
            if (!result.Contains("d") && !result.Contains("h") && !result.Contains("m") && !result.Contains("s"))
            {
                // Assume it's in seconds
                try
                {
                    int finalSeconds = int.Parse(result);
                    isPaused = true;
                    timer = CountdownDuration.Parse(finalSeconds);
                    isPaused = false;
                } catch(Exception ex)
                {
                    MessageBox.Show("Unable to process that number.");
                    return;
                }
            } else
            {
                isPaused = true;
                TimerParseResult timerResult = timerUtils.parseTime(result);
                long finalSeconds = 0;
                if(timerResult.days > 0)
                {
                    finalSeconds += timerResult.days * 86400;
                }
                if(timerResult.hours > 0)
                {
                    finalSeconds += timerResult.hours * 3600;
                }
                if (timerResult.minutes > 0)
                {
                    finalSeconds += timerResult.minutes * 60;
                }
                if (timerResult.seconds > 0)
                {
                    finalSeconds += timerResult.seconds;
                }
                isPaused = true;
                timer = CountdownDuration.Parse(finalSeconds);
                isPaused = false;
            }
        
        }
        // Add time button
        private void button2_Click(object sender, EventArgs e)
        {
            string result = "";
            InputBox.Show("Please enter a time (You may use d for days, h for hours, m for minutes and s for seconds):",
                                         "Input Date", ref result);
            if (result.Count(x => x == 'd') > 1)
            {
                MessageBox.Show("Invalid.. Only use one day modifier.");
                return;
            }
            if (result.Count(x => x == 'h') > 1)
            {
                MessageBox.Show("Invalid.. Only use one hour modifier.");
                return;
            }
            if (result.Count(x => x == 'm') > 1)
            {
                MessageBox.Show("Invalid.. Only use one minute modifier.");
                return;
            }
            if (result.Count(x => x == 's') > 1)
            {
                MessageBox.Show("Invalid.. Only use one second modifier.");
                return;
            }
            if (!result.Contains("d") && !result.Contains("h") && !result.Contains("m") && !result.Contains("s"))
            {
                // Assume it's in seconds
                try
                {
                    isPaused = true;
                    long finalSeconds = timer.Seconds + int.Parse(result);
                    timer = CountdownDuration.Parse(finalSeconds);
                    isPaused = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to process that number.");
                    return;
                }
            }
            else
            {
                isPaused = true;
                TimerParseResult timerResult = timerUtils.parseTime(result);
                long finalSeconds = timer.Seconds;
                if (timerResult.days > 0)
                {
                    finalSeconds += timerResult.days * 86400;
                }
                if (timerResult.hours > 0)
                {
                    finalSeconds += timerResult.hours * 3600;
                }
                if (timerResult.minutes > 0)
                {
                    finalSeconds += timerResult.minutes * 60;
                }
                if (timerResult.seconds > 0)
                {
                    finalSeconds += timerResult.seconds;
                }
                isPaused = true;
                long currentSeconds = timer.Seconds;
                timer = CountdownDuration.Parse(finalSeconds);
                isPaused = false;
            }
        }
        // Remove time button
        private void button3_Click(object sender, EventArgs e)
        {
            string result = "";
            InputBox.Show("Please enter a time (You may use d for days, h for hours, m for minutes and s for seconds):",
                                         "Input Date", ref result);
            if (result.Count(x => x == 'd') > 1)
            {
                MessageBox.Show("Invalid.. Only use one day modifier.");
                return;
            }
            if (result.Count(x => x == 'h') > 1)
            {
                MessageBox.Show("Invalid.. Only use one hour modifier.");
                return;
            }
            if (result.Count(x => x == 'm') > 1)
            {
                MessageBox.Show("Invalid.. Only use one minute modifier.");
                return;
            }
            if (result.Count(x => x == 's') > 1)
            {
                MessageBox.Show("Invalid.. Only use one second modifier.");
                return;
            }
            if (!result.Contains("d") && !result.Contains("h") && !result.Contains("m") && !result.Contains("s"))
            {
                // Assume it's in seconds
                try
                {
                    long finalSeconds = timer.Seconds - int.Parse(result);
                    isPaused = true;
                    if(finalSeconds <= 0)
                    {
                        timer = CountdownDuration.Parse(0);
                        isPaused = false;
                        return;
                    }
                    timer = CountdownDuration.Parse(finalSeconds);
                    isPaused = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to process that number.");
                    return;
                }
            }
            else
            {
                isPaused = true;
                TimerParseResult timerResult = timerUtils.parseTime(result);
                long finalSeconds = timer.Seconds;
                if (timerResult.days > 0)
                {
                    finalSeconds -= timerResult.days * 86400;
                }
                if (timerResult.hours > 0)
                {
                    finalSeconds -= timerResult.hours * 3600;
                }
                if (timerResult.minutes > 0)
                {
                    finalSeconds -= timerResult.minutes * 60;
                }
                if (timerResult.seconds > 0)
                {
                    finalSeconds -= timerResult.seconds;
                }
                if (finalSeconds <= 0)
                {
                    timer = CountdownDuration.Parse(0);
                    isPaused = false;
                    return;
                }
                timer = CountdownDuration.Parse(finalSeconds);
                isPaused = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            config.Show();
        }
    }
}
