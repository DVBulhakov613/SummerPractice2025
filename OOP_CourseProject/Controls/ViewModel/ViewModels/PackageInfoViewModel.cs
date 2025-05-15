using Class_Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class PackageInfoViewModel : INotifyPropertyChanged
    {
        private double _weight;
        public double Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(); }
        }

        private uint _length;
        public uint Length
        {
            get => _length;
            set { _length = value; OnPropertyChanged(); }
        }

        private uint _width;
        public uint Width
        {
            get => _width;
            set { _width = value; OnPropertyChanged(); }
        }

        private uint _height;
        public uint Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PackageType> PackageTypes { get; set; } = [];

        public void LoadEnums()
        {
            if (PackageTypes == null)
                PackageTypes = new ObservableCollection<PackageType>();
            else
                PackageTypes.Clear();

            foreach (PackageType value in Enum.GetValues(typeof(PackageType)))
                PackageTypes.Add(value);
        }

        private PackageType _selectedPackageType;
        public PackageType SelectedPackageType
        {
            get => _selectedPackageType;
            set { _selectedPackageType = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
