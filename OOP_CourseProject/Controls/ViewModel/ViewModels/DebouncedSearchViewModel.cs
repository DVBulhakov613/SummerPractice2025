using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class DebouncedSearchViewModel<T> : INotifyPropertyChanged
    {
        private readonly Timer _searchDelayTimer;
        private readonly Func<string, Task<IEnumerable<T>>> _searchFunction;
        private string _pendingQuery;

        public ObservableCollection<T> SearchResults { get; } = new();

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

        private T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                ItemSelected?.Invoke(value);
            }
        }

        public event Action<T> ItemSelected;

        public DebouncedSearchViewModel(Func<string, Task<IEnumerable<T>>> searchFunction, double delayMilliseconds = 500)
        {
            _searchFunction = searchFunction;

            _searchDelayTimer = new Timer(delayMilliseconds);
            _searchDelayTimer.AutoReset = false;
            _searchDelayTimer.Elapsed += async (_, __) => await PerformSearchAsync();
        }

        private void RestartSearchTimer(string query)
        {
            _pendingQuery = query;
            _searchDelayTimer.Stop();
            _searchDelayTimer.Start();
        }

        private async Task PerformSearchAsync()
        {
            var query = _pendingQuery;
            if (string.IsNullOrWhiteSpace(query)) return;

            var results = await _searchFunction(query);

            Application.Current.Dispatcher.Invoke(() =>
            {
                SearchResults.Clear();
                foreach (var item in results)
                    SearchResults.Add(item);
            });
        }

        public void Select()
        {
            if (SelectedItem != null)
                ItemSelected?.Invoke(SelectedItem);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
