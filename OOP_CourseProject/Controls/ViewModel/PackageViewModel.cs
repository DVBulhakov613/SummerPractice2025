using Class_Lib;
using Class_Lib.Backend.Database;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.ViewModels;
using Class_Lib.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class PackageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Package> Packages { get; } = new();

        private Package _selectedPackage;
        public Package SelectedPackage
        {
            get => _selectedPackage;
            set
            {
                _selectedPackage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedPackageInfoViewModel));
            }
        }

        public PackageViewModel()
        {
            LoadPackages();
        }

        private async void LoadPackages()
        {
            var packageRepo = App.AppHost.Services.GetRequiredService<PackageRepository>();
            //var packages = await packageRepo.GetByCriteriaAsync(p => p.SentFromID == App.CurrentEmployee.WorkplaceID || p.SentToID == App.CurrentEmployee.WorkplaceID);
            var packages = await packageRepo.GetByCriteriaAsync(p => p.Height > 0);
            Packages.Clear();
            foreach (var package in packages)
            {
                Packages.Add(package);
            }
        }

        public PackageInfoViewModel SelectedPackageInfoViewModel =>
            SelectedPackage != null ? new PackageInfoViewModel(SelectedPackage) : null;

        // Implement INotifyPropertyChanged (can use CommunityToolkit if preferred)
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
