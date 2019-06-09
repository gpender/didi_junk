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
using System.Linq;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DidiWebSocketTest.ViewModels
{
    public class ScopeVM : BaseVM
    {
        ScopeProtocol protocol;
        string message ="";
        RelayCommand<ScopeCommandName> scopeCommand;
        AxisCollection yAxes = new AxisCollection();
        TimeSpanAxis xAxis = new TimeSpanAxis();
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

            _timerNewDataUpdate = new Timer(dt * 1000);
            _timerNewDataUpdate.AutoReset = true;
            _timerNewDataUpdate.Elapsed += OnNewData;
            CreateDataSetAndSeries();
        }

        private void Protocol_OnInfo(object sender, string e)
        {
            Message = e;
        }
        private void Protocol_OnMessage(object sender, MessageBase msg)
        {
            ScopeBufferMessage scopeBufferMsg = msg as ScopeBufferMessage;
            if (scopeBufferMsg != null)
            {
                //scopeBufferMsg.ChannelCount
                List<ByteEntry> byteEntries = GetTimeStampedValues(scopeBufferMsg.Data, 2);// scopeBufferMsg.ChannelCount);
                int ms = (int)TimeSpan.Zero.TotalMilliseconds;
                foreach (var be in byteEntries)
                {
                    int index = 0;
                    TimeSpan ts = TimeSpan.FromMilliseconds(ms);
                    //float y1 = (float)(3.0 * Math.Sin(((2 * Math.PI) * 1.4) * ms) + _random.NextDouble() * 0.5);
                    //float y2 = (float)(2.0 * Math.Cos(((2 * Math.PI) * 0.8) * ms) + _random.NextDouble() * 0.5);
                    //((XyDataSeries<TimeSpan, float>)ChannelDataVMCollection[0].DataSeries).Append(ts, y1);
                    //((XyDataSeries<TimeSpan, float>)ChannelDataVMCollection[1].DataSeries).Append(ts, y2);
                    foreach (var d in be.Values)
                    {

                        ((XyDataSeries<TimeSpan, float>)ChannelDataVMCollection[index].DataSeries).Append(be.TimeStamp, d);
                        //((IXyDataSeries<double, double>)ChannelDataVMCollection[index].DataSeries).Append(be.TimeStamp.Ticks, d);
                        index++;
                    }
                    ms++;
                }
                //// Compute our three series values
                //double y1 = 3.0 * Math.Sin(((2 * Math.PI) * 1.4) * t) + _random.NextDouble() * 0.5;
                //double y2 = 2.0 * Math.Cos(((2 * Math.PI) * 0.8) * t) + _random.NextDouble() * 0.5;
                //double y3 = 1.0 * Math.Sin(((2 * Math.PI) * 2.2) * t) + _random.NextDouble() * 0.5;

                //// Suspending updates is optional, and ensures we only get one redraw
                //// once all three dataseries have been appended to
                ////using (sciChart.SuspendUpdates())
                //{
                //    // Append x,y data to previously created series
                //    series0.Append(t, y1);
                //    series1.Append(t, y2);
                //    series2.Append(t, y3);
                //}

                //// Increment current time
                //t += dt;

            }
            Message = $"{DateTime.Now.ToLongTimeString()} Data received";
        }
        private List<ByteEntry> GetTimeStampedValues(float[] bytes, int numberOfChannels)
        {
            List<ByteEntry> entries = new List<ByteEntry>();
            int entrySize = 1 + numberOfChannels;// 4 + ((int)numberOfChannels * 4);
            int numEntries = Convert.ToInt32(bytes.Length / entrySize);
            int index = 0;
            int offset = 0;
            while (index < numEntries)
            {
                //float time = bytes[offset];// (long)(GetSingle(bytes, offset));
                long ticks = (long)(bytes[offset] * 10000000); // long.Parse(time.ToString());
                if (ticks < 0) ticks = 0;
                if (ticks > TimeSpan.MaxValue.Ticks) ticks = TimeSpan.MaxValue.Ticks;
                ByteEntry be = new ByteEntry(TimeSpan.FromTicks(ticks));
                List<byte> tmp = new List<byte>();
                for (int j = 1; j <= numberOfChannels; j++)
                {
                    float val = bytes[offset + j];// GetSingle(bytes, offset + (j * 4));
                    //if (((IScopeChannel2)CurrentScopeChannels[j - 1]).ParameterType == "16")
                    //{
                    //    val = val / 1000;
                    //}
                    be.Values.Add(val);
                }
                entries.Add(be);
                offset = offset + entrySize;
                index++;
            }
            //if (SingleCapture)
            //{
            //    entries.Sort();
            //}
            return entries;
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
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = series0
            });
            ChannelDataVMCollection.Add(new ChannelDataVM(1)
            {
                StrokeThickness = 2,
                Stroke = Colors.SteelBlue,
                DataSeries = series1
            });


            // Set the dataseries on the chart's RenderableSeries
            //RenderableSeries0.DataSeries = series0;
            //RenderableSeries1.DataSeries = series1;
            //RenderableSeries2.DataSeries = series2;
            YAxes.Add(ChannelDataVMCollection[0].YAxis);
            YAxes.Add(ChannelDataVMCollection[1].YAxis);

        }
        private void OnNewData(object sender, EventArgs e)
        {
            //// Compute our three series values
            //double y1 = 3.0 * Math.Sin(((2 * Math.PI) * 1.4) * t) + _random.NextDouble() * 0.5;
            //double y2 = 2.0 * Math.Cos(((2 * Math.PI) * 0.8) * t) + _random.NextDouble() * 0.5;
            //double y3 = 1.0 * Math.Sin(((2 * Math.PI) * 2.2) * t) + _random.NextDouble() * 0.5;

            //// Suspending updates is optional, and ensures we only get one redraw
            //// once all three dataseries have been appended to
            ////using (sciChart.SuspendUpdates())
            //{
            //    // Append x,y data to previously created series
            //    series0.Append(t, y1);
            //    series1.Append(t, y2);
            //    series2.Append(t, y3);
            //}

            //// Increment current time
            //t += dt;
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
                    //protocol.Close();
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
    }
}
