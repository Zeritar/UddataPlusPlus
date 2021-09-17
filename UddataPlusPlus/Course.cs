using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UddataPlusPlus
{
    public enum CourseType
    {
        Dansk,
        Engelsk,
        Matematik,
        Biologi,
        Geografi,
        Samfundsfag,
        Religion
    }

    public class Course
    {
        public int Id { get; set; }
        public CourseType CourseType { get; set; }
        public int FK_TeacherID { get; set; }
        public string ClassName { get; set; }
        public List<CourseStudent> Students { get; set; }
    }
}
