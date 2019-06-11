using System;
using System.Collections.ObjectModel;
using DidiWebSocketTest.ViewModels;
using SciChart.Charting.Model;
using SciChart.Charting.Visuals.Axes;

namespace DidiWebSocketTest.Models
{
    public interface IScope
    {
        AxisCollection YAxes { get; }
        TimeSpanAxis XAxis { get; }
        ObservableCollection<ChannelDataVM> ChannelDataVMCollection { get; }

        event EventHandler<string> OnInfo;
        event EventHandler OnData;

        void Start();
        void Pause();
        void Stop();
    }
}