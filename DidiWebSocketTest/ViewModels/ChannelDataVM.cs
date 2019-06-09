using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Visuals.Axes;
using System;
using System.Windows.Media;

namespace DidiWebSocketTest.ViewModels
{
    public class ChannelDataVM : LineRenderableSeriesViewModel, IDisposable
    {
        public NumericAxis YAxis { get; set; }

        public ChannelDataVM(int index)
        {
            YAxis = new NumericAxis();
            SetAxis(index);
        }


        private void SetAxis(int index)
        {
            ////PointMarker = new SciChart.Charting.Visuals.PointMarkers.CrossPointMarker() { Width = 1, Height = 1, Stroke = channel.Color, StrokeThickness = 1 };

            ////YAxis.VisibleRangeChanged -= YAxis_VisibleRangeChanged;
            //Stroke = Colors.Red;// channel.Color;
            //StrokeThickness = 1;// (int)channel.LineWidth;
            //YAxis.Id = $"2.{index.ToString()}";//ScopeId.ToString() + "." + channel.Index.ToString();
            //YAxisId = index.ToString();// YAxis.Id;
            ////YAxis.AxisTitle = driveName + " : " + channel.ParameterTag.ToString("0000") + " " + channel.Name;
            //YAxis.BorderBrush = new SolidColorBrush(Colors.Red); //ColorBrush;


            ////if (channel.AutoScale)
            //{
            //    YAxis.AutoRange = AutoRange.Always;
            //}
            ////else
            ////{
            ////    YAxis.AutoRange = AutoRange.Never;
            ////    if (YAxis.VisibleRange == null)
            ////    {
            ////        YAxis.VisibleRange = new DoubleRange();
            ////    }
            ////    YAxis.VisibleRange.SetMinMax(Min, Max);
            ////}
            ////if (!string.IsNullOrEmpty(channel.Units))
            ////{
            ////    YAxis.AxisTitle += " (" + channel.Units + ")";
            ////}
            //YAxis.StrokeThickness = 1;
            //YAxis.BorderBrush = new SolidColorBrush(Colors.Red);// channel.Color);
            //YAxis.BorderThickness = new System.Windows.Thickness(4, 3, 2, 3);
            //YAxis.AxisAlignment = AxisAlignment.Left;
            //YAxis.Margin = new System.Windows.Thickness(0, 0, 1, 0);
            ////YAxis.CursorTextFormatting = "0.####### '" + Units + "'";
            //YAxis.TextFormatting = "0.#######";

            //OnPropertyChanged("AutoZoom");
            //OnPropertyChanged("ChannelNo");
            //OnPropertyChanged("Max");
            //OnPropertyChanged("Min");
            //OnPropertyChanged("ParameterTag");
            //OnPropertyChanged("Name");
            //OnPropertyChanged("Units");
            //OnPropertyChanged("YAxis");
            //OnPropertyChanged("Color");
            //OnPropertyChanged("ColorBrush");
            //OnPropertyChanged("LineWidth");
            ////YAxis.VisibleRangeChanged += YAxis_VisibleRangeChanged;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
