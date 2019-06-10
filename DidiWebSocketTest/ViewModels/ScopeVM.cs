using DidiWebSocketTest.Commands;
using DidiWebSocketTest.Models;
using DidiWebSocketTest.Models.Messages;
using SciChart.Charting.Model;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DidiWebSocketTest.ViewModels
{
    public class ScopeVM : BaseVM
    {
        ScopeProtocol protocol;
        string message ="";
        RelayCommand<ScopeCommandName> scopeCommand;
        AxisCollection yAxes = new AxisCollection();
        TimeSpanAxis xAxis = new TimeSpanAxis() { AutoRange = AutoRange.Always };
        ObservableCollection<ChannelDataVM> channelDataVMCollection = new ObservableCollection<ChannelDataVM>();
        public ObservableCollection<ChannelDataVM> ChannelDataVMCollection { get { return channelDataVMCollection; } }
        public int ChannelCount { get; set; }
        public string Message
        {
            get { return message; }
            set
            {
                message = value + Environment.NewLine + message;
                OnPropertyChanged("Message");
            }
        }
        public IViewportManager ViewportManager { get; set; }
        public AxisCollection YAxes
        {
            get { return yAxes; }
            set
            {
                yAxes = value;
                OnPropertyChanged("YAxes");
            }
        }
        public TimeSpanAxis XAxis
        {
            get { return xAxis; }
        }
        public ScopeVM(ScopeProtocol protocol)
        {
            this.protocol = protocol;
            this.protocol.OnMessage += Protocol_OnMessage;
            this.protocol.OnError += Protocol_OnInfo;
            this.protocol.OnInfo += Protocol_OnInfo;
            ViewportManager = new DefaultViewportManager();
            CreateDataSetAndSeries();
        }

        private void Protocol_OnInfo(object sender, string e)
        {
            Message = e;
        }
        TimeSpan lastMaxTime = TimeSpan.Zero;
        private void Protocol_OnMessage(object sender, MessageBase msg)
        {
            ScopeBufferMessage scopeBufferMsg = msg as ScopeBufferMessage;
            if(scopeBufferMsg != null)
            {
                for(int i=0; i < scopeBufferMsg.SampleCount; i = i + scopeBufferMsg.ChannelCount + 1)
                {
                    TimeSpan ts = TimeSpan.Zero;
                    try { ts = TimeSpan.FromSeconds(scopeBufferMsg.Data[i]); }catch { }
                    lastMaxTime = ts.Ticks > lastMaxTime.Ticks ? ts : lastMaxTime;
                    for(int j = 1; j <= scopeBufferMsg.ChannelCount; j++)
                    {
                        float val = ts!=TimeSpan.Zero ? scopeBufferMsg.Data[i + j] : float.NaN;
                        ((XyDataSeries<TimeSpan, float>)ChannelDataVMCollection[j-1].DataSeries).Append(ts,val);
                        if(i==0)AddNullData(ChannelDataVMCollection[j - 1].DataSeries);
                    }
                }
                Message = $"{DateTime.Now.ToLongTimeString()} Data received";
            }
        }
        internal void AddNullData(IDataSeries dataSeries)
        {
            int count = dataSeries.XValues.Count;
            if (count > 0)
            {
                TimeSpan lastTimeValue = (TimeSpan)dataSeries.XValues[count - 1];
                lastTimeValue.Add(new TimeSpan(1));
                ((XyDataSeries<TimeSpan, float>)dataSeries).Append(lastTimeValue, float.NaN);
            }
        }

        internal class ByteEntry : IComparable, ICloneable
        {
            public TimeSpan TimeStamp { get; set; }
            public List<float> Values { get; set; }
            public ByteEntry(TimeSpan timeStamp)
            {
                TimeStamp = timeStamp;
                Values = new List<float>();
            }
            public override string ToString()
            {
                return TimeStamp.ToString() + " : " + Values.ToString();
            }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;
                ByteEntry other = obj as ByteEntry;
                return this.TimeStamp.CompareTo(other.TimeStamp);
            }

            public int Compare(ByteEntry x, ByteEntry y)
            {
                if (x.TimeStamp == y.TimeStamp)
                {
                    return 0;
                }
                if (x.TimeStamp > y.TimeStamp)
                {
                    return 1;
                }
                return -1;
            }

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }


        // Data Sample Rate (sec)  - 20 Hz
        private double dt = 0.02;

        // The current time
        private double t;
        private Timer _timerNewDataUpdate;
        Random _random = new Random();
        private int FifoSize = 20000;

        // The dataseries to fill
        private IXyDataSeries<TimeSpan, float> series0;
        private IXyDataSeries<TimeSpan, float> series1;
        private IXyDataSeries<TimeSpan, float> series2;
        private ObservableCollection<IRenderableSeriesViewModel> _renderableSeries;
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries
        {
            get { return _renderableSeries; }
            set
            {
                _renderableSeries = value;
                OnPropertyChanged("RenderableSeries");
            }
        }
        private void CreateDataSetAndSeries()
        {
            //var lineData = new XyDataSeries<TimeSpan, float>() { SeriesName = "TestingSeries" };
            //lineData.Append(TimeSpan.FromSeconds(0), 0);
            //lineData.Append(TimeSpan.FromSeconds(1), 1);
            //lineData.Append(TimeSpan.FromSeconds(2), 2);
            //var lineData1 = new XyDataSeries<TimeSpan, float>() { SeriesName = "TestingSeries2" };
            //lineData1.Append(TimeSpan.FromSeconds(0), 3);
            //lineData1.Append(TimeSpan.FromSeconds(1), 2);
            //lineData1.Append(TimeSpan.FromSeconds(2), 1);

            ////_renderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();
            ////RenderableSeries.Add(new LineRenderableSeriesViewModel()
            ////{
            ////    StrokeThickness = 2,
            ////    Stroke = Colors.SteelBlue,
            ////    DataSeries = lineData,
            ////});
            ////RenderableSeries.Add(new ChannelDataVM(0)
            ////{
            ////    StrokeThickness = 2,
            ////    Stroke = Colors.SteelBlue,
            ////    DataSeries = lineData,
            ////});
            //ChannelDataVMCollection.Add(new ChannelDataVM(0)
            //{
            //    StrokeThickness = 2,
            //    Stroke = Colors.SteelBlue,
            //    DataSeries = lineData,
            //});
            //ChannelDataVMCollection.Add(new ChannelDataVM(1)
            //{
            //    StrokeThickness = 2,
            //    Stroke = Colors.SteelBlue,
            //    DataSeries = lineData1,
            //});



            //return;
            // Create new Dataseries of type X=double, Y=double
            var series0 = new XyDataSeries<TimeSpan, float>() { AcceptsUnsortedData = true, FifoCapacity = FifoSize, SeriesName = "TestingSeries" };
            //series0.Append(TimeSpan.FromSeconds(0), 0);
            //series0.Append(TimeSpan.FromSeconds(1), 1);
            //series0.Append(TimeSpan.FromSeconds(2), 2);
            var series1 = new XyDataSeries<TimeSpan, float>() { AcceptsUnsortedData = true, FifoCapacity = FifoSize, SeriesName = "TestingSeries2" };
            //series1.Append(TimeSpan.FromSeconds(0), 3);
            //series1.Append(TimeSpan.FromSeconds(1), 2);
            //series1.Append(TimeSpan.FromSeconds(2), 1);

            //series2 = new XyDataSeries<TimeSpan, double>();
            //series0.AcceptsUnsortedData = true;
            //series1.AcceptsUnsortedData = true;
            //series2.AcceptsUnsortedData = true;
            //if (IsFifoCheckBox.IsChecked == true)
            {
                // Add three FIFO series to fill with data.                 
                // setting the FIFO capacity will denote this series as a FIFO series. New data is appended until the size is met, at which point
                //  old data is discarded. Internally the FIFO series is implemented as a circular buffer so that old data is pushed out of the buffer
                //  once the capacity has been reached
                // Note: Once a FIFO series has been added to a dataset, all subsequent series must be FIFO series. In addition, the FifoSize must be the
                //  same for all FIFO series in a dataset. 
                //series0.FifoCapacity = FifoSize;
                //series1.FifoCapacity = FifoSize;
                //series2.FifoCapacity = FifoSize;
            }

            ChannelDataVMCollection.Add(new ChannelDataVM(0)
            {
                //StrokeThickness = 1,
                //Stroke = Colors.SteelBlue,
                DataSeries = series0
            });
            ChannelDataVMCollection.Add(new ChannelDataVM(1)
            {
                //StrokeThickness = 1,
                //Stroke = Colors.SteelBlue,
                DataSeries = series1
            });


            // Set the dataseries on the chart's RenderableSeries
            //RenderableSeries0.DataSeries = series0;
            //RenderableSeries1.DataSeries = series1;
            //RenderableSeries2.DataSeries = series2;
            YAxes.Add(ChannelDataVMCollection[0].YAxis);
            YAxes.Add(ChannelDataVMCollection[1].YAxis);

        }
        private void ClearDataSeries()
        {
            if (series0 == null)
                return;

            //using (sciChart.SuspendUpdates())
            {
                series0.Clear();
                series1.Clear();
                //series2.Clear();
            }
        }

        #region Commands
        public ICommand ScopeCommand
        {
            get
            {
                if (scopeCommand == null)
                {
                    scopeCommand = new RelayCommand<ScopeCommandName>(ExecuteScopeCommand, CanExecuteScopeCommand);
                }
                return scopeCommand;
            }
        }
        bool CanExecuteScopeCommand(ScopeCommandName messageType)
        {
            return true;
        }
        void ExecuteScopeCommand(ScopeCommandName messageType)
        {
            switch (messageType)
            {
                case ScopeCommandName.START:
                    ClearDataSeries();
                    //_timerNewDataUpdate.Start();
                    protocol.SendMessage(new HelloMessage());
                    //protocol.GetScopeParameters();// SendMessage();
                    break;
                case ScopeCommandName.STOP:
                    //protocol.GetScopeConfigParameters();// SendMessage();
                    protocol.Close();
                    //_timerNewDataUpdate.Stop();

                    break;
                case ScopeCommandName.PAUSE:
                    //_timerNewDataUpdate.Stop();

                    //protocol.GetScopeBuffer();// SendMessage();
                    break;
            }
        }
        #endregion



        private void SendGetData()
        {
            string driveDisk = "z";
            string ipAddress = "169.254.3.17";
            int tag = 464;
            string trueString = "1";
            string falseString = "0";
            string SET_PARAMETER_VALUE = $"http://{ipAddress}:8080/{driveDisk}:/restricted/parameters.act?V{tag.ToString()}={trueString}";


        }
    }
}
