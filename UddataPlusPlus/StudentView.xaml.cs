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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UddataPlusPlus
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : UserControl
    {
        Main main;
        User user;
        public StudentView(Main main, User user)
        {
            this.main = main;
            this.user = user;
            InitializeComponent();
            tbUsername.Text = user.Username;
            tbFullName.Text = user.FullName;

            fdViewer.Document = GetStudentCourses();
        }

        FlowDocument GetStudentCourses()
        {
            return main.methods.BuildStudentCourseView(user);

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            main.methods.LogOut();
        }
    }
}
