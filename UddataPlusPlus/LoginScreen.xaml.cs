using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace UddataPlusPlus
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : UserControl
    {
        Main main;
        Methods methods;
        public LoginScreen(Main main)
        {
            this.main = main;
            methods = main.methods;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool loginSuccessful = methods.Login(tbName.Text, tbPass.Password);

            if (loginSuccessful)
            {
                bool isTeacher = main.methods.isTeacher(tbName.Text);
                User user = main.methods.GetUserInfoFromUsername(tbName.Text, isTeacher);

                tbDBError.Visibility = Visibility.Visible;
                if (isTeacher)
                {
                    methods.ChangeView(View.TeacherView, user);
                }
                else
                {
                    methods.ChangeView(View.StudentView, user);
                }
                

            }
            else
            {
                tbDBError.Visibility = Visibility.Visible;
                tbDBError.Text = "De indtastede oplysninger matcher ikke en bruger i databasen.";
            }
        }

        private void btnFakeData_Click(object sender, RoutedEventArgs e)
        {
            tbDBStatus.Visibility = Visibility.Visible;
            tbDBStatus.Text = "Nulstiller database...";
            btnLogin.IsEnabled = false;

            Task.Factory.StartNew(new Action(GenerateFakeData)).ContinueWith(
                task => FinishFakeData(),
                TaskScheduler.FromCurrentSynchronizationContext());

        }

        void FinishFakeData()
        {
            tbDBStatus.Text = "Database nulstillet.";
            btnLogin.IsEnabled = true;
        }

        void GenerateFakeData()
        {
           methods.GenerateFakeData(tbDBStatus);
        }
    }
}
