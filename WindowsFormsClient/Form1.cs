using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;


namespace WindowsFormsClient
{
    public partial class Form1 : Form
    {
        private IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://192.168.1.44:8080/signalr";
        private HubConnection Connection { get; set; }
        private bool isPaused { get; set; }
        private int delay;
        private List<DataElem> buffer;
        private bool writeToBuffer; // true if client is paused or is resumed (from buffer) 
        private Thread showFromBufferThread;
        private Series seriesRandomNumber;

        public Form1()
        {
            InitializeComponent();
            initChart();
            isPaused = false;
            writeToBuffer = false;
            buttonResumeRealFlow.Visible = false;
            buffer = new List<DataElem>();
        }

        private void initChart()
        {
            chartRandomNumber.Series.Clear();
            seriesRandomNumber = new Series
            {
                Name = "SeriesRandomNumber",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            chartRandomNumber.Series.Add(seriesRandomNumber);
            chartRandomNumber.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
        }

        private void AddPointToChartRandomNumber(DateTime x, int y)
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                if (seriesRandomNumber.Points.Count > 30)
                    seriesRandomNumber.Points.RemoveAt(0);
                seriesRandomNumber.Points.AddXY(x, y);
                chartRandomNumber.Invalidate();
            }));
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Connecting to server...";
            buttonConnect.Enabled = false;
            ConnectAsync();
        }

        private void SetLabelNumberText(string text)
        {
            this.Invoke((Action) (() => labelNumber.Text = text));
        }

        private void SetLabelServerTimeText(string text)
        {
            this.Invoke((Action) (() => labelServerTime.Text = text));
        }

        private void ShowDataFromBuffer()
        {
            while (true)
            {
                if (!isPaused)
                {
                    Thread.Sleep(this.delay);
                    SetLabelNumberText(buffer.FirstOrDefault().Number.ToString());
                    SetLabelServerTimeText(buffer.FirstOrDefault().ServerTime.ToString());
                    AddPointToChartRandomNumber(buffer.FirstOrDefault().ServerTime, buffer.FirstOrDefault().Number);
                    buffer.RemoveAt(0);
                }
                else
                {
                    Thread.Sleep(this.delay);
                }
                this.Invoke((Action)(() => labelBufferSize.Text = buffer.Count.ToString()));
            }
        }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("MyHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread


            try
            {
                HubProxy.On<int, DateTime>("broadcastData", (number, currentTime) =>
                {
                    if (writeToBuffer)
                    {
                        buffer.Add(new DataElem() { Number = number, ServerTime = currentTime});
                        this.Invoke((Action)(() => labelBufferSize.Text = buffer.Count.ToString()));
                    }
                    else
                    {
                        SetLabelNumberText(number.ToString());
                        SetLabelServerTimeText(currentTime.ToString());
                        AddPointToChartRandomNumber(currentTime, number);
                    }                        
                });

                HubProxy.On<int>("setDelay", (miliseconds) => this.delay = miliseconds);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("*** Exeption while receiving messages *** \n" + ex);
            }
            

            try
            {
                await Connection.Start();           
            }
            catch (HttpRequestException)
            {
                labelStatus.Text = "Unable to connect to server: Start server before connecting clients.";
                this.Invoke((Action)(() => buttonConnect.Enabled = true));
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            labelStatus.Text = "Connected to server at " + ServerURI;
            this.Invoke((Action)(() => buttonConnect.Visible = false));
        }

        /// <summary>
        /// If the server is stopped, the connection will time out after 30 seconds (default), and the 
        /// Closed event will fire.
        /// </summary>
        private void Connection_Closed()
        {
            if (showFromBufferThread != null)
                if (showFromBufferThread.IsAlive)
                    showFromBufferThread.Abort();
            this.Invoke((Action)(() => labelStatus.Text = "You have been disconnected."));
            this.Invoke((Action)(() => buttonConnect.Visible = true));
            this.Invoke((Action)(() => buttonConnect.Enabled = true));
        }

        private void StartShowDataFromBufferThread()
        {
            showFromBufferThread = new Thread(new ThreadStart(ShowDataFromBuffer));
            showFromBufferThread.Start();   
        }

        private void buttonPauseResume_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                writeToBuffer = true;
                buttonPauseResume.Text = "Resume(from buffer)";
                buttonResumeRealFlow.Visible = true;
            }
            else
            {
                buttonPauseResume.Text = "Pause";
                if (showFromBufferThread != null)
                {
                    if (!showFromBufferThread.IsAlive)
                    {
                        StartShowDataFromBufferThread();
                    }
                }
                else
                {
                    StartShowDataFromBufferThread();
                }
            }
            HubProxy.Invoke("SendStatus", isPaused);
        }

        private void buttonResumeRealFlow_Click(object sender, EventArgs e)
        {
            if (showFromBufferThread != null)
                if (showFromBufferThread.IsAlive)
                    showFromBufferThread.Abort();
            isPaused = false;
            buttonPauseResume.Text = "Pause";
            writeToBuffer = false;
            buttonResumeRealFlow.Visible = false;
            buffer.Clear();
            this.Invoke((Action)(() => labelBufferSize.Text = buffer.Count.ToString()));
        }

    }
}
