using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library_ProkoshevYP;

namespace Hotel
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        Entities db = new Entities();
        Сотрудник Employee;
        public Admin(Сотрудник employee)
        {
            Employee = employee;
            InitializeComponent();
            CBrole.ItemsSource = db.Роли.Where(x => x.Код == 1 || x.Код == 2).ToList();
            RefrashEmployee();
        }

        void RefrashEmployee()
        {
            DGEmployee.ItemsSource = db.Сотрудник.Where(x => x.Роль != 3).ToList();

        }
        private void BTNaddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (TBsurname.Text == "")
            {
                MessageBox.Show("Заполните фамилию");
                return;
            }
            if (TBsurname.Text.Length < 2 || TBsurname.Text.Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Фамилия не может быть меньше 2 букв, и в ней не должно быть цифр");
                return;
            }

            if (TBname.Text == "")
            {
                MessageBox.Show("Заполните имя");
                return;
            }
            if (TBname.Text.Length < 2 || TBname.Text.Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Имя не может быть меньше 2 букв, и в нем не должно быть цифр");
                return;
            }

            if (TBpatronynic.Text == "")
            {
                MessageBox.Show("Заполните отчество");
                return;
            }
            if (TBpatronynic.Text.Length < 2 || TBpatronynic.Text.Any(c => char.IsDigit(c)))
            {
                MessageBox.Show("Отчество не может быть меньше 2 букв,и в нем не должно быть цифр");
                return;
            }

            if (TBnumPhone.Text == "")
            {
                MessageBox.Show("Введите номер телефона");
                return;
            }
            if (TBnumPhone.Text.Length != 11 || !TBnumPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Номер телефона должен состоять из 11 цифр");
                return;
            }

            if(TBemail.Text == "")
            {
                MessageBox.Show("Введите email");
                return;
            }
            if (!Class1.ValidEmail(TBemail.Text))
            {
                MessageBox.Show("Некорректная почта");
                return;
            }

            if(TBlogin.Text == "")
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (!Class1.ValidLogin(TBlogin.Text))
            {
                MessageBox.Show("Некорректный логин (Логин должен содержать только буквы и цифры, и быть длиной от 3 до 20 символов)");
                return;
            }

            if(TBpassword.Text == "")
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            if (!Class1.ValidPassword(TBpassword.Text))
            {
                MessageBox.Show("Некорректный пароль (Пароль должен содержать одну строчную букву и быть длиной от 3 символов)");
                return;
            }

            if(CBrole.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль");
                return;
            }

            if (DPborn.Text == "")
            {
                MessageBox.Show("Заполните дату рождения");
                return;
            }

            DateTime BirthDate = DPborn.SelectedDate.Value;
            DateTime Now = DateTime.Today;
            int age = Now.Year - BirthDate.Year;

            if (BirthDate.Date > Now.AddYears(-age))
            {
                age--;
            }
            if (age < 18)
            {
                MessageBox.Show($"Вам {age} лет. Вы несовершеннолетний.");
                return;
            }


            string Surname = TBsurname.Text;
            string Name = TBname.Text;
            string Patronynic = TBpatronynic.Text;
            string NumPhone = TBnumPhone.Text;
            string Email = TBemail.Text;
            string Login = TBlogin.Text;
            string Password = TBpassword.Text;
            Роли Role = CBrole.SelectedItem as Роли;

            var newEmployee = new Сотрудник
            {
                Фамилия = Surname,
                Имя = Name,
                Отчество = Patronynic,
                Роль = Role.Код,
                Email = Email,
                Логин = Login,
                Пароль = Password,
                Телефон = NumPhone,
                Дата_рождения = BirthDate.Date,
            };

            db.Сотрудник.Add(newEmployee);
            db.SaveChanges();
            ClearEmployee();
            RefrashEmployee();
            MessageBox.Show("Сотрудник добавлен");
        }
        void ClearEmployee()
        {
            TBsurname.Text = null;
            TBname.Text = null;
            TBpatronynic.Text = null;
            TBnumPhone.Text = null;
            TBemail.Text = null;
            TBlogin.Text = null;
            TBpassword.Text = null;
            CBrole.SelectedItem = null;
        }

        private void BTNdelEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = DGEmployee.SelectedItem as Сотрудник;
            if(selectedEmployee != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?",
                              "Подтверждение",
                              MessageBoxButton.YesNo,
                              MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    db.Сотрудник.Remove(selectedEmployee);
                    db.SaveChanges();
                    RefrashEmployee();
                    MessageBox.Show("Сотрудник удален");
                }
                else
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника");
            }
        }
        void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }
        private void BTNback_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new Authorization());
        }
    }
}
