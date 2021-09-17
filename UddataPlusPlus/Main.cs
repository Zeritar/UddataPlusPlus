using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UddataPlusPlus
{
    public class Main
    {
        public List<Course> courses = new List<Course>();
        public List<Teacher> teachers = new List<Teacher>();
        public List<Student> students = new List<Student>();
        public MainWindow window;
        public Methods methods;
        public Main(MainWindow mainWindow)
        {
            window = mainWindow;
            methods = new Methods(this);
            methods.Initialize();
        }
    }
}
