using ChatApp.MVVM.Core;
using ChatApp.MVVM.Model;
using ChatApp.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        private Server _server;
        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.msgReceiveEvent += msgReceive;
            _server.disconnectEvent += disconnect;
            ConnectToServerCommand = new RelayCommand(p => _server.ConnectToServer(Username));
            SendMessageCommand = new RelayCommand(p => _server.SendMessageToServer(Message), p=> !string.IsNullOrEmpty(Message));
        }

        private void disconnect()
        {
            var uid = _server.packetReader.ReadMessage();
            var username = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(()=> Users.Remove(username));
        }

        private void msgReceive()
        {
            var msg = _server.packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void UserConnected()
        {
            var user = new UserModel { 
                Username = _server.packetReader.ReadMessage(),
                UID = _server.packetReader.ReadMessage(),                
            };
            if (!Users.Any(x=> x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
