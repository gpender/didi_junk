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
        RelayCommand<ScopeCommandName> scopeCommand;
        IScope scope;

        string message ="";
        //public int ChannelCount
        //{
        //    get { return scope.ChannelCount; }
        //}
        public ObservableCollection<ChannelDataVM> ChannelDataVMCollection
        {
            get { return scope.ChannelDataVMCollection; }
        }
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
        public TimeSpanAxis XAxis
        {
            get { return scope.XAxis; }
        }
        public AxisCollection YAxes
        {
            get { return scope.YAxes; }
        }
        DriveHttpProtocol parameterProtocol;
        public ScopeVM(IScope scope, DriveHttpProtocol parameterProtocol)
        {
            this.parameterProtocol = parameterProtocol;

            this.scope = scope;
            this.scope.OnInfo += Scope_OnMessage;
            this.scope.OnData += Scope_OnData;
            ViewportManager = new DefaultViewportManager();
        }

        private void Scope_OnData(object sender, EventArgs e)
        {
            Message = $"{DateTime.Now.ToLongTimeString()} Data received";
        }

        private void Scope_OnMessage(object sender, string message)
        {
            Message = message;
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
                    scope.Start();
                    break;
                case ScopeCommandName.STOP:
                    scope.Stop();
                    break;
                case ScopeCommandName.PAUSE:
                    scope.Pause();
                    break;
                case ScopeCommandName.GETMENUS:
                    parameterProtocol.GetMenus();
                    break;
            }
        }
        #endregion
    }
}
