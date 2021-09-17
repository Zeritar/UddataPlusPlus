USE master
GO

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases 
WHERE name = 'UddataDB'))
BEGIN
    ALTER DATABASE UddataDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE UddataDB
END
GO

CREATE DATABASE UddataDB
GO
USE UddataDB
GO

CREATE TABLE StudentTable (
    StudentID INT IDENTITY PRIMARY KEY,
    StudentName NVARCHAR(255),
    Username NVARCHAR(255),
    PassHash NVARCHAR(255),
    Warnings INT
)

CREATE TABLE TeacherTable (
    TeacherID INT IDENTITY PRIMARY KEY,
    TeacherName NVARCHAR(255),
    Username NVARCHAR(255),
    PassHash NVARCHAR(255),
    CoffeeClubMember BIT
)

CREATE TABLE CourseTable (
    CourseID INT IDENTITY PRIMARY KEY,
    CourseType INT,
    CourseName NVARCHAR(255),
    FK_TeacherID INT
)

CREATE TABLE CourseStudentTable (
    FK_CourseID INT,
    FK_StudentID INT,
    Grade INT
)
GO

CREATE VIEW StudentCourses AS
SELECT StudentID, StudentName, CourseID, CourseType, CourseName, FK_TeacherID, Grade FROM StudentTable
INNER JOIN CourseStudentTable ON FK_StudentID = StudentID
INNER JOIN CourseTable ON FK_CourseID = CourseID
GO

CREATE VIEW TeacherCourses AS
SELECT TeacherID, TeacherName, CourseID, CourseType, CourseName FROM TeacherTable
INNER JOIN CourseTable ON FK_TeacherID = TeacherID
GO

CREATE VIEW CourseStudents AS
SELECT CourseID, CourseType, CourseName, StudentID, StudentName, Grade, Warnings FROM CourseTable
INNER JOIN CourseStudentTable ON FK_CourseID = CourseID
INNER JOIN StudentTable ON FK_StudentID = StudentID
GO

CREATE VIEW AllUsersForLogin AS
SELECT StudentTable.Username, StudentTable.PassHash FROM StudentTable
UNION
SELECT TeacherTable.Username, TeacherTable.PassHash FROM TeacherTable
GO

CREATE VIEW StudentUsers AS
SELECT StudentID, Username, StudentName FROM StudentTable
GO

CREATE VIEW TeacherUsers AS
SELECT TeacherID, Username, TeacherName FROM TeacherTable