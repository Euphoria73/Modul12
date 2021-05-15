using System;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;


namespace Modul11_UI_HW.Model
{
    class Organization
    {
        private static Organization instance;

        private Organization()
        { }
        /// <summary>
        /// Синглтон структуры организации
        /// </summary>
        /// <returns></returns>
        public static Organization GetInstance()
        {
            if (instance == null)
            {
                instance = new Organization();
            }
            return instance;
        }
        /// <summary>
        /// Заполнение новой организации псевдослучайными данными
        /// </summary>
        /// <param name="deps"></param>
        public void Populate(ObservableCollection<Department> deps) 
        {
            if (deps.Count == 0)
            {
                deps.Add(new Department
                (
                    "Default organization", 0
                ));
            }
            PopulateOrganization(deps[0].Departments, "Department ", 0);
           
            foreach (var item in deps)
            {                
                deps[0].ManagerDepartment.Salary += item.ManagerDepartment.ManagerGetSalary(item);
            }
        }
                
        /// <summary>
        /// Заполнение структуры данными
        /// </summary>
        /// <param name="deps"></param>
        /// <param name="nameDepartment"></param>
        /// <param name="countDivisions"></param>
        private void PopulateOrganization(ObservableCollection<Department> deps, string nameDepartment, int countDivisions)
        {            
            if (countDivisions == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < countDivisions; i++)
                {
                    deps.Add(new Department(nameDepartment + $"{i + 1}", 2));
                   
                    foreach (var item in deps)
                    {
                        item.ManagerDepartment.Salary += item.ManagerDepartment.ManagerGetSalary(item);
                    }

                    PopulateOrganization(deps[i].Departments, deps[i].NameDepartment + ".", countDivisions - 1);
                }
            }
        }

        public void RefreshSalary(ObservableCollection<Department> organization)
        {
            foreach (var item in organization) //пересчитываем зарплаты руководства
            {
                item.ManagerDepartment.ManagerGetSalary(item);
            }
        }

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

        public string SerializeToJSON(ObservableCollection<Department> organization)
        {
           string text = JsonConvert.SerializeObject(organization, Formatting.Indented,
                             new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            return text;
        }

        public async void SaveToJSONFile(object path, ObservableCollection<Department> organization)
        {
            string f_text = SerializeToJSON(organization);

            var fileName = path as string;

            if (fileName == null)
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Сохранение файла",
                    Filter = "Файл json (*.json)|*.json",
                    InitialDirectory = Environment.CurrentDirectory,
                    RestoreDirectory = true
                };

                if (dialog.ShowDialog() != true) return;
                fileName = dialog.FileName;
            }

            using var writer = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));
            await writer.WriteAsync(f_text).ConfigureAwait(false);
        }
    }
}
