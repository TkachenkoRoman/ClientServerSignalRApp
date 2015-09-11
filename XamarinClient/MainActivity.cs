using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OxyPlot.Xamarin.Android;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Axes;

namespace XamarinClient
{
    [Activity(Label = "XamarinClient", MainLauncher = true, Icon = "@drawable/Icon-chart", Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : Activity
    {
        // ui
        private Button buttonConnect;
        private Button buttonPauseResume;
        private Button buttonResumeRealFlow;
        private TextView textViewStatus;
        private TextView textViewNumber;
        private TextView textViewServerTime;
        private TextView textViewBufferSize;
        private LinearLayout linearLayoutChart;

        // for chart
        private LineSeries normalPointsSeries;
        private PlotView plotView;

        // signalr connection
        private IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://192.168.1.44:8080/signalr";
        private HubConnection Connection { get; set; }
        private bool isPaused { get; set; }
        private int delay;
        private List<DataElem> buffer;
        private bool writeToBuffer; // true if client is paused or is resumed (from buffer) 
        private Thread showFromBufferThread;
        private double currentTimeOffset;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            IntitializeControls();
            InitializeChart();

            isPaused = false;
            writeToBuffer = false;
            buttonResumeRealFlow.Visibility = ViewStates.Invisible;
            buffer = new List<DataElem>();
        }

        private void InitializeChart()
        {
            plotView = FindViewById<PlotView>(Resource.Id.plotView);
            normalPointsSeries = new LineSeries();

            // Set up our graph
            plotView.Model = new PlotModel();
            plotView.Model.Series.Add(normalPointsSeries);
            plotView.Model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left, 
                Minimum = 0, 
                Maximum = 100,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
            plotView.Model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            UpdateChart();
           
        }

        private void AddPointToChart(int number)
        {
            double offset = 0;
            if (delay != 0)
                offset = ((double)delay)/1000.0;
            currentTimeOffset += offset; // offset in seconds
            try
            {
                if (normalPointsSeries.Points.Count > 30)
                    normalPointsSeries.Points.RemoveAt(0);
                normalPointsSeries.Points.Add(new DataPoint(currentTimeOffset, number));
                UpdateChart();
            }
            catch (Exception ex)
            {
           
            }     
        }

        private void UpdateChart()
        {
            //RunOnUiThread(() => { plotView.InvalidatePlot(); }); // not working?
            RunOnUiThread(() => { plotView.Model.InvalidatePlot(true); });
        }

        private void IntitializeControls()
        {
            buttonConnect = FindViewById<Button>(Resource.Id.buttonConnect);
            buttonPauseResume = FindViewById<Button>(Resource.Id.buttonPauseResume);
            buttonResumeRealFlow = FindViewById<Button>(Resource.Id.buttonResumeRealFlow);
            textViewStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            textViewNumber = FindViewById<TextView>(Resource.Id.textViewNumber);
            textViewServerTime = FindViewById<TextView>(Resource.Id.textViewServerTime);
            textViewBufferSize = FindViewById<TextView>(Resource.Id.textViewBufferSize);
            linearLayoutChart = FindViewById<LinearLayout>(Resource.Id.linearLayoutChart);

            buttonConnect.Click += ButtonConnectOnClick;
            buttonPauseResume.Click += ButtonPauseResumeOnClick;
            buttonResumeRealFlow.Click += ButtonResumeRealFlowOnClick;
        }

        private void ButtonResumeRealFlowOnClick(object sender, EventArgs eventArgs)
        {
            if (showFromBufferThread != null)
                if (showFromBufferThread.IsAlive)
                    showFromBufferThread.Abort();
            isPaused = false;
            RunOnUiThread(() => { buttonPauseResume.Text = "Pause"; });          
            writeToBuffer = false;
            RunOnUiThread(() => { buttonResumeRealFlow.Visibility = ViewStates.Invisible; });           
            buffer.Clear();
            RunOnUiThread(() => { textViewBufferSize.Text = buffer.Count.ToString(); });
        }

        private void ButtonPauseResumeOnClick(object sender, EventArgs eventArgs)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                writeToBuffer = true;
                RunOnUiThread(() => { buttonPauseResume.Text = "Resume(from buffer)"; });
                RunOnUiThread(() => { buttonResumeRealFlow.Visibility = ViewStates.Visible; });
                
            }
            else
            {
                RunOnUiThread(() => { buttonPauseResume.Text = "Pause"; });
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

        private void StartShowDataFromBufferThread()
        {
            showFromBufferThread = new Thread(new ThreadStart(ShowDataFromBuffer));
            showFromBufferThread.Start();   
        }

        private void ShowDataFromBuffer()
        {
            while (true)
            {
                if (!isPaused)
                {
                    Thread.Sleep(this.delay);
                    if (buffer.Count > 0)
                    {
                        SetTextViewNumberText(buffer.First().Number.ToString());
                        SetTextViewServerTimeText(buffer.First().ServerTime.ToString());
                        AddPointToChart(buffer.First().Number);
                        buffer.RemoveAt(0);
                    }               
                }
                else
                {
                    Thread.Sleep(this.delay);
                }
                RunOnUiThread(() => { textViewBufferSize.Text = buffer.Count.ToString(); });
            }
        }

        private void ButtonConnectOnClick(object sender, EventArgs eventArgs)
        {
            RunOnUiThread(() => { textViewStatus.Text = "Status: Connecting to server...";});
            RunOnUiThread(() => { buttonConnect.Enabled = false; });
            ConnectAsync();
        }

        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("MyHub");

            try
            {
                HubProxy.On<int, DateTime>("broadcastData", (number, currentTime) =>
                {
                    if (writeToBuffer)
                    {
                        buffer.Add(new DataElem() { Number = number, ServerTime = currentTime });
                        RunOnUiThread(() => { textViewBufferSize.Text = buffer.Count.ToString(); });
                    }
                    else
                    {
                        SetTextViewNumberText(number.ToString());
                        SetTextViewServerTimeText(currentTime.ToString());
                        //AddPointToChartRandomNumber(currentTimeOffset, number);
                        AddPointToChart(number);
                    }
                });

                HubProxy.On<int>("setDelay", (miliseconds) => this.delay = miliseconds);

            }
            catch (Exception ex)
            {
                //Debug.WriteLine("*** Exeption while receiving messages *** \n" + ex); logging
            }


            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                RunOnUiThread(() => { textViewStatus.Text = "Unable to connect to server: Start server before connecting clients."; });
                RunOnUiThread(() => { buttonConnect.Enabled = true; });
                return;
            }
            RunOnUiThread(() => { textViewStatus.Text = "Connected to server at " + ServerURI; });
            RunOnUiThread(() => { buttonConnect.Enabled = false; });
        }

        private void SetTextViewServerTimeText(string p)
        {
            RunOnUiThread(() => { textViewServerTime.Text = p; });
        }

        private void SetTextViewNumberText(string p)
        {
            RunOnUiThread(() => { textViewNumber.Text = p; });
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
            RunOnUiThread(() => { textViewStatus.Text = "You have been disconnected."; });
            RunOnUiThread(() => { buttonConnect.Enabled = true; });
        }
    }
}

