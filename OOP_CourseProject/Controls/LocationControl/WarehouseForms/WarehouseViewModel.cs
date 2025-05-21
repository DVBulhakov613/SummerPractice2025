using Class_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.LocationControl.WarehouseForms
{
    public class WarehouseViewModel : INotifyPropertyChanged
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

        public void LoadFromModel(Warehouse model)
        {
            var coordinates = model.GeoData;
            Longitude = coordinates.Longitude;
            Latitude = coordinates.Latitude;
            Address = coordinates.Address ?? string.Empty;
            Region = coordinates.Region ?? string.Empty;
            MaxStorageCapacity = model.MaxStorageCapacity;
            IsAutomated = model.IsAutomated;
        }

        public Warehouse ToModel()
        {
            return new Warehouse(
            new Coordinates(Longitude, Latitude, Address, Region),
                MaxStorageCapacity,
                IsAutomated
            );
        }

        #region INotifyPropertyChanged
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
        #endregion
    }
}
