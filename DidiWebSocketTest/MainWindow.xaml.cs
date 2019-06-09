using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebSocketSharp;

namespace DidiWebSocketTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebSocket ws = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void button_Click(object send, RoutedEventArgs e1)
        //{
        //    if (ws != null && ws.ReadyState == WebSocketState.Open) return;
        //    ws = new WebSocket("ws://172.18.176.58/chat","test");
        //    //ws.EmitOnPing = true;
        //    ws.OnOpen += Ws_OnOpen;
        //    ws.OnClose += Ws_OnClose;
        //    ws.OnMessage += Ws_OnMessage;
        //    ws.OnError += Ws_OnError;
        //    ws.Connect();
        //}

        //private void Ws_OnError(object sender, ErrorEventArgs e)
        //{
        //    textBlock.Text += $"{e.Message}{Environment.NewLine}";
        //}

        //private void Ws_OnMessage(object sender, MessageEventArgs e)
        //{
        //    textBlock.Text += $"{e.Data}{Environment.NewLine}";
        //}

        //private void Ws_OnClose(object sender, CloseEventArgs e)
        //{
        //    textBlock.Text += $"WebSocket closed{Environment.NewLine}";
        //    ws.OnOpen -= Ws_OnOpen;
        //    ws.OnClose -= Ws_OnClose;
        //    ws.OnMessage -= Ws_OnMessage;
        //    ws.OnError -= Ws_OnError;
        //}

        //private void Ws_OnOpen(object sender, EventArgs e)
        //{
        //    textBlock.Text += $"WebSocket opened{Environment.NewLine}";
        //}

        //private void button2_Click(object send, RoutedEventArgs e1)
        //{
        //    if (ws != null) ws.Close();
        //}
        //private void button3_Click(object send, RoutedEventArgs e1)
        //{
        //    if(ws!=null) ws.Send("hello");
        //}
    }
}
