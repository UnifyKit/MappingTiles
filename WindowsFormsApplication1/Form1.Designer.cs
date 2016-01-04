namespace WindowsFormsApplication1
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.map1 = new MappingTiles.Winforms.Map();
            this.SuspendLayout();
            // 
            // map1
            // 
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.Crs = null;
            this.map1.Location = new System.Drawing.Point(1, 1);
            this.map1.Name = "map1";
            this.map1.Size = new System.Drawing.Size(713, 533);
            this.map1.TabIndex = 0;
            this.map1.Text = "map1";
            this.map1.Viewport = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 535);
            this.Controls.Add(this.map1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MappingTiles.Winforms.Map map1;
    }
}

