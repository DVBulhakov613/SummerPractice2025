using Class_Lib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OOP_CourseProject.Controls.LocationControl.PostalOfficeForms
{
    public class PostalOfficeViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Employee> AssignedEmployees { get; } = [];

        private double? _longitude;
        public double? Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private double? _latitude;
        public double? Latitude
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

        private uint? _maxStorageCapacity;
        public uint? MaxStorageCapacity
        {
            get => _maxStorageCapacity;
            set => SetProperty(ref _maxStorageCapacity, value);
        }

        private bool? _isAutomated = false;
        public bool? IsAutomated
        {
            get => _isAutomated;
            set => SetProperty(ref _isAutomated, value);
        }

        private bool? _handlesPublicDropOffs = false;
        public bool? HandlesPublicDropOffs
        {
            get => _handlesPublicDropOffs;
            set => SetProperty(ref _handlesPublicDropOffs, value);
        }

        private bool? _isRegionalHQ = false;
        public bool? IsRegionalHQ
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

            if (model.Staff != null)
                foreach (var employee in model.Staff)
                    AssignedEmployees.Add(employee);
        }

        public PostalOffice ToModel()
        {
            if (Longitude == null || Latitude == null || MaxStorageCapacity == null ||
                IsAutomated == null || HandlesPublicDropOffs == null || IsRegionalHQ == null)
            {
                throw new InvalidOperationException("Усі ці поля обов'язкові та повинні бути заповнені.");
            }

            PostalOffice po = new PostalOffice(
                new Coordinates(Longitude.Value, Latitude.Value, Address, Region),
                MaxStorageCapacity.Value,
                IsAutomated.Value,
                HandlesPublicDropOffs.Value,
                IsRegionalHQ.Value
            );
            if (po.Staff != null) po.Staff = AssignedEmployees.ToList();
            return po;
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