using Class_Lib;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.ViewModel.DisplayModels
{
    public class DeliveryListViewModel : IInfoProviderViewModel
    {
        public ObservableCollection<InfoSection> InfoSections { get; } = [];

        public DeliveryListViewModel(IEnumerable<Delivery> deliveries)
        {
            foreach (Delivery delivery in deliveries)
            {
                InfoSections.Add(new InfoSection
                {
                    SectionTitle = $"Інформація посилки {delivery.PackageID}",
                    InfoItems = new List<InfoItem>
                {
                    new() { Label = "Дата оформлення", Value = delivery.Package.CreatedAt.ToString("HH:mm, dd-MM-yyyy") },
                    new() { Label = "Розмір", Value = $"{delivery.Package.Length} x {delivery.Package.Width} x {delivery.Package.Height} см" },
                    new() { Label = "Вага", Value = $"{delivery.Package.Weight:F2} кг" },
                    new() { Label = "Тип", Value = $"{delivery.Package.Type.GetDescription()}" }
                }
                });

                InfoSections.Add(new InfoSection
                {
                    SectionTitle = "Відправник",
                    InfoItems = new List<InfoItem>
                {
                    new() { Label = "Повне ім'я", Value = $"{delivery.Sender.FullName}" },
                    new() { Label = "Телефон", Value = delivery.Sender.PhoneNumber },
                    new() { Label = "Email", Value = delivery.Sender.Email },
                }
                });

                InfoSections.Add(new InfoSection
                {
                    SectionTitle = "Отримувач",
                    InfoItems = new List<InfoItem>
                {
                    new() { Label = "Повне ім'я", Value = $"{delivery.Receiver.FullName}" },
                    new() { Label = "Телефон", Value = delivery.Receiver.PhoneNumber },
                    new() { Label = "Email", Value = delivery.Receiver.Email },
                }
                });

                InfoSections.Add(new InfoSection
                {
                    SectionTitle = "Зміст",
                    InfoItems = new List<InfoItem>
                {
                    new()
                    {
                        Label = $"Зміст посилки {delivery.SentToID}",
                        Value = delivery.Package.DeclaredContent == null || delivery.Package.DeclaredContent.Count == 0 ? "Невідомо / Відсутнє" : "Деталі...",
                        OnClick = delivery.Package.DeclaredContent == null || delivery.Package.DeclaredContent.Count == 0 ? null : () =>
                        {
                            var contentListViewModel = new ContentListViewModel(delivery.Package.DeclaredContent); // must implement IInfoProviderViewModel
                            var window = new InfoPopupWindow(contentListViewModel);
                            window.ShowDialog();
                        }
                    }
                }
                });

                InfoSections.Add(new InfoSection
                {
                    SectionTitle = "Локації",
                    InfoItems = new List<InfoItem>
                {
                    new()
                    {
                        Label = "Надіслано з:", Value = $"{delivery.SentFromID} | {delivery.SentFrom.GeoData.Region}{delivery.SentFrom.GeoData.Address}"
                    },

                    new()
                    {
                        Label = "Отримано в:", Value = $"{delivery.SentToID} | {delivery.SentTo.GeoData.Region}{delivery.SentTo.GeoData.Address}"
                    }

                }
                });
            }
        }
    }
}
