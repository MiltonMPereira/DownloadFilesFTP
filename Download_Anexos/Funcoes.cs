using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Download_Anexos
{
    class Funcoes
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public ProgressBar prog_status = (ProgressBar)Application.OpenForms["DownloadAnexos"].Controls.OfType<ProgressBar>().First(x => x.Name == "prog_status");

        public void FocusAplicacao()
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr hWnd = currentProcess.MainWindowHandle;
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
            }
        }

        public void MsgStatus(string msg)
        {
            Label lb_status = (Label)Application.OpenForms["DownloadAnexos"].Controls.OfType<Label>().First(x => x.Name == "lb_status");

            lb_status.Text = msg;
            Application.DoEvents();
        }

        public void MsgInformacao(string msg)
        {
            MessageBox.Show(text: msg, caption: "Informação", buttons:MessageBoxButtons.OK, icon: MessageBoxIcon.Information, defaultButton: MessageBoxDefaultButton.Button1);
        }

        public void MsgAviso(string msg)
        {
            MessageBox.Show(text: msg, caption: "Aviso", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation, defaultButton: MessageBoxDefaultButton.Button1);
        }
    }
}
