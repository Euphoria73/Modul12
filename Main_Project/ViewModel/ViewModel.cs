using System;
using System.Collections.ObjectModel;
using Modul11_UI_HW.Model;
using System.Windows.Input;
using Modul11_UI_HW.Commands;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Modul11_UI_HW.ViewModel
{
    class ViewModel : ViewModelBase
    {
        private string _title = "";

        /// <summary>
        /// Заголовок главного окна
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Department selectedItem;

        /// <summary>
        /// Выбранный департамент в treeview
        /// </summary>
        public Department SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }
        private static ObservableCollection<Department> _myOrganization = new ObservableCollection<Department>();

        private readonly Organization structure = Organization.GetInstance(); //использую синглтон структуры, т.к. в один момент времени мы можем работать только с одной организацией
        
        private readonly OrganizationSource organizationSource = OrganizationSource.GetInstance();
        /// <summary>
        /// Коллекция организации
        /// </summary>
        public ObservableCollection<Department> GetOrganization
        {
            get
            {
                return _myOrganization;
            }

            set => SetProperty(ref _myOrganization, value);
        }

        #region Команды управления программой


        #region Команда для создания новой организации
        public ICommand CreateCommand { get; }

        private bool CanCreateCommandExecute(object file) => true;

        //Создание структуры компании 
        public void OnCreateCommandExecuted(object file)
        {
            structure.Populate(_myOrganization);
        }
        #endregion

        #region Команда открытия базы данных организации в формате JSON
        public ICommand OpenCommand { get; } //Команда для открытия организации из файла

        private bool CanOpenCommandExecute(object path) => true;

        //для открытия записи организации в формате JSON
        public void OnOpenCommandExecuted(object path)
        {
            try
            {
                GetOrganization = organizationSource.OpenFromJSONFile();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Обратитесь в службу поддержки");
            }
        }

        #endregion

        #region Команда сохранения организации в файл
        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object path)
        {
            string f_text = organizationSource.SerializeToJson(GetOrganization);
            if (f_text == null || f_text == "[]") return false; //проверяем, что структура не пустая
            return true;
        }

        //Сохранение организации в формате JSON
        public void OnSaveCommandExecutedAsync(object path)
        {
            organizationSource.SaveToJsonFile(path, GetOrganization);
        }

        #endregion

        #region Команда добавления сотрудника в выбранный департамент
        public ICommand AddEmployeeCommand { get; }

        private bool CanAddEmployeeCommandExecute(object path) => path is Department;

        public void OnAddEmployeeCommandExecuted(object path)
        {
            try
            {
                int numAddingEmployees = 1; //TODO: можно добавить вывод текстбокса для ввода требуемого пользователем числа
                SelectedItem.AddNewEmployee(numAddingEmployees);
                structure.RefreshSalary(_myOrganization);
                MessageBox.Show("Работник успешно добавлен!");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Выберите департамент, в который хотите добавить работника");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message} => {e.GetType()}, передайте информацию в отдел технической поддержки для исправления");
            }
        }
        #endregion     

        #region Команда добавления нового департамента в выбранный департамент
        public ICommand AddDepartmentCommand { get; }

        private bool CanAddDepartmentCommandExecute(object path) => path is Department;

        //для открытия записи организации в формате JSON
        public void OnAddDepartmentCommandExecuted(object path)
        {
            try
            {
                if (selectedItem == _myOrganization[0])
                {
                    selectedItem.AddDepartment(new Department($"Департамент {selectedItem.Departments.Count + 1}", 2));
                }
                else
                {
                    int lastNumberDepartment = selectedItem.Departments.Count + 1;
                    selectedItem.AddDepartment(new Department($"{selectedItem.NameDepartment + "." + lastNumberDepartment}", 3));
                }
                structure.RefreshSalary(_myOrganization);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Выберите родительский департамент, в который хотите добавить новый департамент");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка: {e.Message} => {e.GetType()}, передайте информацию в отдел технической поддержки для исправления");
            }
        }

        #endregion

        #region Команда удаления департамента из организации
        public ICommand DeleteDepartmentCommand { get; }

        private bool CanDeleteDepartmentCommandExecute(object path) => path is Department;

        public void OnDeleteDepartmentCommandExecuted(object path)
        {
            //главный департамент не трогаем/запрет на удаление
            if (selectedItem.ParentDepartment == null)
            {
                return;
            }
            selectedItem.ParentDepartment.Departments.Remove(selectedItem); //использую встроенный метод удаления департамента
            structure.RefreshSalary(_myOrganization);
        }

        #endregion

        #endregion

        /// <summary>
        /// Модель представления для связи между визуальной частью и логикой программы
        /// </summary>
        public ViewModel()
        {
            CreateCommand = new RelayCommand(OnCreateCommandExecuted, CanCreateCommandExecute);
            OpenCommand = new RelayCommand(OnOpenCommandExecuted, CanOpenCommandExecute);
            SaveCommand = new RelayCommand(OnSaveCommandExecutedAsync, CanSaveCommandExecute);
            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
            AddDepartmentCommand = new RelayCommand(OnAddDepartmentCommandExecuted, CanAddDepartmentCommandExecute);
            DeleteDepartmentCommand = new RelayCommand(OnDeleteDepartmentCommandExecuted, CanDeleteDepartmentCommandExecute);
        }


    }
}
