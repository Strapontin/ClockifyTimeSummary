namespace ClockifyAPI
{
    partial class FormDisplayTimes
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartingDate = new System.Windows.Forms.TextBox();
            this.dgvClockifyTimes = new System.Windows.Forms.DataGridView();
            this.Project = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeSinceStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClockifyTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.btnRefresh.Enabled = false;
            this.btnRefresh.Location = new System.Drawing.Point(12, 12);
            this.btnRefresh.Name = "button1";
            this.btnRefresh.Size = new System.Drawing.Size(100, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtStartingDate);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvClockifyTimes);
            this.splitContainer1.Size = new System.Drawing.Size(991, 488);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Date de début";
            // 
            // txtStartingDate
            // 
            this.txtStartingDate.Location = new System.Drawing.Point(12, 89);
            this.txtStartingDate.Name = "txtStartingDate";
            this.txtStartingDate.Size = new System.Drawing.Size(100, 23);
            this.txtStartingDate.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dgvClockifyTimes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClockifyTimes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Project,
            this.TotalTime,
            this.TimeSinceStartDate,
            this.Task});
            this.dgvClockifyTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClockifyTimes.Location = new System.Drawing.Point(0, 0);
            this.dgvClockifyTimes.Name = "dataGridView1";
            this.dgvClockifyTimes.RowTemplate.Height = 25;
            this.dgvClockifyTimes.Size = new System.Drawing.Size(863, 488);
            this.dgvClockifyTimes.TabIndex = 0;
            // 
            // Project
            // 
            this.Project.HeaderText = "Projet";
            this.Project.Name = "Project";
            // 
            // TotalTime
            // 
            this.TotalTime.HeaderText = "Temps Total";
            this.TotalTime.Name = "TotalTime";
            // 
            // TimeSinceStartDate
            // 
            this.TimeSinceStartDate.HeaderText = "Temps Depuis Date Début";
            this.TimeSinceStartDate.Name = "TimeSinceStartDate";
            this.TimeSinceStartDate.Width = 120;
            // 
            // Task
            // 
            this.Task.HeaderText = "Tache";
            this.Task.Name = "Task";
            this.Task.Width = 500;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 488);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormDisplayTimes_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClockifyTimes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnRefresh;
        private SplitContainer splitContainer1;
        private Label label1;
        private TextBox txtStartingDate;
        private DataGridView dgvClockifyTimes;
        private DataGridViewTextBoxColumn Project;
        private DataGridViewTextBoxColumn TotalTime;
        private DataGridViewTextBoxColumn TimeSinceStartDate;
        private DataGridViewTextBoxColumn Task;
    }
}