using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class LocationInfoViewModel : INotifyPropertyChanged
    {
        private uint _id;
        public uint ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        private uint _address;
        public uint Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
