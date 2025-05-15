using Class_Lib;
using Class_Lib.Backend.Person_related;
using GalaSoft.MvvmLight.Command;
using OOP_CourseProject.Controls.SendPackageControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class ClientInfoViewModel : INotifyPropertyChanged
    {
        public string SenderName => Sender?.FirstName ?? "";
        public string SenderSurname => Sender?.Surname ?? "";
        public string SenderPhoneNumber => Sender?.PhoneNumber ?? "";
        public string SenderEmail => Sender?.Email ?? "";

        public string ReceiverName => Receiver?.FirstName ?? "";
        public string ReceiverSurname => Receiver?.Surname ?? "";
        public string ReceiverPhoneNumber => Receiver?.PhoneNumber ?? "";
        public string ReceiverEmail => Receiver?.Email ?? "";

        public ICommand SearchSenderCommand { get; }
        public ICommand SearchReceiverCommand { get; }

        private Client _sender;
        public Client Sender
        {
            get => _sender;
            set
            {
                _sender = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SenderName));
                OnPropertyChanged(nameof(SenderSurname));
                OnPropertyChanged(nameof(SenderPhoneNumber));
                OnPropertyChanged(nameof(SenderEmail));
            }
        }

        private Client _receiver;
        public Client Receiver
        {
            get => _receiver;
            set
            {
                _receiver = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReceiverName));
                OnPropertyChanged(nameof(ReceiverSurname));
                OnPropertyChanged(nameof(ReceiverPhoneNumber));
                OnPropertyChanged(nameof(ReceiverEmail));
            }
        }

        public ClientInfoViewModel()
        {
            SearchSenderCommand = new RelayCommand(SearchSender);
            SearchReceiverCommand = new RelayCommand(SearchReceiver);
        }

        private void SearchSender()
        {
            var window = new ClientSearchWindow(location =>
            {
                Sender = location;
            });
            window.ShowDialog();
        }

        private void SearchReceiver()
        {
            var window = new ClientSearchWindow(location =>
            {
                Receiver = location;
            });
            window.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}

/*
public class LocationInfoViewModel : INotifyPropertyChanged
    {
        private string _sentFrom;
        private string _sentTo;
        private BaseLocation _fromLocation;
        private BaseLocation _toLocation;

        public string SentFrom => FromLocation != null ? $"{FromLocation.GeoData.Region} {FromLocation.GeoData.Address} (відд. {FromLocation.ID})" : string.Empty;
        public string SentTo => ToLocation != null ? $"{ToLocation.GeoData.Region} {ToLocation.GeoData.Address} (відд. {ToLocation.ID})" : string.Empty;


        public ICommand SearchFromLocationCommand { get; }
        public ICommand SearchToLocationCommand { get; }

        public LocationInfoViewModel()
        {
            SearchFromLocationCommand = new RelayCommand(OpenSearchFrom);
            SearchToLocationCommand = new RelayCommand(OpenSearchTo);
        }

        public BaseLocation FromLocation
        {
            get => _fromLocation;
            set
            {
                _fromLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SentFrom));
            }
        }

        public BaseLocation ToLocation
        {
            get => _toLocation;
            set
            {
                _toLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SentTo));
            }
        }

        private void OpenSearchFrom()
        {
            var window = new LocationSearchWindow(location =>
            {
                FromLocation = location;
            });
            window.ShowDialog();
        }

        private void OpenSearchTo()
        {
            var window = new LocationSearchWindow(location =>
            {
                ToLocation = location;
            });
            window.ShowDialog();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
*/