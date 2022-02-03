
namespace Str8tsGenerationProject.Pages
{
    partial class Page_Solve_Board
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.file_label = new System.Windows.Forms.Label();
            this.button_import_and_display = new System.Windows.Forms.Button();
            this.button_solve = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(48, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(876, 402);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(235, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Currently Imported:";
            // 
            // file_label
            // 
            this.file_label.AutoSize = true;
            this.file_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_label.Location = new System.Drawing.Point(285, 72);
            this.file_label.Name = "file_label";
            this.file_label.Size = new System.Drawing.Size(47, 20);
            this.file_label.TabIndex = 6;
            this.file_label.Text = "None";
            // 
            // button_import_and_display
            // 
            this.button_import_and_display.Location = new System.Drawing.Point(79, 41);
            this.button_import_and_display.Name = "button_import_and_display";
            this.button_import_and_display.Size = new System.Drawing.Size(130, 51);
            this.button_import_and_display.TabIndex = 4;
            this.button_import_and_display.Text = "Import And Display";
            this.button_import_and_display.UseVisualStyleBackColor = true;
            this.button_import_and_display.Click += new System.EventHandler(this.button_import_and_display_Click);
            // 
            // button_solve
            // 
            this.button_solve.Location = new System.Drawing.Point(794, 540);
            this.button_solve.Name = "button_solve";
            this.button_solve.Size = new System.Drawing.Size(130, 51);
            this.button_solve.TabIndex = 8;
            this.button_solve.Text = "Solve";
            this.button_solve.UseVisualStyleBackColor = true;
            this.button_solve.Click += new System.EventHandler(this.button_solve_Click);
            // 
            // Page_Solve_Board
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_solve);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.file_label);
            this.Controls.Add(this.button_import_and_display);
            this.Name = "Page_Solve_Board";
            this.Size = new System.Drawing.Size(961, 606);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label file_label;
        private System.Windows.Forms.Button button_import_and_display;
        private System.Windows.Forms.Button button_solve;
    }
}
