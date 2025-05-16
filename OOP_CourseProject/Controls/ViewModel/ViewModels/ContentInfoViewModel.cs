using Class_Lib;
using GalaSoft.MvvmLight.Command;
using OOP_CourseProject.Controls.PackageInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private ContentType? _selectedContentType;
        public ContentType? SelectedContentType { get => _selectedContentType; set { _selectedContentType = value; OnPropertyChanged(); } }

        private string _amount;
        public string Amount { get => _amount; set { _amount = value; OnPropertyChanged(); } }

        public ObservableCollection<ContentType> ContentTypes { get; } = new ((ContentType[])Enum.GetValues(typeof(ContentType)));

        public ICommand AddItemCommand { get; }
        public ICommand ShowSummaryCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand EditItemCommand { get; }

        public ContentInfoViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem);
            ShowSummaryCommand = new RelayCommand(ShowSummary);
            RemoveItemCommand = new RelayCommand<ContentItem>(RemoveItem);
            EditItemCommand = new RelayCommand<ContentItem>(EditItem);
        }

        private void AddItem()
        {
            if (string.IsNullOrWhiteSpace(this.Name) || SelectedContentType == null || string.IsNullOrWhiteSpace(this.Amount))
            {
                MessageBox.Show("Не всі обов'язкові поля заповнені.", "Увага!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                Items.Add(new ContentItem
                    {
                        Name = this.Name,
                        Description = this.Description,
                        ContentType = SelectedContentType.Value,
                        Amount = uint.TryParse(this.Amount, out _) 
                            ? uint.Parse(this.Amount) > 0 
                                ? uint.Parse(this.Amount) 
                                : throw new ArgumentException("Не можна назначити від'ємне число.")
                            : throw new ArgumentException("Не можна назначити числу текст.")
                    });
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message, "Увага!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            Name = string.Empty;
            Description = string.Empty;
            SelectedContentType = null;
            Amount = string.Empty;
        }

        private void ShowSummary()
        {
            var window = new ContentItemListWindow(this);
            window.ShowDialog();
        }

        private void RemoveItem(ContentItem item)
        {
            if (item != null)
                Items.Remove(item);
        }

        private void EditItem(ContentItem item)
        {
            if (item == null) return;

            Name = item.Name;
            Description = item.Description;
            SelectedContentType = item.ContentType;
            Amount = item.Amount.ToString();

            Items.Remove(item);

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ContentItemListWindow)
                {
                    window.Close();
                    break;
                }
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class ContentItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ContentType ContentType { get; set; }
        public uint Amount { get; set; }
    }

}
