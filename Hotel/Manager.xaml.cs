using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library_ProkoshevYP;

namespace Hotel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Manager : Window
    {
        Entities db = new Entities();
        Сотрудник Employee;

        public Manager(Сотрудник employee)
        {
            InitializeComponent();
            Employee = employee;
            DGguest.ItemsSource = db.Гость.Where(x => !db.Бронь.Any(y => y.Гость == x.Код)).ToList();
            RefreshGuest();
        }

        void RefreshGuest()
        {
            CBchooseGuest.ItemsSource = db.Гость.Where(x => !db.Бронь.Any(y => y.Гость == x.Код)).ToList();
        }

        private void BTNaddGuest_Click(object sender, RoutedEventArgs e)
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

            if(TBnumPhone.Text == "")
            {
                MessageBox.Show("Введите номер телефона");
                return;
            }
            if (TBnumPhone.Text.Length != 11 || !TBnumPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Номер телефона должен состоять из 11 цифр");
                return;
            }
            
            if (TBpassportSeries.Text == "")
            {
                MessageBox.Show("Заполните серию паспорта");
                return;
            }
            if (TBpassportSeries.Text.Length !=4 || !TBpassportSeries.Text.All(char.IsDigit))
            {
                MessageBox.Show("Серия паспорта должна состоять из 4 цифр");
                return;
            }

            if (TBpassportNum.Text == "")
            {
                MessageBox.Show("Заполните номер паспорта");
                return;
            }
            if (TBpassportNum.Text.Length != 6 || !TBpassportNum.Text.All(char.IsDigit))
            {
                MessageBox.Show("Номер паспорта должен состоять из 6 цифр");
                return;
            }

            if (DPborn.Text == "")
            {
                MessageBox.Show("Заполните дату рождения");
                return;
            }

            string Surname = TBsurname.Text;
            string Name = TBname.Text;
            string Patronymic = TBpatronynic.Text;
            string NumPhone = TBnumPhone.Text;
            string PasSeries = TBpassportSeries.Text;
            string PasNum = TBpassportNum.Text;
            DateTime BirthDate = DPborn.SelectedDate.Value;
            DateTime Now = DateTime.Today;
            int age = Now.Year - BirthDate.Year;

            if (BirthDate.Date > Now.AddYears(-age))
            {
                age--;
            }
            if (age < 18)
            {
                MessageBox.Show($"Вам нет 18 лет.");
                return;
            }

            var newPassport = new Паспорт
            {
                Серия = TBpassportSeries.Text,
                Номер = TBpassportNum.Text,
                Дата_рождения = BirthDate.Date,
            };  

            db.Паспорт.Add(newPassport);


            var newGuest = new Гость
            {
                Фамилия = Surname,
                Имя = Name,
                Отчество = Patronymic,
                Номер_телефона = NumPhone,
                Код_паспорта = newPassport.Код,
                Код_заселения = 2
            };
            
            db.Гость.Add(newGuest);
            db.SaveChanges();
            ClearGuest();
            RefreshGuest();
            DGguest.ItemsSource = db.Гость.Where(x => !db.Бронь.Any(y => y.Гость == x.Код)).ToList();
            MessageBox.Show("Гость добавлен");
        }

        private void CBtypeRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный элемент из первого ComboBox
            var selectedItem = CBtypeRoom.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                // Получаем текст выбранного элемента
                string selectedRoomType = selectedItem.Content.ToString();

                // Фильтруем свободные номера по выбранному типу
                var freeRooms = db.Номер
                    .Where(n => n.Статус_номера == 2 && n.Вместимость == selectedRoomType)
                    .ToList();

                // Устанавливаем результат в второй ComboBox
                CBfreeRooms.ItemsSource = freeRooms;
            }
        }

        private void BTNbooking_Click(object sender, RoutedEventArgs e)
        {
            if(CBtypeRoom.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип номера");
                return;
            }
            if(CBfreeRooms.SelectedItem == null)
            {
                MessageBox.Show("Выберите свободный номер");
                return;
            }
            if(CBchooseGuest.SelectedItem == null)
            {
                MessageBox.Show("Выберите гостя");
                return;
            }

            if(DPfirstDate.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату начала брони");
                return;
            }
            if(DPfirstDate.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Нельзя указать дату текущую дату");
                return;
            }
            if(DPlastDate.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату конца брони");
                return;
            }
            if(DPlastDate.SelectedDate < DPfirstDate.SelectedDate)
            {
                MessageBox.Show("Конечная дата бронирования не может быть раньше чем начальная");
                return;
            }
            int lastDay = DPlastDate.SelectedDate.Value.Day;
            int firstDay = DPfirstDate.SelectedDate.Value.Day;

            DateTime datefirst = DPfirstDate.SelectedDate.Value.Date;
            DateTime datelast = DPlastDate.SelectedDate.Value.Date;

            int Days = lastDay - firstDay;
            ++Days;
            Номер num = CBfreeRooms.SelectedItem as Номер;
            //double Sum = (int)num.Цена_за_ночь * Days;
            Гость guest = CBchooseGuest.SelectedItem as Гость;


            var newBooking = new Бронь
            {
                Сотрудник = Employee.Код,
                //Стоимость = Sum,
                Дата_начала_бронирования = datefirst,
                Дата_конца_бронирования = datelast,
                Гость = guest.Код,
                Код_номера = num.Код,
                Количество_дней = Days
            };
            num.Статус_номера = 1;

            db.Бронь.Add(newBooking);
            db.SaveChanges();
            ClearBookong();
            RefreshGuest();
            DGguest.ItemsSource = db.Гость.Where(x => !db.Бронь.Any(y => y.Гость == x.Код)).ToList();
            MessageBox.Show("Бронь оформлена");
        }
        void ClearBookong()
        {
            CBtypeRoom.SelectedItem = null;
            CBfreeRooms.SelectedItem = null;
            CBfreeRooms.ItemsSource = null;
            CBchooseGuest.SelectedItem = null;
            DPfirstDate.SelectedDate = null;
            DPlastDate.SelectedDate = null;
        }
        void ClearGuest()
        {
            TBsurname.Text = null;
            TBname.Text = null;
            TBpatronynic.Text = null;
            TBnumPhone.Text = null;
            TBpassportSeries.Text = null;
            TBpassportNum.Text = null;
            DPborn.SelectedDate = null;
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

        private void BTNdelGuest_Click(object sender, RoutedEventArgs e)
        {
            var selectedGuest = DGguest.SelectedItem as Гость;
            if (DGguest.SelectedItem == null)
            {
                MessageBox.Show("Выберите гостя");
                return;
            }
            db.Гость.Remove(selectedGuest);
            db.SaveChanges();
            DGguest.ItemsSource = db.Гость.Where(x => !db.Бронь.Any(y => y.Гость == x.Код)).ToList();
            RefreshGuest();
            MessageBox.Show("Гость удален");
        }
    }
}
