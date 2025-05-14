using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OOP_CourseProject.Controls.ViewModel.ViewModels
{
    public class ContentInfoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ContentItem> Items { get; set; } = new();

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string _description;
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }

        private string _selectedContentType;
        public string SelectedContentType { get => _selectedContentType; set { _selectedContentType = value; OnPropertyChanged(); } }

        private int _amount;
        public int Amount { get => _amount; set { _amount = value; OnPropertyChanged(); } }

        public ObservableCollection<string> ContentTypes { get; } = new() { "Документи", "Електроніка", "Одяг" }; // example

        public ICommand AddItemCommand { get; }
        public ICommand ShowSummaryCommand { get; }

        public ContentInfoViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
            ShowSummaryCommand = new RelayCommand(ShowSummary);
        }

        private void AddItem()
        {
            Items.Add(new ContentItem
            {
                Name = this.Name,
                Description = this.Description,
                ContentType = this.SelectedContentType,
                Amount = this.Amount
            });

            // Optionally clear fields
            Name = string.Empty;
            Description = string.Empty;
            SelectedContentType = null;
            Amount = 0;
        }

        private void ShowSummary()
        {
            var viewModel = new InfoSectionList("Вміст посилки", Items.Select((item, i) => new InfoItem
            {
                Label = $"{i + 1}. {item.Name} ({item.Amount} шт)",
                Value = string.IsNullOrWhiteSpace(item.Description) ? "Без опису" : item.Description
            }));

            var window = new InfoPopupWindow(viewModel);
            window.ShowDialog();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class ContentItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public int Amount { get; set; }
    }

}
