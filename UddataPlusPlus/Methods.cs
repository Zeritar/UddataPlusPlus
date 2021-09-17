using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UddataPlusPlus
{
    public partial class Methods
    {
        Main main;
        SQLMethods sQLMethods = new SQLMethods();
        PassHasher passHasher = new PassHasher();
        public Methods(Main main)
        {
            this.main = main;
        }
        public void CreateStudent(string name)
        {
            Student student = new Student()
            {
                Name = name,
                Warnings = 0,
                Username = GenerateUserName(name),
                Password = GeneratePassword()
            };

            int? id = sQLMethods.InsertStudentIntoDatabase(sQLMethods.StudentInsertQuery(
                student.Username,
                passHasher.Hash(student.Username, student.Password),
                student.Name
                ));

            if (id != null)
            {
                student.Id = (int)id;
                main.students.Add(student);
            }
        }

        public void CreateTeacher(string name, bool coffeeClub)
        {
            Teacher teacher = new Teacher()
            {
                Name = name,
                CoffeeClubMember = coffeeClub,
                Username = GenerateUserName(name),
                Password = GeneratePassword()
            };



            int? id = sQLMethods.InsertTeacherIntoDatabase(sQLMethods.TeacherInsertQuery(
                teacher.Username,
                passHasher.Hash(teacher.Username, teacher.Password),
                teacher.Name,
                teacher.CoffeeClubMember
                ));

            if (id != null)
            {
                teacher.Id = (int)id;
                main.teachers.Add(teacher);
            }
        }

        public void CreateTestTeacher()
        {
            Teacher teacher = new Teacher()
            {
                Name = "Test Lærer",
                CoffeeClubMember = true,
                Username = "Brugernavn",
                Password = "Password"
            };

            int? id = sQLMethods.InsertTeacherIntoDatabase(sQLMethods.TeacherInsertQuery(
                teacher.Username,
                passHasher.Hash(teacher.Username, teacher.Password),
                teacher.Name,
                teacher.CoffeeClubMember
                ));

            if (id != null)
            {
                teacher.Id = (int)id;
                main.teachers.Add(teacher);
            }
        }

        public void CreateTestStudent()
        {
            Student student = new Student()
            {
                Name = "Test Elev",
                Warnings = 0,
                Username = "Brugernavn2",
                Password = "Password"
            };

            int? id = sQLMethods.InsertStudentIntoDatabase(sQLMethods.StudentInsertQuery(
                student.Username,
                passHasher.Hash(student.Username, student.Password),
                student.Name
                ));

            if (id != null)
            {
                student.Id = (int)id;
                main.students.Add(student);
            }
        }

        public void CreateCourse(CourseType courseType, string courseName, int teacherID, List<int> studentIDs)
        {
            Course course = new Course()
            {
                CourseType = courseType,
                FK_TeacherID = teacherID,
                ClassName = courseName,
                Students = new List<CourseStudent>()
            };

            int? id = sQLMethods.InsertCourseIntoDatabase(sQLMethods.CourseInsertQuery(
                (int)course.CourseType,
                course.ClassName,
                course.FK_TeacherID
                ));

            if (id != null)
            {
                course.Id = (int)id;

                foreach (int studentID in studentIDs)
                {
                    course.Students.Add(new CourseStudent() { StudentID = studentID });

                    sQLMethods.InsertCourseStudentPairIntoDatabase(sQLMethods.CourseStudentLinkQuery(
                    course.Id,
                    studentID
                    ));
                }

                main.courses.Add(course);
            }
        }

        public void GenerateFakeData(TextBlock tbStatus)
        {
            sQLMethods.TruncateAllTables();

            // Create teachers
            CreateTeacher("John Cena", false);
            CreateTeacher("Torben Flemmingsen", true);
            CreateTeacher("Hans Andersen", false);
            CreateTeacher("Birger Christensen", true);
            CreateTeacher("Anthon Berg", false);

            CreateTestTeacher();

            // Create students
            CreateStudent("Ib Jensen");
            CreateStudent("Lasse Hansen");
            CreateStudent("Mads Larsen");
            CreateStudent("Thomas Møller");
            CreateStudent("Henriette Haarding");
            CreateStudent("Lukas Mortensen");
            CreateStudent("Mette Hansson");
            CreateStudent("Julie Dahl");
            CreateStudent("Lone Albrechsen");
            CreateStudent("Søster Lagkage");

            CreateTestStudent();

            // Disse elever skal have dansk, engelsk og matematik
            List<int> basicStudents = new List<int>()
                {1,2,3,4,5,6,7,8,9,11};

            // Dansk
            CreateCourse(CourseType.Dansk, "2108DAN1", 6, basicStudents);

            // Engelsk
            CreateCourse(CourseType.Engelsk, "2108ENG1", 6, basicStudents);

            // Matematik
            CreateCourse(CourseType.Matematik, "2108MAT1", 6, basicStudents);
        }

        public void Initialize()
        {
            ChangeView(View.Login, null);
        }

        private string GenerateUserName(string name)
        {
            Random rnd = new Random();
            return name[0..4].Replace(' ', '_') + rnd.Next(0, 10000).ToString().PadLeft(4, '0');
        }

        private string GeneratePassword()
        {
            Random rnd = new Random();
            return "Kode-" + rnd.Next(0, 10000).ToString().PadLeft(4, '0');
        }

        public bool Login(string username, string password)
        {
            string hashedPass = passHasher.Hash(username, password);
            bool success = sQLMethods.SelectFromTable(sQLMethods.LoginQuery(username, hashedPass)).Count > 0 ? true : false;
            return success;
        }

        public void LogOut()
        {
            ChangeView(View.Login, null);
        }

        public bool isTeacher(string username)
        {
            return sQLMethods.SelectFromTable(sQLMethods.TeacherUsernamePollQuery(username)).Count > 0 ? true : false;
        }

        public User GetUserInfoFromUsername(string username, bool isTeacher)
        {
            User user = new User();
            if (isTeacher)
            {
                string[] query = sQLMethods.SelectFromTable(sQLMethods.TeacherUsernamePollQuery(username))[0];
                user.IsTeacher = true;
                user.UserID = int.Parse(query[0]);
                user.Username = username;
                user.FullName = query[2];
            }
            else
            {
                string[] query = sQLMethods.SelectFromTable(sQLMethods.StudentUsernamePollQuery(username))[0];
                user.UserID = int.Parse(query[0]);
                user.Username = username;
                user.FullName = query[2];
            }
            return user;
        }


        public void ChangeView(View view, User? user)
        {
            main.window.sPanel.Children.Clear();
            switch (view)
            {
                case View.Login:
                    main.window.sPanel.Children.Add(new LoginScreen(main));
                    break;

                case View.StudentView:
                    if (user != null)
                    {
                        main.window.sPanel.Children.Add(new StudentView(main, user));
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(user));
                    }
                    main.window.sPanel.Children.Add(new LoginScreen(main));
                    break;

                case View.TeacherView:
                    if (user != null)
                    {
                        main.window.sPanel.Children.Add(new TeacherView(main, user));
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(user));
                    }
                    main.window.sPanel.Children.Add(new LoginScreen(main));
                    break;

                default:
                    main.window.sPanel.Children.Add(new LoginScreen(main));
                    break;
            }
        }

        public List<Course> GetStudentCourses(User user)
        {
            List<string[]> query = sQLMethods.SelectFromTable(sQLMethods.StudentCourseQuery(user.UserID));
            List<Course> courses = new List<Course>();
            foreach (string[] row in query)
            {
                courses.Add(new Course()
                {
                    Id = int.Parse(row[2]),
                    CourseType = (CourseType)int.Parse(row[3]),
                    ClassName = row[4],
                    FK_TeacherID = int.Parse(row[5])
                });
            }
            return courses;
        }

        public List<Student> GetCourseStudents(Course course)
        {
            List<string[]> query = sQLMethods.SelectFromTable(sQLMethods.CourseStudentQuery(course.Id));
            List<Student> students = new List<Student>();
            if (query.Count > 0)
                foreach (string[] row in query)
                {
                    students.Add(new Student()
                    {
                        Id = int.Parse(row[3]),
                        Name = row[4],
                        Warnings = int.Parse(row[6])
                    });
                }
            return students;
        }

        public List<Course> GetTeacherCourses(User user)
        {
            List<string[]> query = sQLMethods.SelectFromTable(sQLMethods.TeacherCourseQuery(user.UserID));
            List<Course> courses = new List<Course>();
            foreach (string[] row in query)
            {
                courses.Add(new Course()
                {
                    Id = int.Parse(row[2]),
                    CourseType = (CourseType)int.Parse(row[3]),
                    ClassName = row[4]
                });
            }
            return courses;
        }

        public string GetTeacherNameFromID(int teacherID)
        {
            List<string[]> query = sQLMethods.SelectFromTable(sQLMethods.TeacherIDPollQuery(teacherID));

            if (query.Count > 0)
                return query[0][0];
            else
                return "Invalid Teacher ID";
        }
    }
}
