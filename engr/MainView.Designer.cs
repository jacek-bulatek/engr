namespace engr
{
    partial class MainView
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.canvas = new System.Windows.Forms.PictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.choosePicButt = new System.Windows.Forms.Button();
            this.refreshButt = new System.Windows.Forms.Button();
            this.startButt = new System.Windows.Forms.Button();
            this.stopButt = new System.Windows.Forms.Button();
            this.agentNoBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(225, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(500, 500);
            this.canvas.TabIndex = 0;
            this.canvas.TabStop = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "txt";
            this.openFileDialog.FileName = "fDestData";
            // 
            // choosePicButt
            // 
            this.choosePicButt.Location = new System.Drawing.Point(12, 12);
            this.choosePicButt.Name = "choosePicButt";
            this.choosePicButt.Size = new System.Drawing.Size(100, 25);
            this.choosePicButt.TabIndex = 1;
            this.choosePicButt.Text = "Choose Picture";
            this.choosePicButt.UseVisualStyleBackColor = true;
            this.choosePicButt.Click += new System.EventHandler(this.choosePicButt_Click);
            // 
            // refreshButt
            // 
            this.refreshButt.Location = new System.Drawing.Point(12, 43);
            this.refreshButt.Name = "refreshButt";
            this.refreshButt.Size = new System.Drawing.Size(100, 25);
            this.refreshButt.TabIndex = 2;
            this.refreshButt.Text = "Refresh";
            this.refreshButt.UseVisualStyleBackColor = true;
            this.refreshButt.Click += new System.EventHandler(this.refreshButt_Click);
            // 
            // startButt
            // 
            this.startButt.Location = new System.Drawing.Point(12, 74);
            this.startButt.Name = "startButt";
            this.startButt.Size = new System.Drawing.Size(100, 25);
            this.startButt.TabIndex = 3;
            this.startButt.Text = "Start";
            this.startButt.UseVisualStyleBackColor = true;
            this.startButt.Click += new System.EventHandler(this.startButt_Click);
            // 
            // stopButt
            // 
            this.stopButt.Location = new System.Drawing.Point(12, 105);
            this.stopButt.Name = "stopButt";
            this.stopButt.Size = new System.Drawing.Size(100, 25);
            this.stopButt.TabIndex = 4;
            this.stopButt.Text = "Stop";
            this.stopButt.UseVisualStyleBackColor = true;
            this.stopButt.Click += new System.EventHandler(this.stopButt_Click);
            // 
            // agentNoBox
            // 
            this.agentNoBox.Location = new System.Drawing.Point(119, 13);
            this.agentNoBox.Name = "agentNoBox";
            this.agentNoBox.Size = new System.Drawing.Size(100, 20);
            this.agentNoBox.TabIndex = 5;
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 521);
            this.Controls.Add(this.agentNoBox);
            this.Controls.Add(this.stopButt);
            this.Controls.Add(this.startButt);
            this.Controls.Add(this.refreshButt);
            this.Controls.Add(this.choosePicButt);
            this.Controls.Add(this.canvas);
            this.Name = "MainView";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button choosePicButt;
        private System.Windows.Forms.Button refreshButt;
        private System.Windows.Forms.Button startButt;
        private System.Windows.Forms.Button stopButt;
        private System.Windows.Forms.TextBox agentNoBox;
    }
}

