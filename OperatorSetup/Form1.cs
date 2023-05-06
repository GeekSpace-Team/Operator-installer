using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;

namespace OperatorSetup
{
    public partial class Form1 : Form
    {
        private string adminUrl = "http://" + GetLocalIPAddress() + ":" + Properties.Settings.Default.FrontPort + "/login";
        private string entireUrl = "http://" + GetLocalIPAddress() + ":" + Properties.Settings.Default.FrontPort + "/login";
        private string frontName = "OperatorFront";
        private string backName = "OperatorBack";
        

        private bool isFrontRunning = false, isBackendRunning = false;

        public Form1()
        {
            InitializeComponent();
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FrontendPath == "")
            {
                new Starter().ShowDialog();
                init();
                start();
            }
            else
            {
                init();
                start();
            }

        }

       

        private void start()
        {
            deleteBackend();
        }

        private void startBackend()
        {
            runCmdCommand("cd " + textBox1.Text + "&pm2 start index.mjs --name '" + backName + "'", 1);
        }

        private void startFrontEnd()
        {
            runCmdCommand("cd " + textBox4.Text + "&pm2 serve build " + textBox5.Text + " --name '" + frontName + "' --spa", 2);
        }

        private void deleteBackend()
        {
            runCmdCommand("pm2 delete " + backName, -1);
        }

        private void deleteFrontend()
        {
            runCmdCommand("pm2 delete " + frontName, 0);
        }


        private void check()
        {
            runCmdCommand("pm2 list", 3);
        }

        private void runningChecker(string str)
        {
            isFrontRunning = str.Contains(frontName);
            isBackendRunning = str.Contains(backName);

            if (isBackendRunning)
            {
                label7.Text = "Işleýär...";
                label7.ForeColor = Color.Green;
            }
            else
            {
                label7.Text = "Işlemeýär";
                label7.ForeColor = Color.Red;
            }

            if (isFrontRunning)
            {
                label9.Text = "Işleýär...";
                label9.ForeColor = Color.Green;
            }
            else
            {
                label9.Text = "Işlemeýär";
                label9.ForeColor = Color.Red;
            }

            runCmdCommand("pm2 save", 5);
        }

        private void runCmdCommand(string v, int type)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(850);
            cmd.Start();

            cmd.StandardInput.WriteLine(v);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            cmd.WaitForExit();
            string result = cmd.StandardOutput.ReadToEnd();
            writeConsole(result);

            if (type == -1)
            {
                startBackend();
            }

            if (type == 1)
            {
                deleteFrontend();
            }

            if (type == 0)
            {
                startFrontEnd();
            }

            if (type == 2)
            {
                check();
            }

            if (type == 3)
            {
                runningChecker(result);
            }
        }

        private void init()
        {
            label5.Text = "IP salgysy: " + GetLocalIPAddress();

            if (Properties.Settings.Default.AdminUrl != "")
            {

                adminUrl = Properties.Settings.Default.AdminUrl;
            }

            if (Properties.Settings.Default.EntireUrl != "")
            {
                entireUrl = Properties.Settings.Default.EntireUrl;
            }

            textBox2.Text = adminUrl;
            textBox3.Text = entireUrl;

            textBox1.Text = Properties.Settings.Default.BackendPath;
            textBox4.Text = Properties.Settings.Default.FrontendPath;
            textBox5.Text = Properties.Settings.Default.FrontPort.ToString();
        }

        public void writeConsole(String str)
        {
            consoleLog.AppendText("\n\n\n" + str);
        }

        public static string GetLocalIPAddress()
        {
            string localIP = "0";
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }

        public static string getPort()
        {
            return "6415";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AdminUrl = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.EntireUrl = textBox3.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Properties.Settings.Default.BackendPath = dialog.FileName;
                textBox1.Text = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Properties.Settings.Default.FrontendPath = dialog.FileName;
                textBox4.Text = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WebPanel webPanel = new WebPanel();
            webPanel.source = textBox2.Text;
            webPanel.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WebPanel webPanel = new WebPanel();
            webPanel.source = textBox3.Text;
            webPanel.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HowOpenBrowser how = new HowOpenBrowser();
            how.address = textBox3.Text;
            how.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(textBox2.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(textBox3.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FrontPort = int.Parse(textBox5.Text);
            Properties.Settings.Default.Save();
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
