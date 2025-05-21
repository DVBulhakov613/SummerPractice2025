using Class_Lib;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OOP_CourseProject.Controls.LocationControl.PostalOfficeForms
{
    public class PostalOfficeViewModel : INotifyPropertyChanged
    {
        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _region;
        public string Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private uint _maxStorageCapacity;
        public uint MaxStorageCapacity
        {
            get => _maxStorageCapacity;
            set => SetProperty(ref _maxStorageCapacity, value);
        }

        private bool _isAutomated;
        public bool IsAutomated
        {
            get => _isAutomated;
            set => SetProperty(ref _isAutomated, value);
        }

        private bool _handlesPublicDropOffs;
        public bool HandlesPublicDropOffs
        {
            get => _handlesPublicDropOffs;
            set => SetProperty(ref _handlesPublicDropOffs, value);
        }

        private bool _isRegionalHQ;
        public bool IsRegionalHQ
        {
            get => _isRegionalHQ;
            set => SetProperty(ref _isRegionalHQ, value);
        }

        public void LoadFromModel(PostalOffice model)
        {
            var coordinates = model.GeoData;
            Longitude = coordinates.Longitude;
            Latitude = coordinates.Latitude;
            Address = coordinates.Address ?? string.Empty;
            Region = coordinates.Region ?? string.Empty;
            MaxStorageCapacity = model.MaxStorageCapacity;
            IsAutomated = model.IsAutomated;
            HandlesPublicDropOffs = model.HandlesPublicDropOffs;
            IsRegionalHQ = model.IsRegionalHQ;
        }

        public PostalOffice ToModel()
        {
            return new PostalOffice(
            new Coordinates(Longitude, Latitude, Address, Region),
                MaxStorageCapacity,
                IsAutomated,
                HandlesPublicDropOffs,
                IsRegionalHQ
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
}