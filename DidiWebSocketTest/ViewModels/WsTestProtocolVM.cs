using DidiWebSocketTest.Commands;
using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models;
using DidiWebSocketTest.Models.Messages;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DidiWebSocketTest.ViewModels
{
    public class WsTestProtocolVM : BaseVM
    {
        TestProtocol protocol;
        IUtilityServices utilityServices;
        string message;
        BitmapImage image;
        ICommand sendMessageCommand,closeCommand;
        public BitmapImage Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        public string Message
        {
            get { return message; }
            set
            {
                message += value + Environment.NewLine;
                OnPropertyChanged("Message");
            }
        }
        public WsTestProtocolVM(TestProtocol protocol, IUtilityServices utilityServices )
        {
            this.protocol = protocol;
            this.utilityServices = utilityServices;
            this.protocol.OnMessage += Protocol_OnMessage;
            this.protocol.OnError += Protocol_OnError;
            this.protocol.OnInfo += Protocol_OnError;
        }

        private void Protocol_OnError(object sender, string e)
        {
            Message = e;
        }

        private void Protocol_OnMessage(object sender, MessageBase msg)
        {
            if(msg is HelloMessage)
            {
                Message = msg.Message;
            }
            if(msg is ImageMessage)
            {
                Image = utilityServices.ByteArrayToImage(msg.MessageBytes);
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                if(sendMessageCommand == null)
                {
                    sendMessageCommand = new RelayCommand(SendMessage, CanSend);
                }
                return sendMessageCommand;
            }
        }
        void SendMessage()
        {
            protocol.SendMessage(new HelloMessage());
        }

        bool CanSend()
        {
            return true;
        }
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                {
                    closeCommand = new RelayCommand(Close, CanClose);
                }
                return closeCommand;
            }
        }
        void Close()
        {
            protocol.Close();
        }

        bool CanClose()
        {
            return true;
        }

        #region private methods

        #endregion
    }
}
