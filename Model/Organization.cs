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

        /// <summary>
        /// Метод обновления зарплаты руководства
        /// </summary>
        /// <param name="organization"></param>
        public void RefreshSalary(ObservableCollection<Department> organization)
        {
            foreach (var item in organization) //пересчитываем зарплаты руководства
            {
                item.ManagerDepartment.ManagerGetSalary(item);
            }
        }

        
    }
}
