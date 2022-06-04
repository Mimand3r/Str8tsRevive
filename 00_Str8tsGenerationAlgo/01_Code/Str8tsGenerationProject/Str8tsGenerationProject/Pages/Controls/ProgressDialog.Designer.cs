namespace Str8tsGenerationProject.Pages.Controls
{
    partial class ProgressDialog
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Label();
            this.mainText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bitte warten";
            // 
            // timer
            // 
            this.timer.AutoSize = true;
            this.timer.Location = new System.Drawing.Point(224, 10);
            this.timer.Name = "timer";
            this.timer.Size = new System.Drawing.Size(13, 13);
            this.timer.TabIndex = 1;
            this.timer.Text = "0";
            // 
            // mainText
            // 
            this.mainText.AutoSize = true;
            this.mainText.Location = new System.Drawing.Point(17, 61);
            this.mainText.MaximumSize = new System.Drawing.Size(230, 60);
            this.mainText.MinimumSize = new System.Drawing.Size(230, 60);
            this.mainText.Name = "mainText";
            this.mainText.Size = new System.Drawing.Size(230, 60);
            this.mainText.TabIndex = 2;
            this.mainText.Text = "Datei 1/10 wird erstellt";
            this.mainText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.mainText);
            this.Controls.Add(this.timer);
            this.Controls.Add(this.label1);
            this.Name = "ProgressDialog";
            this.Size = new System.Drawing.Size(261, 141);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label timer;
        private System.Windows.Forms.Label mainText;
    }
}
