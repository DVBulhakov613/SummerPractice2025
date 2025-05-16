using Class_Lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class PackageInfoViewModel : INotifyPropertyChanged
    {
        private string _weight;
        public string Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(); }
        }

        private string _length;
        public string Length
        {
            get => _length;
            set { _length = value; OnPropertyChanged(); }
        }

        private string _width;
        public string Width
        {
            get => _width;
            set { _width = value; OnPropertyChanged(); }
        }

        private string _height;
        public string Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PackageType> PackageTypes { get; set; } = new((PackageType[])Enum.GetValues(typeof(PackageType)));

        private PackageType _selectedPackageType;
        public PackageType SelectedPackageType
        {
            get => _selectedPackageType;
            set { _selectedPackageType = value; OnPropertyChanged(); }
        }

        public PackageInfoViewModel()
        {
            // Debug.WriteLine moved to constructor to avoid invalid placement in class body
            Debug.WriteLine("PackageTypes: " + PackageTypes.Count);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
