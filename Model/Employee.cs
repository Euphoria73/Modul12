using System;
using System.Collections.Generic;
using System.Text;

namespace Modul11_UI_HW.Model
{
    class Employee
    {
        private static int totalEmployees;
        public int ID { get; private set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public int Salary { get; set; }

        /// <summary>
        /// Конструктор сотрудника
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="age">Возраст</param>
        public Employee(string firstName, string lastName)
        {
            //Генерация данных сотрудника при пустых параметрах
            var fullName = EmployeeGenerator.GetFullName();
            if (firstName == "")
            {
                this.FirstName = fullName[0];
            }
            if (lastName == "")
            {
                this.LastName = fullName[1];
            }
            this.Age = EmployeeGenerator.random.Next(18, 66);

            ID = GetID();
        }
        /// <summary>
        /// Пустой конструктор сотрудника
        /// </summary>
        public Employee()
        {

        }

        //Получение ID нового сотрудника в зависимости от общего числа сотрудников
        private int GetID()
        {
            totalEmployees += 1;
            return totalEmployees;
        }

        /// <summary>
        /// Абстракция получения зарплаты сотрудника
        /// </summary>
        /// <returns></returns>
        public virtual int GetSalary()
        {
            Salary = 0; //dollars
            return Salary;
        }
    }

    class Intern : Employee
    {
        /// <summary>
        /// Конструктор интерна
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        public Intern(string firstName, string lastName) : base(firstName, lastName)
        {
            Salary = GetSalary(); //dollars
            Position = "Интерн";
        }

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public Intern() : this("", "")
        {

        }

        /// <summary>
        /// Расчёт зарплаты интерна
        /// </summary>
        /// <returns>Зарплата интерна</returns>
        public override int GetSalary()
        {
            int monthRate = 500; // максимальная зарплата в месяц в долларах
            int maxWorkDaysInMonth = 20;
            int dayRate = monthRate / maxWorkDaysInMonth;
            int currentworkDaysInMonth = EmployeeGenerator.random.Next(0, 32);
            int monthSalary = dayRate * currentworkDaysInMonth;
            return monthSalary;
        }
    }

    class Worker : Employee
    {
        /// <summary>
        /// Конструктор рабочего
        /// </summary>
        /// <param name="firstName">Имя</param>
        /// <param name="lastName">Фамилия</param>
        /// <param name="age">Возраст</param>
        /// <param name="workDaysInMonth">Число рабочих дней в месяц</param>
        public Worker(string firstName, string lastName) : base(firstName, lastName)
        {

            Salary = GetSalary(); //dollars
            Position = "Рабочий";
        }

        /// <summary>
        /// Пустой конструктор Рабочего
        /// </summary>
        public Worker() : this("", "")
        {

        }

        /// <summary>
        /// Расчёт зарплаты рабочего
        /// </summary>
        /// <returns>месячная зарплата рабочего</returns>
        public override int GetSalary()
        {
            int hourRate = 12; //dollars
            int workTimeInDay = 8;
            int currentworkDaysInMonth = EmployeeGenerator.random.Next(0, 32);
            int monthSalary = hourRate * workTimeInDay * currentworkDaysInMonth;
            return monthSalary;
        }
    }

    class Manager : Employee
    {
        public Manager(string firstName, string lastName) : base(firstName, lastName)
        {

        }

        public Manager() : this("", "")
        {

        }

        public int ManagerGetSalary(Department currentDepartment)
        {
            currentDepartment.ManagerDepartment.Salary = 0;

            //прибавляем к зарплате менеджера 15% зарплаты других сотрудников в департаменте
            if (currentDepartment.Employees.Count != 0)
            {
                foreach (var employee in currentDepartment.Employees)
                {
                    currentDepartment.ManagerDepartment.Salary += (int)(employee.Salary * 0.15);
                }
            }

            //если есть ведомства в департементе
            if (currentDepartment.Departments != null)
            {
                foreach (var subdivision in currentDepartment.Departments)
                {
                    currentDepartment.ManagerDepartment.Salary += ManagerGetSalary(subdivision); //рекурсия увеличения зарплаты для менеджера по всем ведомствам в департаменте
                }
                if (currentDepartment.ManagerDepartment.Salary < 1300)
                {
                    currentDepartment.ManagerDepartment.Salary = 1300;
                }
                return currentDepartment.ManagerDepartment.Salary;
            }
            //если нет ведомств, проверяем зарплату на минимальную оплату
            if (currentDepartment.ManagerDepartment.Salary < 1300)
            {
                return 1300;
            }

            return currentDepartment.ManagerDepartment.Salary;
        }        
    }
}
