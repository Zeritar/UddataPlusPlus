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
    /// Interaction logic for TeacherView.xaml
    /// </summary>
    public partial class TeacherView : UserControl
    {
        Main main;
        User user;
        public TeacherView(Main main, User user)
        {
            this.main = main;
            this.user = user;
            InitializeComponent();
            tbUsername.Text = user.Username;
            tbFullName.Text = user.FullName;
            
            fdViewer.Document = GetTeacherCourses();
        }

        FlowDocument GetTeacherCourses()
        {
            return main.methods.BuildTeacherCourseView(user);

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            main.methods.LogOut();
        }
    }
}
