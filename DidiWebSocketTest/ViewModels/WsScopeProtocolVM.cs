using DidiWebSocketTest.Commands;
using DidiWebSocketTest.Interfaces;
using DidiWebSocketTest.Models;
using DidiWebSocketTest.Models.Messages;
using System;
using System.Windows.Input;

namespace DidiWebSocketTest.ViewModels
{
    public class WsScopeProtocolVM : BaseVM
    {
        ScopeProtocol protocol;
        string message;
        RelayCommand<MessageType> sendMessageCommand;
        ICommand closeCommand;
        public string Message
        {
            get { return message; }
            set
            {
                message += value + Environment.NewLine;
                OnPropertyChanged("Message");
            }
        }
        public WsScopeProtocolVM(ScopeProtocol protocol)
        {
            this.protocol = protocol;
            this.protocol.OnMessage += Protocol_OnMessage;
            this.protocol.OnError += Protocol_OnInfo;
            this.protocol.OnInfo += Protocol_OnInfo;
        }

        private void Protocol_OnInfo(object sender, string e)
        {
            Message = e;
        }

        private void Protocol_OnMessage(object sender, IMessage msg)
        {
        }

        public ICommand SendMessageCommand
        {
            get
            {
                if(sendMessageCommand == null)
                {
                    sendMessageCommand = new RelayCommand<MessageType>(SendMessage);
                }
                return sendMessageCommand;
            }
        }
        void SendMessage(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.GET_SCOPE_PARAMS:
                    protocol.GetScopeParameters();// SendMessage();
                    break;
                case MessageType.CONFIG_PARAMS:
                    protocol.GetScopeConfigParameters();// SendMessage();
                    break;
                case MessageType.SEND_BUFFER:
                    protocol.GetScopeBuffer();// SendMessage();
                    break;
            }
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
            protocol.Connect();
        }

        bool CanClose()
        {
            return true;
        }

        #region private methods


        #endregion
    }
}
