namespace WindowsFormsClient
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelNumber = new System.Windows.Forms.Label();
            this.buttonPauseResume = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelServerTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonResumeRealFlow = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelBufferSize = new System.Windows.Forms.Label();
            this.chartRandomNumber = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartRandomNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(13, 13);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Status:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 29);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Location = new System.Drawing.Point(62, 67);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(16, 13);
            this.labelNumber.TabIndex = 2;
            this.labelNumber.Text = "...";
            // 
            // buttonPauseResume
            // 
            this.buttonPauseResume.Location = new System.Drawing.Point(12, 463);
            this.buttonPauseResume.Name = "buttonPauseResume";
            this.buttonPauseResume.Size = new System.Drawing.Size(99, 23);
            this.buttonPauseResume.TabIndex = 3;
            this.buttonPauseResume.Text = "Pause";
            this.buttonPauseResume.UseVisualStyleBackColor = true;
            this.buttonPauseResume.Click += new System.EventHandler(this.buttonPauseResume_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Server time:";
            // 
            // labelServerTime
            // 
            this.labelServerTime.AutoSize = true;
            this.labelServerTime.Location = new System.Drawing.Point(78, 96);
            this.labelServerTime.Name = "labelServerTime";
            this.labelServerTime.Size = new System.Drawing.Size(16, 13);
            this.labelServerTime.TabIndex = 5;
            this.labelServerTime.Text = "...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Number:";
            // 
            // buttonResumeRealFlow
            // 
            this.buttonResumeRealFlow.Location = new System.Drawing.Point(117, 463);
            this.buttonResumeRealFlow.Name = "buttonResumeRealFlow";
            this.buttonResumeRealFlow.Size = new System.Drawing.Size(75, 23);
            this.buttonResumeRealFlow.TabIndex = 7;
            this.buttonResumeRealFlow.Text = "Resume real";
            this.buttonResumeRealFlow.UseVisualStyleBackColor = true;
            this.buttonResumeRealFlow.Click += new System.EventHandler(this.buttonResumeRealFlow_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Buffer size:";
            // 
            // labelBufferSize
            // 
            this.labelBufferSize.AutoSize = true;
            this.labelBufferSize.Location = new System.Drawing.Point(78, 125);
            this.labelBufferSize.Name = "labelBufferSize";
            this.labelBufferSize.Size = new System.Drawing.Size(16, 13);
            this.labelBufferSize.TabIndex = 9;
            this.labelBufferSize.Text = "...";
            // 
            // chartRandomNumber
            // 
            chartArea1.Name = "ChartArea1";
            this.chartRandomNumber.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRandomNumber.Legends.Add(legend1);
            this.chartRandomNumber.Location = new System.Drawing.Point(16, 141);
            this.chartRandomNumber.Name = "chartRandomNumber";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRandomNumber.Series.Add(series1);
            this.chartRandomNumber.Size = new System.Drawing.Size(611, 300);
            this.chartRandomNumber.TabIndex = 10;
            this.chartRandomNumber.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 498);
            this.Controls.Add(this.chartRandomNumber);
            this.Controls.Add(this.labelBufferSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonResumeRealFlow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelServerTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPauseResume);
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.labelStatus);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chartRandomNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.Button buttonPauseResume;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelServerTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonResumeRealFlow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelBufferSize;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRandomNumber;
    }
}

