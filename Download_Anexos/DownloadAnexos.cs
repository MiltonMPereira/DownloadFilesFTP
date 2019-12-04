using System;
using System.Windows.Forms;

namespace Download_Anexos
{
    public partial class DownloadAnexos : Form
    {
        public DownloadAnexos()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private void DownloadAnexos_Shown(object sender, EventArgs e)
        {
            this.Hide();
            Arquivo a = new Arquivo();
            a.BuscaArquivos();
            Environment.Exit(0);
        }

        private void DownloadAnexos_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ftp.parar = true;
            e.Cancel = true;
        }
    }
}
