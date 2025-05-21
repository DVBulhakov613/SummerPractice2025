using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls.Helpers
{
    public static class SerializationHelper
    {
        public static string? PickFolder()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Оберіть папку для збереження"
            };

            return dialog.ShowDialog() == CommonFileDialogResult.Ok
                ? dialog.FileName
                : null;
        }

        public static bool SerializeSelectedItemsToFolder<T>(DataGrid dataGrid, Func<T, object> toDtoFunc, string filePrefix = null) where T : class
        {
            if (dataGrid.SelectedItems == null || dataGrid.SelectedItems.Count == 0)
            {
                throw new ArgumentException("Будь ласка, виберіть елементи для експорту.");
            }

            string folderPath = PickFolder();
            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentException("Необхідно вибрати папку для збереження.");

            foreach (var obj in dataGrid.SelectedItems)
            {
                if (obj is T item)
                {
                    var dto = toDtoFunc(item);
                    var idProp = dto.GetType().GetProperty("ID");
                    var id = idProp?.GetValue(dto)?.ToString() ?? Guid.NewGuid().ToString();
                    var typeName = filePrefix ?? typeof(T).Name;
                    var filePath = Path.Combine(folderPath, $"{typeName}_{id}.json");

                    var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(filePath, json);
                }
            }

            return true;
        }

        public static void SaveToJSON()
        {
            throw new NotImplementedException();
        }

        public static void LoadFromJSON()
        {
            throw new NotImplementedException();
        }
    }
}
