using Class_Lib;
using Class_Lib.Backend.Database.Repositories;
using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Timers;
using Timer = System.Timers.Timer;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class LocationSearchViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BaseLocation> SearchResults { get; } = new();

        private readonly System.Timers.Timer _searchDelayTimer;
        private string _pendingQuery;

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    RestartSearchTimer(value);
                }
            }
        }

        private BaseLocation _selectedLocation;
        public BaseLocation SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand SelectCommand { get; }

        public event Action<BaseLocation> LocationSelected;

        public LocationSearchViewModel()
        {
            SelectCommand = new RelayCommand(Select, () => SelectedLocation != null);

            _searchDelayTimer = new Timer(300);
            _searchDelayTimer.AutoReset = false;
            _searchDelayTimer.Elapsed += async (_, __) => await SearchAsync();
        }

        private void RestartSearchTimer(string query)
        {
            _pendingQuery = query;
            _searchDelayTimer.Stop();
            _searchDelayTimer.Start();
        }

        private async Task SearchAsync()
        {
            var query = _pendingQuery;
            if (string.IsNullOrWhiteSpace(query)) return;

            var db = App.AppHost.Services.GetRequiredService<LocationRepository>();
            var results = await db.GetByCriteriaAsync(l =>
                (!string.IsNullOrEmpty(l.GeoData.Address) && l.GeoData.Address.Contains(query)) ||
                l.ID.ToString().Contains(query) ||
                l.GeoData.Region.Contains(query));

            App.Current.Dispatcher.Invoke(() =>
            {
                SearchResults.Clear();
                foreach (var item in results)
                    SearchResults.Add(item);
            });
        }

        private void Select()
        {
            if (SelectedLocation != null)
                LocationSelected?.Invoke(SelectedLocation);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
