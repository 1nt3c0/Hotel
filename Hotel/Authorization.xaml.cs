using System;
using System.Collections.Generic;
using System.IO;
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
using EasyCaptcha.Wpf;

namespace Hotel
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        string answer = "";
        bool hasFailedAttempt = false;
        Entities db = new Entities();
        public Authorization()
        {
            InitializeComponent();
            Refresh();

        }
        public void Refresh()
        {
            MyCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
            answer = MyCaptcha.CaptchaText;
        }

        private void BTNcaptcha_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        void OpenWindow(Window window)
        {
            window.Show();
            this.Close();
        }

        private void BTNauthorization_Click(object sender, RoutedEventArgs e)
        {
            string login = TBlogin.Text;
            string password = TBpassword.Text;
            var user = db.Сотрудник.FirstOrDefault(x => x.Логин == login && x.Пароль == password);
            if (user != null && (!hasFailedAttempt || TBcaptcha.Text == answer))
            {
                hasFailedAttempt = false;
                switch (user.Роль.Value)
                {
                    case 1:
                        OpenWindow(new Manager(user));
                        break;
                    case 2:
                        OpenWindow(new Receptionist(user));
                        break;
                    case 3:
                        OpenWindow(new Admin(user));
                        break;
                }
            }
            else
            {
                SPcaptcha.Visibility = Visibility.Visible;
                hasFailedAttempt = true;
                Refresh();
                TBcaptcha.Text = "";
                return;
            }
           
        }
    }
}
