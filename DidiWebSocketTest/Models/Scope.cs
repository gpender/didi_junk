using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models.Messages;
using DidiWebSocketTest.ViewModels;
using SciChart.Charting.Model;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DidiWebSocketTest.Models
{
    public class Scope : IScope
    {
        public event EventHandler<string> OnInfo;
        public event EventHandler OnData;
        ScopeProtocol protocol;
        TimeSpan lastMaxTime = TimeSpan.Zero;
        int FifoSize = 20000;
        readonly double msGapToleranceForAddingNull = 50;
        IXyDataSeries<TimeSpan, float> series0;
        IXyDataSeries<TimeSpan, float> series1;
        IXyDataSeries<TimeSpan, float> series2;

        AxisCollection yAxes = new AxisCollection();
        TimeSpanAxis xAxis = new TimeSpanAxis() { AutoRange = AutoRange.Always };
        public ObservableCollection<ChannelDataVM> ChannelDataVMCollection { get; } = new ObservableCollection<ChannelDataVM>();

        public TimeSpanAxis XAxis
        {
            get { return xAxis; }
        }
        public AxisCollection YAxes
        {
            get { return yAxes; }
        }

        public Scope(ScopeProtocol protocol)
        {
            this.protocol = protocol;
            this.protocol.OnMessage += Protocol_OnMessage;
            this.protocol.OnError += Protocol_OnInfo;
            this.protocol.OnInfo += Protocol_OnInfo;
            CreateDataSetAndSeries();
        }

        private void Protocol_OnInfo(object sender, string msg)
        {
            OnInfo?.Invoke(this, msg);
        }
        private void Protocol_OnMessage(object sender, IMessage msg)
        {
            ScopeBufferMessage scopeBufferMsg = msg as ScopeBufferMessage;
            if (scopeBufferMsg != null)
            {
                int step = scopeBufferMsg.ChannelCount + 1;
                TimeSpan ts = TimeSpan.Zero;
                for (int i = 0; i < scopeBufferMsg.SampleCount; i = i + step)
                {
                    ts = TimeSpan.FromSeconds(scopeBufferMsg.Data[i]);
                    for (int j = 1; j <= scopeBufferMsg.ChannelCount; j++)
                    {
                        if (ts > lastMaxTime)
                        {
                            if (i == 0 && (ts.TotalMilliseconds - lastMaxTime.TotalMilliseconds) > msGapToleranceForAddingNull)
                            {
                                AddNullData(ChannelDataVMCollection[j - 1].DataSeries);
                            }
                            float val = scopeBufferMsg.Data[i + j];
                            ((XyDataSeries<TimeSpan, float>)ChannelDataVMCollection[j - 1].DataSeries).Append(ts, val);
                        }
                    }
                }
                lastMaxTime = ts;
                OnData?.Invoke(this, new EventArgs());
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
        private void CreateDataSetAndSeries()
        {
            series0 = new XyDataSeries<TimeSpan, float>() { AcceptsUnsortedData = true, FifoCapacity = FifoSize, SeriesName = "TestingSeries" };
            series1 = new XyDataSeries<TimeSpan, float>() { AcceptsUnsortedData = true, FifoCapacity = FifoSize, SeriesName = "TestingSeries2" };
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

        public void Start()
        {
            ClearDataSeries();
            lastMaxTime = TimeSpan.Zero;
            protocol.Connect();
        }
        public void Stop()
        {
            protocol.Close();
        }
        public void Pause()
        {
        }
    }
}
