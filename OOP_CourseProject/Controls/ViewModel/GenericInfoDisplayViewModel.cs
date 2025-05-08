using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class GenericInfoDisplayViewModel : InfoDisplayViewModelBase
    {
        public ObservableCollection<object> Items { get; } = new();

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedInfoViewModel));
            }
        }

        private readonly Func<object, IInfoProviderViewModel> _viewModelFactory;

        public GenericInfoDisplayViewModel(IEnumerable<object> items, Func<object, IInfoProviderViewModel> viewModelFactory)
        {
            foreach (var item in items)
                Items.Add(item);

            _viewModelFactory = viewModelFactory;
        }

        public override IInfoProviderViewModel SelectedInfoViewModel =>
            SelectedItem != null ? _viewModelFactory(SelectedItem) : null;
    }


}
