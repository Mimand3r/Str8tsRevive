
namespace Str8tsGenerationProject.Pages
{
    partial class Page_Import_And_Display
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.file_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(70, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 51);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import And Display";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_import_and_draw_clicked);
            // 
            // file_label
            // 
            this.file_label.AutoSize = true;
            this.file_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_label.Location = new System.Drawing.Point(290, 61);
            this.file_label.Name = "file_label";
            this.file_label.Size = new System.Drawing.Size(47, 20);
            this.file_label.TabIndex = 2;
            this.file_label.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(234, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Currently Imported:";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(138, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 388);
            this.panel1.TabIndex = 3;
            // 
            // Page_Import_And_Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.file_label);
            this.Controls.Add(this.button1);
            this.Name = "Page_Import_And_Display";
            this.Size = new System.Drawing.Size(961, 606);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label file_label;
        private System.Windows.Forms.Panel panel1;
    }
}
