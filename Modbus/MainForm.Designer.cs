namespace OvensManager
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStartReading = new Button();
            lblReg1 = new Label();
            lblReg2 = new Label();
            lblReg3 = new Label();
            lblReg4 = new Label();
            lblReg5 = new Label();
            lblReg6 = new Label();
            lblReg7 = new Label();
            lblReg0 = new Label();
            btnStop = new Button();
            lblStatus = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStartReading
            // 
            btnStartReading.Location = new Point(12, 671);
            btnStartReading.Name = "btnStartReading";
            btnStartReading.Size = new Size(149, 67);
            btnStartReading.TabIndex = 0;
            btnStartReading.Text = "Start";
            btnStartReading.UseVisualStyleBackColor = true;
            btnStartReading.Click += btnStartReading_Click;
            // 
            // lblReg1
            // 
            lblReg1.AutoSize = true;
            lblReg1.BackColor = Color.Black;
            lblReg1.Font = new Font("Consolas", 19.8000011F);
            lblReg1.ForeColor = Color.Red;
            lblReg1.Location = new Point(3, 178);
            lblReg1.Name = "lblReg1";
            lblReg1.Size = new Size(93, 40);
            lblReg1.TabIndex = 1;
            lblReg1.Text = "0000";
            // 
            // lblReg2
            // 
            lblReg2.AutoSize = true;
            lblReg2.BackColor = Color.Black;
            lblReg2.Font = new Font("Consolas", 19.8000011F);
            lblReg2.ForeColor = Color.Red;
            lblReg2.Location = new Point(3, 264);
            lblReg2.Name = "lblReg2";
            lblReg2.Size = new Size(93, 40);
            lblReg2.TabIndex = 1;
            lblReg2.Text = "0000";
            // 
            // lblReg3
            // 
            lblReg3.AutoSize = true;
            lblReg3.BackColor = Color.Black;
            lblReg3.Font = new Font("Consolas", 19.8000011F);
            lblReg3.ForeColor = Color.Red;
            lblReg3.Location = new Point(3, 350);
            lblReg3.Name = "lblReg3";
            lblReg3.Size = new Size(93, 40);
            lblReg3.TabIndex = 1;
            lblReg3.Text = "0000";
            // 
            // lblReg4
            // 
            lblReg4.AutoSize = true;
            lblReg4.BackColor = Color.Black;
            lblReg4.Font = new Font("Consolas", 19.8000011F);
            lblReg4.ForeColor = Color.Red;
            lblReg4.Location = new Point(409, 92);
            lblReg4.Name = "lblReg4";
            lblReg4.Size = new Size(93, 40);
            lblReg4.TabIndex = 1;
            lblReg4.Text = "0000";
            // 
            // lblReg5
            // 
            lblReg5.AutoSize = true;
            lblReg5.BackColor = Color.Black;
            lblReg5.Font = new Font("Consolas", 19.8000011F);
            lblReg5.ForeColor = Color.Red;
            lblReg5.Location = new Point(409, 178);
            lblReg5.Name = "lblReg5";
            lblReg5.Size = new Size(93, 40);
            lblReg5.TabIndex = 1;
            lblReg5.Text = "0000";
            // 
            // lblReg6
            // 
            lblReg6.AutoSize = true;
            lblReg6.BackColor = Color.Black;
            lblReg6.Font = new Font("Consolas", 19.8000011F);
            lblReg6.ForeColor = Color.Red;
            lblReg6.Location = new Point(409, 264);
            lblReg6.Name = "lblReg6";
            lblReg6.Size = new Size(93, 40);
            lblReg6.TabIndex = 1;
            lblReg6.Text = "0000";
            // 
            // lblReg7
            // 
            lblReg7.AutoSize = true;
            lblReg7.BackColor = Color.Black;
            lblReg7.Font = new Font("Consolas", 19.8000011F);
            lblReg7.ForeColor = Color.Red;
            lblReg7.Location = new Point(409, 350);
            lblReg7.Name = "lblReg7";
            lblReg7.Size = new Size(93, 40);
            lblReg7.TabIndex = 1;
            lblReg7.Text = "0000";
            // 
            // lblReg0
            // 
            lblReg0.AutoSize = true;
            lblReg0.BackColor = Color.Black;
            lblReg0.Font = new Font("Consolas", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblReg0.ForeColor = Color.Red;
            lblReg0.Location = new Point(3, 92);
            lblReg0.Name = "lblReg0";
            lblReg0.Size = new Size(93, 40);
            lblReg0.TabIndex = 1;
            lblReg0.Text = "0000";
            // 
            // btnStop
            // 
            btnStop.Location = new Point(192, 671);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(149, 67);
            btnStop.TabIndex = 0;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 634);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label1, 3);
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(3, 46);
            label1.Name = "label1";
            label1.Size = new Size(1215, 46);
            label1.TabIndex = 3;
            label1.Text = "температура";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label2, 3);
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(3, 132);
            label2.Name = "label2";
            label2.Size = new Size(1215, 46);
            label2.TabIndex = 3;
            label2.Text = "Номер текущей Программы технолога";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label3, 3);
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(3, 218);
            label3.Name = "label3";
            label3.Size = new Size(1215, 46);
            label3.TabIndex = 3;
            label3.Text = "Номер текущего шага Программы технолога";
            label3.TextAlign = ContentAlignment.TopCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label4, 3);
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(3, 304);
            label4.Name = "label4";
            label4.Size = new Size(1215, 46);
            label4.TabIndex = 3;
            label4.Text = "Режим работы прибора";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.Controls.Add(label5, 0, 0);
            tableLayoutPanel1.Controls.Add(label4, 0, 7);
            tableLayoutPanel1.Controls.Add(lblReg7, 1, 8);
            tableLayoutPanel1.Controls.Add(label6, 1, 0);
            tableLayoutPanel1.Controls.Add(lblReg3, 0, 8);
            tableLayoutPanel1.Controls.Add(label3, 0, 5);
            tableLayoutPanel1.Controls.Add(label7, 2, 0);
            tableLayoutPanel1.Controls.Add(lblReg6, 1, 6);
            tableLayoutPanel1.Controls.Add(label2, 0, 3);
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(lblReg2, 0, 6);
            tableLayoutPanel1.Controls.Add(lblReg0, 0, 2);
            tableLayoutPanel1.Controls.Add(lblReg5, 1, 4);
            tableLayoutPanel1.Controls.Add(lblReg4, 1, 2);
            tableLayoutPanel1.Controls.Add(lblReg1, 0, 4);
            tableLayoutPanel1.Location = new Point(1, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 9;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1221, 614);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(400, 46);
            label5.TabIndex = 3;
            label5.Text = "№ 1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label6.Location = new Point(409, 0);
            label6.Name = "label6";
            label6.Size = new Size(401, 46);
            label6.TabIndex = 3;
            label6.Text = "№ 2";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label7.Location = new Point(816, 0);
            label7.Name = "label7";
            label7.Size = new Size(402, 46);
            label7.TabIndex = 3;
            label7.Text = "№ 3";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1234, 750);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(lblStatus);
            Controls.Add(btnStop);
            Controls.Add(btnStartReading);
            Name = "MainForm";
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStartReading;
        private Label lblReg1;
        private Label lblReg2;
        private Label lblReg3;
        private Label lblReg4;
        private Label lblReg5;
        private Label lblReg6;
        private Label lblReg7;
        private Label lblReg0;
        private Button btnStop;
        private Label lblStatus;
        private Label label1;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label3;
        private Label label4;
    }
}
