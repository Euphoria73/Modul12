using System;
using System.Collections.Generic;
using System.Text;

namespace Modul11_UI_HW.Model
{
    public static class EmployeeGenerator
    {       
        public static Random random = new Random();

        /// <summary>
        /// Генерация имени и фамилии сотрудника
        /// </summary>
        /// <returns>вощврашает случайное имя и фамилию сотрудника</returns>
        public static string[] GetFullName()
        {
            string[] fullName = new string[2];

            var manFirstNames = new string[]
            {
                "Александр",
                "Максим",
                "Михаил",
                 "Артём",
                 "Даниил",
                 "Иван",
                 "Дмитрий",
                 "Кирилл",
                 "Андрей",
                 "Матвей",
                 "Егор",
                 "Илья",
                 "Марк",
                 "Тимофей",
                 "Роман",
                 "Никита",
                 "Алексей",
                 "Лев",
                 "Владимир",
                 "Фёдор",
                 "Ярослав",
                 "Константин",
                 "Сергей",
                 "Степан",
                 "Николай"
            };

            var womanFirstNames = new string[]
            {
                "София",
                "Мария",
                "Анна",
                 "Виктория",
                 "Алиса",
                 "Анастасия",
                 "Полина",
                 "Елизавета",
                 "Александра",
                 "Дарья",
                 "Варвара",
                 "Екатерина",
                 "Ксения",
                 "Арина",
                 "Ева",
                 "Вероника",
                 "Василиса",
                 "Милана",
                 "Валерия",
                 "Ульяна",
                 "Кира",
                 "Вера",
                 "Таисия",
                 "Софья",
                 "Маргарита"
            };

            var lastNames = new string[]
            {
                "Смит",
                "Джонсон",
                "Уильямс",
                 "Джонс",
                 "Браун",
                 "Дэвис",
                 "Миллер",
                 "Уилсон",
                 "Мур",
                 "Тейлор",
                 "Андерсон",
                 "Томас",
                 "Джексон",
                 "Уайт",
                 "Харрис",
                 "Мартин",
                 "Томпсон",
                 "Гарсиа",
                 "Мартинес",
                 "Робинсон",
                 "Кларк",
                 "Родригес",
                 "Льюис",
                 "Ли",
                 "Уокер",
                 "Холл",
                 "Аллен",
                 "Янг",
                 "Эрнандес",
                 "Кинг",
                 "Райт",
                 "Лопес",
                 "Хилл",
                 "Скотт",
                 "Грин",
                 "Адамс",
                 "Бейкер",
                 "Фостер"
            };

            var chooseGenderEmpployee = random.Next(0, 2);

            switch (chooseGenderEmpployee)
            {
                case 0:
                    fullName[0] = manFirstNames[random.Next(0, manFirstNames.Length)];
                    fullName[1] = lastNames[random.Next(0, lastNames.Length)];
                    return fullName;
                case 1:
                    fullName[0] = womanFirstNames[random.Next(0, womanFirstNames.Length)];
                    fullName[1] = lastNames[random.Next(0, lastNames.Length)];
                    return fullName;
                default:
                    return null;
            }
        }
    }
}
