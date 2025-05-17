using Class_Lib;
using Class_Lib.Backend.Person_related;
using OOP_CourseProject.Controls.ViewModel.DisplayModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel
{
    public class ClientViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public ClientViewModel(Client client)
        {
            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Загальна інформація",
                InfoItems = new List<InfoItem>
                {
                    new() { Label = "Повне ім'я", Value = $"{client.FullName}" },
                    new() { Label = "Телефон", Value = client.PhoneNumber },
                    new() { Label = "Email", Value = client.Email }
                }
            });

            InfoSections.Add(new InfoSection
            {
                SectionTitle = "Пов'язані посилки",
                InfoItems = new List<InfoItem>
                {
                    new()
                    {
                        Label= "Відправлені",
                        Value = client.DeliveriesSent == null || client.DeliveriesSent.Count == 0 ? "Відсутні" : "Список...",
                        OnClick = client.DeliveriesSent == null || client.DeliveriesSent.Count == 0 ? null : () =>
                        {
                            var deliveryViewModel = new DeliveryListViewModel(client.DeliveriesSent); // must implement IInfoProviderViewModel
                            var window = new InfoPopupWindow(deliveryViewModel);
                            window.ShowDialog();
                        }
                    },
                    new()
                    {
                        Label= "Отримані",
                        Value = client.DeliveriesReceived == null || client.DeliveriesReceived.Count == 0 ? "Відсутні" : "Список...",
                        OnClick = client.DeliveriesReceived == null || client.DeliveriesReceived.Count == 0 ? null : () =>
                        {
                            var deliveryViewModel = new DeliveryListViewModel(client.DeliveriesReceived); // must implement IInfoProviderViewModel
                            var window = new InfoPopupWindow(deliveryViewModel);
                            window.ShowDialog();
                        }
                    },
                }

            });
                
        }
    }
}
