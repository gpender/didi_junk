using SciChart.Core.Utility;
using System.Diagnostics;
using System.Windows.Controls;

namespace DidiWebSocketTest.Views
{
    class OutputWindowLogger : ISciChartLoggerFacade
    {
        public void Log(string formatString, params object[] args)
        {
            Debug.WriteLine(formatString, args);
        }

    }
    /// <summary>
    /// Interaction logic for ScopeUC.xaml
    /// </summary>
    public partial class ScopeUC : UserControl
    {
        public ScopeUC()
        {
            InitializeComponent();
            SciChartDebugLogger.Instance.SetLogger(new OutputWindowLogger());
        }
    }
}
