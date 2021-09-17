using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UddataPlusPlus
{
    public class SQLMethods
    {
        const string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=UddataDB;Integrated Security=True";
        public List<string[]> SelectFromTable(string[] query)
        {
            List<string[]> queryResult = new List<string[]>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                {
                    SqlCommand command = new SqlCommand(query[0], connection);
                    try
                    {
                        command.Connection.Open();
                        for (int i = 1; i < query.Length; i++)
                        {
                            command.Parameters.Add($"@var{i}", SqlDbType.NVarChar).Value = query[i];
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                string[] fields = new string[reader.FieldCount];
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    fields[i] = reader[i].ToString();
                                }
                                queryResult.Add(fields);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database Error: {ex}");
                    }
                }
            }
            return queryResult;
        }

        public int? InsertStudentIntoDatabase(string[] query)
        {
            List<string[]> queryResult = new List<string[]>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Prepare the command to be executed on the db
                    using (SqlCommand command = new SqlCommand(query[0], connection))
                    {
                        // Create and set the parameters values 
                        command.Parameters.Add("@uname", SqlDbType.NVarChar).Value = query[1];
                        command.Parameters.Add("@passhash", SqlDbType.NVarChar).Value = query[2];
                        command.Parameters.Add("@fullname", SqlDbType.NVarChar).Value = query[3];

                        var id = command.ExecuteScalar();
                        return (int?)id;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex}");
                    return null;
                }
            }
        }

        public int? InsertTeacherIntoDatabase(string[] query)
        {
            List<string[]> queryResult = new List<string[]>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Prepare the command to be executed on the db
                    using (SqlCommand command = new SqlCommand(query[0], connection))
                    {
                        // Create and set the parameters values 
                        command.Parameters.Add("@uname", SqlDbType.NVarChar).Value = query[1];
                        command.Parameters.Add("@passhash", SqlDbType.NVarChar).Value = query[2];
                        command.Parameters.Add("@fullname", SqlDbType.NVarChar).Value = query[3];
                        command.Parameters.Add("@coffee", SqlDbType.Bit).Value = StringToBool(query[4]);

                        var id = command.ExecuteScalar();
                        return (int?)id;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex}");
                    return null;
                }
            }
        }

        public int? InsertCourseIntoDatabase(string[] query)
        {
            List<string[]> queryResult = new List<string[]>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Prepare the command to be executed on the db
                    using (SqlCommand command = new SqlCommand(query[0], connection))
                    {
                        // Create and set the parameters values 
                        command.Parameters.Add("@ctype", SqlDbType.Int).Value = int.Parse(query[1]);
                        command.Parameters.Add("@cname", SqlDbType.NVarChar).Value = query[2];
                        command.Parameters.Add("@teachID", SqlDbType.Int).Value = int.Parse(query[3]);

                        var id = command.ExecuteScalar();
                        return (int?)id;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex}");
                    return null;
                }
            }
        }

        public bool InsertCourseStudentPairIntoDatabase(string[] query)
        {
            List<string[]> queryResult = new List<string[]>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                try
                {
                    connection.Open();
                    // Prepare the command to be executed on the db
                    using (SqlCommand command = new SqlCommand(query[0], connection))
                    {
                        // Create and set the parameters values 
                        command.Parameters.Add("@sID", SqlDbType.Int).Value = int.Parse(query[1]);
                        command.Parameters.Add("@cID", SqlDbType.Int).Value = int.Parse(query[2]);

                        command.ExecuteScalar();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex}");
                    return false;
                }
            }
        }

        public void TruncateAllTables()
        {

            string[] queries = new string[] {"TRUNCATE TABLE CourseStudentTable",
            "TRUNCATE TABLE CourseTable",
            "TRUNCATE TABLE TeacherTable",
            "TRUNCATE TABLE StudentTable"
            };
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();
                    // Prepare the command to be executed on the db
                    foreach (string query in queries)
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex}");
                }
            }
        }

        bool StringToBool(string str)
        {
            if (str == "True")
            {
                return true;
            }
            else
                return false;
        }

        // TODO: Build commands from string arrays to reduce number of Insert methods

        public string[] LoginQuery(string username, string passhash)
        {
            return new string[] { @"SELECT Username, PassHash From AllUsersForLogin WHERE Username = @var1 AND PassHash = @var2", username, passhash };
        }

        public string[] StudentInsertQuery(string username, string passhash, string fullname)
        {
            return new string[] { @"INSERT INTO StudentTable (Username, PassHash, StudentName, Warnings) OUTPUT INSERTED.StudentID VALUES (@uname, @passhash, @fullname, 0)", username, passhash, fullname };
        }

        public string[] TeacherInsertQuery(string username, string passhash, string fullname, bool coffee)
        {
            return new string[] { @"INSERT INTO TeacherTable (Username, PassHash, TeacherName, CoffeeClubMember) OUTPUT INSERTED.TeacherID VALUES (@uname, @passhash, @fullname, @coffee)", username, passhash, fullname, coffee.ToString() };
        }

        public string[] CourseInsertQuery(int courseType, string courseName, int teacherID)
        {
            return new string[] { @"INSERT INTO CourseTable (CourseType, CourseName, FK_TeacherID) OUTPUT INSERTED.CourseID VALUES (@ctype, @cname, @teachID)", courseType.ToString(), courseName, teacherID.ToString() };
        }

        public string[] CourseStudentLinkQuery(int courseID, int studentID)
        {
            return new string[] { @"INSERT INTO CourseStudentTable (FK_StudentID, FK_CourseID) VALUES (@sID, @cID)", studentID.ToString(),  courseID.ToString()};
        }

        public string[] StudentUsernamePollQuery(string username)
        {
            return new string[] { @"SELECT * FROM StudentUsers WHERE Username = @var1", username};
        }

        public string[] TeacherUsernamePollQuery(string username)
        {
            return new string[] { @"SELECT * FROM TeacherUsers WHERE Username = @var1", username};
        }

        public string[] TeacherIDPollQuery(int teacherID)
        {
            return new string[] { @"SELECT TeacherName FROM TeacherUsers WHERE TeacherID = @var1", teacherID.ToString() };
        }

        public string[] StudentCourseQuery(int studentID)
        {
            return new string[] { @"SELECT * FROM StudentCourses WHERE StudentID = @var1", studentID.ToString()};
        }

        public string[] CourseStudentQuery(int courseID)
        {
            return new string[] { @"SELECT * FROM CourseStudents WHERE CourseID = @var1", courseID.ToString() };
        }

        public string[] TeacherCourseQuery(int teacherID)
        {
            return new string[] { @"SELECT * FROM TeacherCourses WHERE TeacherID = @var1", teacherID.ToString()};
        }
    }
}
