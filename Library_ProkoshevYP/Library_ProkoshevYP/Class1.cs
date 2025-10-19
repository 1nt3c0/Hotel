using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library_ProkoshevYP
{
    public static class Class1
    {
        public static bool ValidLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return false;

            // Логин должен содержать только буквы и цифры, и быть длиной от 3 до 20 символов
            return Regex.IsMatch(login, @"^[a-zA-Z0-9]{3,20}$");
        }

        public static bool ValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Пароль должен содержать одну строчную букву и быть длиной от 8 символов
            return Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-zA-Z])[a-zA-Z0-9]{8,}$");
        }

        public static bool ValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Проверка корректности формата электронной почты
            return Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$");
        }
        public static bool ValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            //Проверка корректности номера телефона
            return Regex.IsMatch(phone, @"^(?:\+7|8)\d{10}$");
        }
        public static bool ValidDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return false;
            return Regex.IsMatch(date, @"^\d{2}\.\d{2}\.\d{4}$");
        }
    }
}
