using Class_Lib.Backend.Database.Repositories;
using Class_Lib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Class_Lib.Backend.Person_related;
using Class_Lib.Database.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    // yes this is a copy paste of the LocationSearchViewModel i don't care enough to make it generic right now
    public class ClientSearchViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> SearchResults { get; } = new();

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
                _ = SearchAsync(value);
            }
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand SelectCommand { get; }

        public event Action<Client> ClientSelected;

        public ClientSearchViewModel()
        {
            SelectCommand = new RelayCommand(Select, () => SelectedClient != null);
        }

        private async Task SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            var db = App.AppHost.Services.GetRequiredService<ClientRepository>();
            var results = await db.GetByCriteriaAsync(client =>
                client.ID.ToString().Contains(query) ||
                client.FirstName.Contains(query) ||
                client.Surname.Contains(query) ||
                client.Email.Contains(query) ||
                client.PhoneNumber.Contains(query)
        );


            SearchResults.Clear();
            foreach (var item in results)
                SearchResults.Add(item);
        }

        private void Select()
        {
            if (SelectedClient != null)
                ClientSelected?.Invoke(SelectedClient);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
