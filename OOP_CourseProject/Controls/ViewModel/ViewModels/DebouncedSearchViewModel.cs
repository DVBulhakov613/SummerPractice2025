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
    /// <summary>
    /// A view model for a debounced search functionality. Generic to allow for different types of search results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DebouncedSearchViewModel<T> : INotifyPropertyChanged
    {
        private readonly Timer _searchDelayTimer;
        private readonly Func<string, Task<IEnumerable<T>>> _searchFunction;
        private string _pendingQuery;

        /// <summary>
        /// Collection of search results.
        /// </summary>
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

        /// <summary>
        /// Event that is triggered when an item is selected from the search results.
        /// </summary>
        public event Action<T> ItemSelected;

        /// <summary>
        /// Creates a new instance of the DebouncedSearchViewModel.
        /// </summary>
        /// <param name="searchFunction"></param>
        /// <param name="delayMilliseconds"></param>
        public DebouncedSearchViewModel(Func<string, Task<IEnumerable<T>>> searchFunction, double delayMilliseconds = 500)
        {
            _searchFunction = searchFunction;

            _searchDelayTimer = new Timer(delayMilliseconds);
            _searchDelayTimer.AutoReset = false;
            _searchDelayTimer.Elapsed += async (_, __) => await PerformSearchAsync();
        }

        /// <summary>
        /// Restarts the search timer with the new query.
        /// </summary>
        /// <param name="query"></param>
        private void RestartSearchTimer(string query)
        {
            _pendingQuery = query;
            _searchDelayTimer.Stop();
            _searchDelayTimer.Start();
        }

        /// <summary>
        /// Performs the search operation asynchronously.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Selects the currently selected item and triggers the ItemSelected event.
        /// </summary>
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
