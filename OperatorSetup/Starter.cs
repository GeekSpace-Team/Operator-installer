using Ionic.Zip;
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

namespace OperatorSetup
{
    public partial class Starter : Form
    {
        public Starter()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Starter_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        public async Task  ExtractZip(string zipToExtract, string directory)
        {
            try
            {
                using (var zip = ZipFile.Read(zipToExtract))
                {
                    zip.ExtractAll(directory, ExtractExistingFileAction.OverwriteSilently);
                }
                Properties.Settings.Default.FrontendPath = directory;
                Properties.Settings.Default.Save();
                await ExtractBackZip("OperatorBackend.zip");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task  ExtractBackZip(string zipToExtract)
        {
            try
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var subFolderPath = Path.Combine(path, "OperatorBack");
                bool exists = System.IO.Directory.Exists(subFolderPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(subFolderPath);
                using (var zip = ZipFile.Read(zipToExtract))
                {
                    zip.ExtractAll(subFolderPath, ExtractExistingFileAction.OverwriteSilently);
                }
                Properties.Settings.Default.BackendPath = Path.Combine(subFolderPath, "OperatorBackend");
                Properties.Settings.Default.Save();
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(path, "Operator");
            bool exists = System.IO.Directory.Exists(subFolderPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(subFolderPath);

            ExtractZip("build.zip", subFolderPath);
        }
    }
}
