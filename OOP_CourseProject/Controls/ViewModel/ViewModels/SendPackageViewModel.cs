using Class_Lib.Backend.Package_related;
using GalaSoft.MvvmLight.Command;
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
    public class SendPackageViewModel/* : INotifyPropertyChanged*/
    {
        public ClientInfoViewModel ClientInfo { get; } = new();
        public LocationInfoViewModel LocationsInfo { get; } = new();
        public PackageInfoViewModel PackageConfigurations { get; } = new();
        public ContentInfoViewModel ContentInfo { get; } = new();

        //public ICommand ConfirmCommand { get; }

        public SendPackageViewModel()
        {
            //ConfirmCommand = new RelayCommand(SendDelivery);
        }

        //private void SendDelivery()
        //{
        //    Delivery delivery = new(
        //        new Package(
        //            PackageConfigurations.Length,
        //            PackageConfigurations.Width,
        //            PackageConfigurations.Height,
        //            PackageConfigurations.Weight,
        //            ClientInfo.Sender,
        //            ClientInfo.Receiver,
        //            (Class_Lib.Warehouse)LocationsInfo.FromLocation,
        //            (Class_Lib.Warehouse)LocationsInfo.ToLocation,
        //            PackageConfigurations.SelectedPackageType
        //            ),
        //        ClientInfo.Sender,
        //        ClientInfo.Receiver,
        //        (Class_Lib.Warehouse)LocationsInfo.FromLocation,
        //        (Class_Lib.Warehouse)LocationsInfo.ToLocation,
        //        CostEstimate(),
        //        false);
        //}

        public double CostEstimate()
        {
            throw new NotImplementedException();
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
