using AppChat.Mum.Core;
using AppChat.Mum.Model;
using AppChat.Net;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AppChat.Mum.VeiwModel
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
            _server= new Server();
            _server.connectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendMessageCommand=new RelayCommand(o =>_server.ConnectToServer(Message),o => !string.IsNullOrEmpty(Message));
        }

        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user=Users.Where(x=> x.UID==uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke((() => Users.Remove(user)));
        }

        private void MessageReceived()
        {
            var msg=_server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                UserName = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),

            };
            if (!Users.Any(x=>x.UID==user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }





        //public ObservableCollection<MessageModel> Messages{ get;set; }

        //public ObservableCollection<ContactModel> Contacts { get; set; }

        ////instantiate tow collections 
        //public MainViewModel()
        //{
        //    Messages = new ObservableCollection<MessageModel>();
        //    Contacts = new ObservableCollection<ContactModel>();

        //    //showcase items
        //    Messages.Add(new MessageModel
        //    {
        //        UserNamre = "EHAB",
        //        UserColor = "#409aff",
        //        ImageSource = "",
        //        Message = "Test",
        //        Time = DateTime.Now,
        //        IsNativeOrigin = false,
        //        FirstMessage=true

        //    });
        //    for(int i = 0; i < 3; i++)
        //    {
        //        Messages.Add(new MessageModel
        //        {
        //            UserNamre = "EHAB",
        //            UserColor = "#409aff",
        //            ImageSource = "",
        //            Message = "Test",
        //            Time = DateTime.Now,
        //            IsNativeOrigin = false,
        //            FirstMessage = false

        //        });

        //    }
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Messages.Add(new MessageModel
        //        {
        //            UserNamre = "Bunny",
        //            UserColor = "#409aff",
        //            ImageSource = "https://www.onlinewebfonts.com/icon/206976",
        //            Message = "Test",
        //            Time = DateTime.Now,
        //            IsNativeOrigin =true



        //        });

        //    }
        //    Messages.Add(new MessageModel
        //    {
        //        UserNamre = "Bunny",
        //        UserColor = "#409aff",
        //        ImageSource = "",
        //        Message = "Last",
        //        Time = DateTime.Now,
        //        IsNativeOrigin = true


        //    });
        //    for(int i = 0; i > 4; i++)
        //    {
        //        Contacts.Add(new ContactModel
        //        {
        //            UserName = $"Ehab(i)",
        //            ImageSoures="",
        //            Message=Messages
        //        });
        //    }
        //}


    }
}
