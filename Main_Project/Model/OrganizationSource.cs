using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Modul11_UI_HW.Model
{   
    interface IOrganizationSource<T>
    {
        T OpenFromJSONFile();
        string SerializeToJson(T organization);
        void SaveToJsonFile(object fileName, T organization);
    }

    class OrganizationSource : IOrganizationSource<ObservableCollection<Department>>
    {
        private static OrganizationSource instance;
        private OrganizationSource()
        {

        }
        public static OrganizationSource GetInstance()
        {
            if (instance == null)
            {
                instance = new OrganizationSource();
            }
            return instance;
        }

        /// <summary>
        /// Метод открытия файла с сохраненной структурой организации
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Department> OpenFromJSONFile()
        {
            var dlg = new OpenFileDialog
            {
                Title = "Открыть файл",
                Filter = "Файл json (*.json)|*.json",
                InitialDirectory = Environment.CurrentDirectory,
                RestoreDirectory = true
            };
            if (dlg.ShowDialog() == false) return null;

            var file = dlg.FileName;

            using StreamReader reader = File.OpenText(file);
            var fileText = reader.ReadToEnd();

            return JsonConvert.DeserializeObject<ObservableCollection<Department>>(fileText);
        }

        /// <summary>
        /// Сериализация структуры организации
        /// </summary>
        /// <param name="organization">организация</param>
        /// <returns></returns>
        public string SerializeToJson(ObservableCollection<Department> organization)
        {
            string text = JsonConvert.SerializeObject(organization, Formatting.Indented,
                              new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return text;
        }

        /// <summary>
        /// Метод сохранения структуры организации в Json файл 
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <param name="organization">организация</param>
        public async void SaveToJsonFile(object fileName, ObservableCollection<Department> organization)
        {
            string f_text = SerializeToJson(organization);

            var f_name = fileName as string;

            if (f_name == null)
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Сохранение файла",
                    Filter = "Файл json (*.json)|*.json",
                    InitialDirectory = Environment.CurrentDirectory,
                    RestoreDirectory = true
                };

                if (dialog.ShowDialog() != true) return;
                f_name = dialog.FileName;
            }

            using var writer = new StreamWriter(new FileStream(f_name, FileMode.Create, FileAccess.Write));
            await writer.WriteAsync(f_text).ConfigureAwait(false);
        }
    }
}
