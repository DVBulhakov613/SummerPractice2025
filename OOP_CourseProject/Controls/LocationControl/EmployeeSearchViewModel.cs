using Class_Lib;
using GalaSoft.MvvmLight.Command;
using OOP_CourseProject.Controls.ViewModel.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OOP_CourseProject.Controls.LocationControl
{
    public class EmployeeSearchViewModel : DebouncedSearchViewModel<Employee>
    {
        private readonly ObservableCollection<Employee> _assignedEmployees = [];
        public ObservableCollection<Employee> AssignedEmployees => _assignedEmployees;

        public ICommand RemoveEmployeeCommand { get; }
        public ICommand ConfirmCommand { get; }

        public event Action<List<Employee>> Confirmed;

        public EmployeeSearchViewModel(Func<string, Task<IEnumerable<Employee>>> searchFunc, List<Employee>? initiallyAssigned = null) : base(searchFunc)
        {
            RemoveEmployeeCommand = new RelayCommand<Employee>(RemoveEmployee);
            ConfirmCommand = new RelayCommand(ConfirmSelection);

            if (initiallyAssigned != null)
            {
                foreach (Employee emp in initiallyAssigned)
                {
                    AddEmployee(emp);
                }
            }
        }


        public void AddEmployee(Employee employee)
        {
            if (!_assignedEmployees.Any(e => e.ID == employee.ID))
                _assignedEmployees.Add(employee);
        }

        private void RemoveEmployee(Employee employee)
        {
            _assignedEmployees.Remove(employee);
        }

        private void ConfirmSelection()
        {
            Confirmed?.Invoke(_assignedEmployees.ToList());
        }
    }

}
