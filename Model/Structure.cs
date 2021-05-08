using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Modul11_UI_HW.Model
{
    class Structure
    {
        private static Structure instance;

        private Structure()
        { }
        /// <summary>
        /// Синглтон структуры организации
        /// </summary>
        /// <returns></returns>
        public static Structure GetInstance()
        {
            if (instance == null)
            {
                instance = new Structure();
            }
            return instance;
        }
        /// <summary>
        /// Заполнение новой организации псевдослучайными данными
        /// </summary>
        /// <param name="deps"></param>
        public void FillSomeStructure(ObservableCollection<Department> deps) 
        {
            if (deps.Count == 0)
            {
                deps.Add(new Department
                (
                    "Top Secret", 0
                ));
            }
            PopulateStructure(deps[0].Departments, "Department ", 0);
           
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
        private void PopulateStructure(ObservableCollection<Department> deps, string nameDepartment, int countDivisions)
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
                    
                    PopulateStructure(deps[i].Departments, deps[i].NameDepartment + ".", countDivisions - 1);
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
    }
}
