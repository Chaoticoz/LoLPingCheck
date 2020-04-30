using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLPingCheck
{
    public partial class Form1 : Form
    {
        private Thread t;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.TopMost = true;
        }
        private static Dictionary<string, string> servers = new Dictionary<string, string>()
        {
          {
            "EUW",
            "104.160.141.3"
          },
          {
            "NA",
            "104.160.131.3"
          },
          {
            "EUNE",
            "104.160.142.3"
          },
          {
            "LAN",
            "104.160.136.3"
          },
          {
            "Garena",
            "203.117.172.253"
          },
          {
            "BR",
            "104.160.152.3"
          }
        };
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("EUW");
            comboBox1.Items.Add("EUNE");
            comboBox1.Items.Add("NA");
            comboBox1.Items.Add("Garena");
            comboBox1.Items.Add("LAN");
            comboBox1.Items.Add("BR");
            comboBox1.SelectedIndex = Properties.Settings.Default.Server;
            lblPing.Text = "Pinging..";
            lblPing.ForeColor = Color.Black;
            t = new Thread(new ThreadStart(this.pingServer));
            t.Start();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Server = comboBox1.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void pingServer()
        {
            int TTL = 64;
            int timeout = 1000;
            

            Ping pingSender = new Ping();
            byte[] buffer = new byte[32];
            PingOptions options = new PingOptions(TTL, true);
            while (true)
            {
                string server = getServer();
                PingReply reply = pingSender.Send(server, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    lblPing.Text = reply.RoundtripTime.ToString();
                    lblPing.ForeColor = Color.Green;
                    if (reply.RoundtripTime > 50) { lblPing.ForeColor = Color.Orange; }
                    if (reply.RoundtripTime > 100) { lblPing.ForeColor = Color.Red; }
                }
                else
                {
                    lblPing.Text = "???";
                }
                Thread.Sleep(250);
            }
        }
        public string getServer()
        {
            return servers[comboBox1.Text];
        }

        private void btnPing_Click(object sender, EventArgs e)
        {

        }

        private void lblPing_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.mpgh.net/forum/member.php?u=670645"));
            this.linkLabel1.LinkVisited = true;
        }
    }
}
