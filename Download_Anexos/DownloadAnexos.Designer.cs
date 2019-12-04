namespace Download_Anexos
{
    partial class DownloadAnexos
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadAnexos));
            this.prog_status = new System.Windows.Forms.ProgressBar();
            this.lb_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // prog_status
            // 
            this.prog_status.Location = new System.Drawing.Point(21, 46);
            this.prog_status.Name = "prog_status";
            this.prog_status.Size = new System.Drawing.Size(288, 23);
            this.prog_status.Step = 1;
            this.prog_status.TabIndex = 0;
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_status.Location = new System.Drawing.Point(18, 27);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(45, 16);
            this.lb_status.TabIndex = 1;
            this.lb_status.Text = "Status";
            // 
            // DownloadAnexos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 104);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.prog_status);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadAnexos";
            this.Text = "Download de anexos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadAnexos_FormClosing);
            this.Shown += new System.EventHandler(this.DownloadAnexos_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar prog_status;
        private System.Windows.Forms.Label lb_status;
    }
}

