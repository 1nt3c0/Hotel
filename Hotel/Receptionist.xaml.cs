using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
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

namespace Hotel
{
    /// <summary>
    /// Логика взаимодействия для Receptionist.xaml
    /// </summary>
    public partial class Receptionist : Window
    {
        Entities db = new Entities();
        Сотрудник Employee;
        public Receptionist(Сотрудник employee)
        {
            Employee = employee;
            InitializeComponent();
            DGguests.ItemsSource = db.Бронь.ToList();
        }

        public void Refresh()
        {
            DGguests.ItemsSource = db.Бронь.ToList();
        }

        private void BTNstatys_Click(object sender, RoutedEventArgs e)
        {
            var selectesGuest = DGguests.SelectedItem as Бронь;
            if(DGguests.SelectedItem == null)
            {
                MessageBox.Show("Выберите гостя");
                return;
            }
            if(selectesGuest.Гость1.Код_заселения == 1)
            {
                selectesGuest.Гость1.Код_заселения = 2;
                db.SaveChanges();
                Refresh();
                MessageBox.Show("Гость не заселен");
                return;
            }
            if(selectesGuest.Гость1.Код_заселения == 2)
            {
                selectesGuest.Гость1.Код_заселения = 1;
                db.SaveChanges();
                Refresh();
                MessageBox.Show("Гость заселен");
                return;
            }
            if(selectesGuest.Гость1.Код_заселения == 3)
            {
                MessageBox.Show("Нельзя изменить статус заселения у выселенного гостя");
                return;
            }
            
        }

        private void TBsearchGuest_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Surname = TBsearchGuest.Text;
            var user = db.Бронь.Where(x => x.Гость1.Фамилия.Contains(Surname)).ToList();
            DGguests.ItemsSource = user;
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

        private void BTNstatys_CheckOut_Click(object sender, RoutedEventArgs e)
        {
            var selectesGuest = DGguests.SelectedItem as Бронь;
            if (DGguests.SelectedItem == null)
            {
                MessageBox.Show("Выберите гостя");
                return;
            }
            var result = MessageBox.Show("Вы уверены, что хотите выселить гостя?",
                              "Подтверждение",
                              MessageBoxButton.YesNo,
                              MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                selectesGuest.Гость1.Код_заселения = 3;
                selectesGuest.Номер.Статус_номера = 2;
                db.SaveChanges();
                Refresh();
                MessageBox.Show("Гость выселен");
            }
            else
            {
                
            }
        }

        private void BTNdelBooking_Click(object sender, RoutedEventArgs e)
        {
            var selectesGuest = DGguests.SelectedItem as Бронь;
            if (DGguests.SelectedItem == null)
            {
                MessageBox.Show("Выберите гостя");
                return;
            }
            if (selectesGuest.Гость1.Код_заселения == 3)
            {
                db.Бронь.Remove(selectesGuest);
                db.SaveChanges();
                Refresh();
                MessageBox.Show("Бронь удалена");
            }
            else
            {
                MessageBox.Show("Бронь можно удалить когда гость Выселен");
            }
            
        }
    }
}
