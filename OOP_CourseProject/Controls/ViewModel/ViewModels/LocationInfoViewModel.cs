using Class_Lib;
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
}
